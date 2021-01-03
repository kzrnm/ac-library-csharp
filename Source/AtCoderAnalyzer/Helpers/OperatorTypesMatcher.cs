using Microsoft.CodeAnalysis;
using static AtCoderAnalyzer.Helpers.Constants;

namespace AtCoderAnalyzer.Helpers
{
    public class OperatorTypesMatcher
    {
        public INamedTypeSymbol IsOperatorAttribute { get; }
        public INamedTypeSymbol IComparer { get; }

        public OperatorTypesMatcher(INamedTypeSymbol isOperator, INamedTypeSymbol iComparer)
        {
            IsOperatorAttribute = isOperator;
            IComparer = iComparer;
        }

        public bool IsMatch(ITypeSymbol typeSymbol)
        {
            if (SymbolEqualityComparer.Default.Equals(typeSymbol, IComparer))
                return true;

            foreach (var at in typeSymbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(at.AttributeClass, IsOperatorAttribute))
                    return true;
            }
            return false;
        }
        public static bool TryParseTypes(Compilation compilation, out OperatorTypesMatcher types)
        {
            types = null;
            var isOperatorAttr = compilation.GetTypeByMetadataName(AtCoder_IsOperatorAttribute);
            if (isOperatorAttr is null)
                return false;
            var iComparer = compilation.GetTypeByMetadataName(System_Collections_Generic_IComparer);
            if (iComparer is null)
                return false;
            types = new OperatorTypesMatcher(isOperatorAttr, iComparer);
            return true;
        }
    }
}
