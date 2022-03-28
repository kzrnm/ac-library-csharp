using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace AtCoderAnalyzer.Test
{
    public class SymbolHelperTest
    {
        private readonly CSharpSyntaxTree[] trees = new CSharpSyntaxTree[1];
        private readonly CSharpCompilation compilation;
        public SymbolHelperTest()
        {
            trees[0] = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(@"namespace TestAssembly.TA {
    public class Foo { 
        static ConstraintGenerics<Foo, Bar, IGenerics<Foo, Bar>> field1;
        static IGenerics<Foo, Bar> field2;
        static IGenerics<Foo, IGenerics<Foo, Bar>> field3;
    }
    public struct Bar { }
    public interface IGenerics<T, U> { }
    public class ConstraintGenerics<R, S, Op> where Op : IGenerics<R, S> { }
    public class NestGenerics<R, S, Op> where Op : IGenerics<R, IGenerics<R, S>> { }
}
");
            compilation = CSharpCompilation.Create("Asm", trees);
        }

        [Fact]
        public async Task TypeEqual()
        {
            var root = await trees[0].GetRootAsync();
            var semanticModel = compilation.GetSemanticModel(trees[0], false);
            var nodes = root.DescendantNodes();

            var constructedConstraintGenericsSymbol
                = nodes
                .OfType<TypeDeclarationSyntax>()
                .Single(sy => sy.Identifier.Text == "Foo")
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(sy => sy.Declaration.Variables.Single().Identifier.Text == "field1")
                .Select(sy => semanticModel.GetSymbolInfo(sy.Declaration.Type).Symbol)
                .OfType<INamedTypeSymbol>()
                .Single();

            Assert.Equal(
                constructedConstraintGenericsSymbol.TypeArguments[0],
                ((INamedTypeSymbol)constructedConstraintGenericsSymbol.TypeArguments[2]).TypeArguments[0],
                SymbolEqualityComparer.Default);
            Assert.Equal(
                constructedConstraintGenericsSymbol.TypeArguments[1],
                ((INamedTypeSymbol)constructedConstraintGenericsSymbol.TypeArguments[2]).TypeArguments[1],
                SymbolEqualityComparer.Default);
        }

        [Fact]
        public async Task GenericReplace()
        {
            var root = await trees[0].GetRootAsync();
            var semanticModel = compilation.GetSemanticModel(trees[0], false);
            var nodes = root.DescendantNodes();

            var declaredTypeSymbols
                = nodes
                .OfType<TypeDeclarationSyntax>()
                .Select(sy => semanticModel.GetDeclaredSymbol(sy))
                .OfType<INamedTypeSymbol>()
                .ToArray();

            var constraintGenericsSymbol = declaredTypeSymbols
                .Single(s => s.ToString() == "TestAssembly.TA.ConstraintGenerics<R, S, Op>");

            var constraint = (INamedTypeSymbol)constraintGenericsSymbol.TypeParameters[2].ConstraintTypes.Single();

            var builder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
            builder.Add(constraintGenericsSymbol.TypeParameters[0],
                declaredTypeSymbols.Single(s => s.ToString() == "TestAssembly.TA.Foo"));
            builder.Add(constraintGenericsSymbol.TypeParameters[1],
                declaredTypeSymbols.Single(s => s.ToString() == "TestAssembly.TA.Bar"));

            var got = SymbolHelpers.ReplaceGenericType(constraint, builder.ToImmutable());

            Assert.Equal(nodes
                .OfType<TypeDeclarationSyntax>()
                .Single(sy => sy.Identifier.Text == "Foo")
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(sy => sy.Declaration.Variables.Single().Identifier.Text == "field2")
                .Select(sy => semanticModel.GetSymbolInfo(sy.Declaration.Type).Symbol)
                .OfType<INamedTypeSymbol>()
                .Single()
                , got, SymbolEqualityComparer.Default);
        }

        [Fact]
        public async Task NestGenericReplace()
        {
            var root = await trees[0].GetRootAsync();
            var semanticModel = compilation.GetSemanticModel(trees[0], false);
            var nodes = root.DescendantNodes();

            var declaredTypeSymbols
                = nodes
                .OfType<TypeDeclarationSyntax>()
                .Select(sy => semanticModel.GetDeclaredSymbol(sy))
                .OfType<INamedTypeSymbol>()
                .ToArray();

            var constraintGenericsSymbol = declaredTypeSymbols
                .Single(s => s.ToString() == "TestAssembly.TA.NestGenerics<R, S, Op>");

            var constraint = (INamedTypeSymbol)constraintGenericsSymbol.TypeParameters[2].ConstraintTypes.Single();

            var builder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
            builder.Add(constraintGenericsSymbol.TypeParameters[0],
                declaredTypeSymbols.Single(s => s.ToString() == "TestAssembly.TA.Foo"));
            builder.Add(constraintGenericsSymbol.TypeParameters[1],
                declaredTypeSymbols.Single(s => s.ToString() == "TestAssembly.TA.Bar"));

            var got = SymbolHelpers.ReplaceGenericType(constraint, builder.ToImmutable());

            Assert.Equal(nodes
                .OfType<TypeDeclarationSyntax>()
                .Single(sy => sy.Identifier.Text == "Foo")
                .DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(sy => sy.Declaration.Variables.Single().Identifier.Text == "field3")
                .Select(sy => semanticModel.GetSymbolInfo(sy.Declaration.Type).Symbol)
                .OfType<INamedTypeSymbol>()
                .Single()
                , got, SymbolEqualityComparer.Default);
        }

        [Fact]
        public async Task ToTypeSyntax()
        {
            var root = await trees[0].GetRootAsync();
            var semanticModel = compilation.GetSemanticModel(trees[0], false);
            var nodes = root.DescendantNodes();
            var declaredTypeSymbols
                = nodes
                .OfType<TypeDeclarationSyntax>()
                .Select(sy => semanticModel.GetDeclaredSymbol(sy))
                .OfType<INamedTypeSymbol>()
                .ToArray();

            var usings = ImmutableHashSet.Create("TestAssembly.TA");
            var ns = SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("TestAssembly"), SyntaxFactory.IdentifierName("TA"));

            var tests = new (TypeSyntax got, TypeSyntax expected)[]
            {
                (declaredTypeSymbols[0].ToTypeSyntax(semanticModel, 0),
                    SyntaxFactory.QualifiedName(ns, (SimpleNameSyntax)SyntaxFactory.ParseName("Foo"))),

                (declaredTypeSymbols[1].ToTypeSyntax(semanticModel, 0),
                    SyntaxFactory.QualifiedName(ns, (SimpleNameSyntax)SyntaxFactory.ParseName("Bar"))),

                (declaredTypeSymbols[4].ToTypeSyntax(semanticModel, 0),
                    SyntaxFactory.QualifiedName(ns,
                        SyntaxFactory.GenericName("NestGenerics").AddTypeArgumentListArguments(
                            SyntaxFactory.ParseName("R"),SyntaxFactory.ParseName("S"),SyntaxFactory.ParseName("Op")))),
            };

            foreach (var (got, expected) in tests)
            {
                Assert.True(got.IsEquivalentTo(expected, true), $"{expected}");
            }
        }
    }
}
