using Microsoft.CodeAnalysis;

namespace AtCoderAnalyzer.Diagnostics
{
    public static class DiagnosticDescriptors
    {
        internal static readonly DiagnosticDescriptor AC0001_MultiplyOverflowInt32 = new DiagnosticDescriptor(
            "AC0001",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_AC0002_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Overflow",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
            );
        internal static readonly DiagnosticDescriptor AC0002_LeftShiftOverflowInt32 = new DiagnosticDescriptor(
            "AC0002",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0002_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_AC0002_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Overflow",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
            );


        internal static readonly DiagnosticDescriptor AC0003_StaticModIntOperator = new DiagnosticDescriptor(
            "AC0002",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0002_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_AC0002_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Overflow",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true
            );
    }
}
