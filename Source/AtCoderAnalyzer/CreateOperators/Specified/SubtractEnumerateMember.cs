﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AtCoderAnalyzer.CreateOperators.Specified
{
    internal class SubtractEnumerateMember : OperatorEnumerateMember
    {
        internal SubtractEnumerateMember(SemanticModel semanticModel, ITypeSymbol typeSymbol) : base(semanticModel, typeSymbol) { }

        protected override SyntaxKind? GetSyntaxKind(IMethodSymbol symbol)
            => symbol switch
            {
                { Parameters: { Length: not 2 } } => null,
                { Name: "Subtract" } => SyntaxKind.SubtractExpression,
                _ => null,
            };
    }
}
