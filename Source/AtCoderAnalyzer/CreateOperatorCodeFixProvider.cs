using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtCoderAnalyzer.Diagnostics;
using AtCoderAnalyzer.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static AtCoderAnalyzer.Helpers.Constants;

namespace AtCoderAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CreateOperatorCodeFixProvider)), Shared]
    public class CreateOperatorCodeFixProvider : CodeFixProvider
    {
        private const string title = "Create operator type";
        public override ImmutableArray<string> FixableDiagnosticIds
            => ImmutableArray.Create(
                DiagnosticDescriptors.AC0008_DefineOperatorType.Id);
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

            var isOperatorAttribute = semanticModel.Compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);

            if (semanticModel.GetSymbolInfo(genericNode, context.CancellationToken).Symbol
                is not INamedTypeSymbol symbol)
                return;

            var writtenTypeSyntaxes = genericNode.TypeArgumentList.Arguments;
            var originalTypes = symbol.TypeParameters;
            var writtenTypes = symbol.TypeArguments;

            if (originalTypes.Length != writtenTypes.Length)
                return;

            var defaultSet = ImmutableHashSet.Create<ITypeSymbol>(SymbolEqualityComparer.Default);
            var genericDicBuilder = ImmutableDictionary.CreateBuilder<ITypeParameterSymbol, ITypeSymbol>(SymbolEqualityComparer.Default);
            var constraintDicBuilder = ImmutableDictionary.CreateBuilder<string, ImmutableHashSet<ITypeSymbol>>();
            for (int i = 0; i < originalTypes.Length; i++)
            {
                var writtenTypeSyntax = writtenTypeSyntaxes[i];
                var originalType = originalTypes[i];
                var constraintTypes = originalType.ConstraintTypes;
                var writtenType = writtenTypes[i] as INamedTypeSymbol;

                if (!constraintTypes.SelectMany(ty => ty.GetAttributes())
                    .Select(at => at.AttributeClass)
                    .Contains(isOperatorAttribute, SymbolEqualityComparer.Default))
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
                    = p.Value.Select(sy => SymbolHelpers.ReplaceGenericType(sy, genericDic)).ToImmutableArray();
            }


            var action = CodeAction.Create(title: title,
               createChangedDocument: c => AddOperatorType(
                   context.Document,
                   root,
                   constraintArrayDic.ToImmutable()),
               equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }

        private async Task<Document> AddOperatorType(
            Document document,
            CompilationUnitSyntax root,
            ImmutableDictionary<string, ImmutableArray<ITypeSymbol>> constraintDic)
        {
            bool hasMethod = false;

            var usings = root.Usings.ToNamespaceHashSet();

            MemberDeclarationSyntax[] newMembers = new MemberDeclarationSyntax[constraintDic.Count];
            foreach (var (p, i) in constraintDic.Select((p, i) => (p, i)))
            {
                bool m;
                (newMembers[i], m) = CreateOperatorTypeSyntax(p.Key, p.Value, usings);
                hasMethod |= m;
            }

            root = root.AddMembers(newMembers);
            if (hasMethod && !usings.Contains(System_Runtime_CompilerServices))
                root = SyntaxHelpers.AddSystem_Runtime_CompilerServicesSyntax(root);

            return document.WithSyntaxRoot(root);
        }

        private (StructDeclarationSyntax syntax, bool hasMethod) CreateOperatorTypeSyntax(
            string operatorTypeName,
            ImmutableArray<ITypeSymbol> constraints,
            ImmutableHashSet<string> usings)
        {
            bool hasMethod = false;
            var structDeclarationSyntax = SyntaxFactory.StructDeclaration(operatorTypeName);

            var simplifyTypeSyntax = new SimplifyTypeSyntaxRewriter(usings);

            var baseTypes = ImmutableArray.CreateBuilder<BaseTypeSyntax>(constraints.Length);
            var members = ImmutableList.CreateBuilder<MemberDeclarationSyntax>();

            foreach (var constraint in constraints)
            {
                if (constraint is not INamedTypeSymbol namedType)
                    continue;
                baseTypes.Add(CreateBaseTypeSyntax(namedType));

                foreach (var member in namedType.GetMembers())
                {
                    if (member is IPropertySymbol property)
                    {
                        members.Add(CreatePropertySyntax(property));
                    }
                    else if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    {
                        members.Add(CreateMethodSyntax(method));
                        hasMethod = true;
                    }
                }
            }
            var dec = structDeclarationSyntax.AddBaseListTypes(baseTypes.ToArray()).AddMembers(members.Distinct(MemberDeclarationEqualityComparer.Default).ToArray());
            return ((StructDeclarationSyntax)simplifyTypeSyntax.Visit(dec), hasMethod);
        }

        private BaseTypeSyntax CreateBaseTypeSyntax(INamedTypeSymbol symbol)
            => SyntaxFactory.SimpleBaseType(symbol.ToTypeSyntax());

        private PropertyDeclarationSyntax CreatePropertySyntax(IPropertySymbol symbol)
        {
            var dec = SyntaxFactory.PropertyDeclaration(symbol.Type.ToTypeSyntax(), symbol.Name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));


            if (symbol.SetMethod == null)
                return dec
                    .WithExpressionBody(SyntaxHelpers.ArrowDefault)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
            else if (symbol.GetMethod == null)
                return dec.AddAccessorListAccessors(new AccessorDeclarationSyntax[]
                {
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                });

            return dec.AddAccessorListAccessors(new AccessorDeclarationSyntax[]
            {
                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken)
            });
        }

        private MethodDeclarationSyntax CreateMethodSyntax(IMethodSymbol symbol)
        {
            var dec = SyntaxFactory.MethodDeclaration(symbol.ReturnType.ToTypeSyntax(), symbol.Name)
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)
                    .WithParameterList(symbol.ToParameterListSyntax())
                    .WithExpressionBody(SyntaxHelpers.ArrowDefault)
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken);
            return dec;
        }
        public int Prop { get; set; }

        #region Old
#pragma warning disable IDE0060
        private SyntaxNode AC0005_SegtreeOperator(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[1].ToString();
            bool hasUsing = false;
            bool hasSystem_Runtime_CompilerServices = false;

            foreach (var sy in root.Usings)
            {
                var name = sy.Name.ToString();
                if (name == "AtCoder")
                    hasUsing = true;
                if (name == System_Runtime_CompilerServices)
                    hasSystem_Runtime_CompilerServices = true;
            }
            if (!hasSystem_Runtime_CompilerServices)
                root = SyntaxHelpers.AddSystem_Runtime_CompilerServicesSyntax(root);

            var segType = genericArgs[0];
            var baseType = ParseAtCoder(hasUsing, "ISegtreeOperator", segType);
            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(baseType)
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(segType, "Identity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                    SyntaxFactory.MethodDeclaration(segType, "Operate")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("y")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        )
                );
            return root.AddMembers(newMember);
        }
        private async Task<Document> CreateOperatorTypeOld(
            Document document,
            CompilationUnitSyntax root,
            string diagnosticId,
            SeparatedSyntaxList<TypeSyntax> genericArgs,
            CancellationToken cancellationToken)
            => document.WithSyntaxRoot(diagnosticId switch
            {
                "AC0003" => AC0003_StaticModInt(root, genericArgs),
                "AC0004" => AC0004_DynamicModInt(root, genericArgs),
                "AC0005" => AC0005_SegtreeOperator(root, genericArgs),
                "AC0006" => AC0006_LazySegtreeOperator(root, genericArgs),
                _ => root,
            });
        private static SimpleBaseTypeSyntax ParseAtCoder(bool hasUsing, string text, params TypeSyntax[] genericTypes)
        {
            SimpleNameSyntax type;
            if (genericTypes.Length == 0)
            {
                type = SyntaxFactory.ParseName(text) as SimpleNameSyntax;
            }
            else
            {
                type = SyntaxFactory.GenericName(text).AddTypeArgumentListArguments(genericTypes);
            }

            if (!hasUsing)
            {
                return SyntaxFactory.SimpleBaseType(
                    SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName("AtCoder"), type));
            }
            return SyntaxFactory.SimpleBaseType(type);
        }
        private SyntaxNode AC0003_StaticModInt(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[0].ToString();
            var hasUsing = root.Usings.Select(sy => sy.Name.ToString()).Any(n => n == "AtCoder");
            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(
                    ParseAtCoder(hasUsing, "IStaticMod")
                    )
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.UIntKeyword)),
                        "Mod"
                    ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(1000000007)))
                        )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                    SyntaxFactory.PropertyDeclaration(
                        SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                        "IsPrime"
                    ).AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken)
                );
            return root.AddMembers(newMember);
        }
        private SyntaxNode AC0004_DynamicModInt(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[0].ToString();
            var hasUsing = root.Usings.Select(sy => sy.Name.ToString()).Any(n => n == "AtCoder");
            return root.AddMembers(
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(
                    ParseAtCoder(hasUsing, "IDynamicModID")
                    ));
        }

        private SyntaxNode AC0006_LazySegtreeOperator(CompilationUnitSyntax root, SeparatedSyntaxList<TypeSyntax> genericArgs)
        {
            var operatorTypeName = genericArgs[2].ToString();
            bool hasUsing = false;
            bool hasSystem_Runtime_CompilerServices = false;

            foreach (var sy in root.Usings)
            {
                var name = sy.Name.ToString();
                if (name == "AtCoder")
                    hasUsing = true;
                if (name == System_Runtime_CompilerServices)
                    hasSystem_Runtime_CompilerServices = true;
            }
            if (!hasSystem_Runtime_CompilerServices)
                root = SyntaxHelpers.AddSystem_Runtime_CompilerServicesSyntax(root);

            var segType = genericArgs[0];
            var funType = genericArgs[1];
            var baseType = ParseAtCoder(hasUsing, "ILazySegtreeOperator", segType, funType);

            var newMember =
                SyntaxFactory.StructDeclaration(operatorTypeName)
                .AddBaseListTypes(baseType)
                .AddMembers(
                    SyntaxFactory.PropertyDeclaration(segType, "Identity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),
                    SyntaxFactory.PropertyDeclaration(funType, "FIdentity")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithExpressionBody(
                        SyntaxFactory.ArrowExpressionClause(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                        )
                    .WithSemicolonToken(SyntaxHelpers.SemicolonToken),

                    SyntaxFactory.MethodDeclaration(funType, "Composition")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("f")).WithType(funType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("g")).WithType(funType),
                            })
                            ))
                    .AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        ),

                    SyntaxFactory.MethodDeclaration(segType, "Mapping")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("f")).WithType(funType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        ),

                    SyntaxFactory.MethodDeclaration(segType, "Operate")
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .WithParameterList(
                        SyntaxFactory.ParameterList(
                            SyntaxFactory.SeparatedList(new[] {
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")).WithType(segType),
                                SyntaxFactory.Parameter(SyntaxFactory.Identifier("y")).WithType(segType),
                            })
                            ))
                    .AddAttributeLists(SyntaxHelpers.AggressiveInliningAttributeList)
                    .WithBody(SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.DefaultLiteralExpression))
                            )
                        )
                );
            return root.AddMembers(newMember);
        }
#pragma warning restore IDE0060
        #endregion Old
    }
}
