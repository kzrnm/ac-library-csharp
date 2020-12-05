using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AtCoderAnalyzer.Helpers
{
    internal class SyntaxHelpers
    {
        public static CompilationUnitSyntax AddSystem_Runtime_CompilerServicesSyntax(CompilationUnitSyntax root)
        {
            return root.AddUsings(
                SyntaxFactory.UsingDirective(
                    SyntaxFactory.QualifiedName(
                        SyntaxFactory.QualifiedName(
                            SyntaxFactory.IdentifierName("System"),
                            SyntaxFactory.IdentifierName("Runtime")
                        ),
                        SyntaxFactory.IdentifierName("CompilerServices")
                    )));
        }

        public static readonly AttributeSyntax AggressiveInliningAttribute
            = SyntaxFactory.Attribute(SyntaxFactory.ParseName("MethodImpl"))
                    .AddArgumentListArguments(
                        SyntaxFactory.AttributeArgument(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("MethodImplOptions"),
                                SyntaxFactory.IdentifierName("AggressiveInlining")
                        )));

        public static readonly AttributeListSyntax AggressiveInliningAttributeList
            = SyntaxFactory.AttributeList(
                SyntaxFactory.SeparatedList(new[] { AggressiveInliningAttribute }));
    }
}
