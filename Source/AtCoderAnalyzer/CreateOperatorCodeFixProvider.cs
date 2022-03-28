using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using AtCoderAnalyzer.CreateOperators;
using AtCoderAnalyzer.Diagnostics;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CreateOperatorCodeFixProvider)), Shared]
    public class CreateOperatorCodeFixProvider : CodeFixProvider
    {
        private const string title = "Create operator type";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0008_DefineOperatorType_Descriptor.Id);
        public sealed override FixAllProvider GetFixAllProvider()
            => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            if (await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false)
                is not CompilationUnitSyntax root)
                return;
            var diagnostic = context.Diagnostics[0];
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            if (root.FindNode(diagnosticSpan)
                is not GenericNameSyntax genericNode)
                return;

            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

            ImmutableArray<ITypeParameterSymbol> originalTypes;
            ImmutableArray<ITypeSymbol> writtenTypes;
            switch (semanticModel.GetSymbolInfo(genericNode, context.CancellationToken).Symbol)
            {
                case INamedTypeSymbol symbol:
                    originalTypes = symbol.TypeParameters;
                    writtenTypes = symbol.TypeArguments;
                    break;
                case IMethodSymbol symbol:
                    originalTypes = symbol.TypeParameters;
                    writtenTypes = symbol.TypeArguments;
                    break;
                default:
                    return;
            }


            var writtenTypeSyntaxes = genericNode.TypeArgumentList.Arguments;

            if (originalTypes.Length != writtenTypes.Length)
                return;

            if (!OperatorTypesMatcher.TryParseTypes(semanticModel.Compilation, out var types))
                return;

            var defaultSet = ImmutableHashSet.Create<ITypeSymbol>(SymbolEqualityComparer.Default);
            var genericDicBuilder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
            var constraintDicBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableHashSet<ITypeSymbol>>();
            for (int i = 0; i < originalTypes.Length; i++)
            {
                var writtenTypeSyntax = writtenTypeSyntaxes[i];
                var originalType = originalTypes[i];
                var constraintTypes = originalType.ConstraintTypes;
                var writtenType = writtenTypes[i];

                if (!constraintTypes
                    .OfType<INamedTypeSymbol>()
                    .Select(ty => ty.ConstructedFrom)
                    .Any(ty => types.IsMatch(ty)))
                {
                    genericDicBuilder.Add(originalType, writtenType);
                    continue;
                }

                if (writtenType.TypeKind == TypeKind.Error)
                {
                    var name = writtenType.Name;
                    var typeSymbols = constraintDicBuilder.GetValueOrDefault(name, defaultSet);
                    constraintDicBuilder[name] = typeSymbols.Union(constraintTypes);
                }
            }
            if (constraintDicBuilder.Count == 0)
                return;

            var genericDic = genericDicBuilder.ToImmutable();
            var constraintArrayDic = ImmutableDictionary.CreateBuilder<string, ImmutableArray<ITypeSymbol>>();
            foreach (var p in constraintDicBuilder)
            {
                constraintArrayDic[p.Key]
                    = p.Value.Select(sy => SymbolHelpers.ReplaceGenericType(sy, genericDic))
                    .OrderBy(sy => sy.ToDisplayString())
                    .ToImmutableArray();
            }

            var action = CodeAction.Create(title: title,
               createChangedDocument: c => AddOperatorType(
                   context.Document,
                   root,
                   semanticModel,
                   constraintArrayDic.ToImmutable()),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }

        private async Task<Document> AddOperatorType(
            Document document,
            CompilationUnitSyntax root,
            SemanticModel semanticModel,
            ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraintDic)
        {
            bool hasMethod = false;
            var usings = root.Usings.ToNamespaceHashSet();
            var builder = new OperatorTypeSyntaxBuilder(semanticModel);

            MemberDeclarationSyntax[] newMembers = new MemberDeclarationSyntax[constraintDic.Count];
            foreach (var (p, i) in constraintDic.Select((p, i) => (p, i)))
            {
                bool m;
                (newMembers[i], m) = builder.Build(p.Key, p.Value);
                hasMethod |= m;
            }

            root = root.AddMembers(newMembers);
            return document.WithSyntaxRoot(root);
        }

        private class OperatorTypeSyntaxBuilder
        {
            private readonly SemanticModel semanticModel;
            private readonly int origPosition;
            public OperatorTypeSyntaxBuilder(SemanticModel semanticModel)
            {
                this.semanticModel = semanticModel;
                origPosition = semanticModel.SyntaxTree.Length;
            }

            public (StructDeclarationSyntax syntax, bool hasMethod) Build(string operatorTypeName, ImmutableArray<ITypeSymbol> constraints)
            {
                bool hasMethod = false;
                var members = ImmutableList.CreateBuilder<MemberDeclarationSyntax>();
                var added = ImmutableHashSet.CreateBuilder<ITypeSymbol>(SymbolEqualityComparer.Default);

                foreach (var constraint in constraints)
                {
                    foreach (var baseType in constraint.AllInterfaces.Append(constraint))
                    {
                        if (!added.Add(baseType))
                            continue;

                        foreach (var (member, isMethod) in EnumerateMember.Create(semanticModel, baseType).EnumerateMemberSyntax())
                        {
                            members.Add(member);
                            hasMethod |= isMethod;
                        }
                    }
                }

                var dec = SyntaxFactory.StructDeclaration(operatorTypeName)
                    .WithBaseList(SyntaxFactory.BaseList(
                        constraints.Select(c => (BaseTypeSyntax)SyntaxFactory.SimpleBaseType(c.ToTypeSyntax(semanticModel, origPosition))).ToSeparatedSyntaxList()))
                    .WithMembers(SyntaxFactory.List(members.Distinct(MemberDeclarationEqualityComparer.Default)));
                return (dec, hasMethod);
            }
        }
    }
}
