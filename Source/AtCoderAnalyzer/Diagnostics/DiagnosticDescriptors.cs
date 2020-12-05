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


        internal static readonly DiagnosticDescriptor AC0003_StaticModInt = new DiagnosticDescriptor(
            "AC0003",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0003_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0003_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
            );

        internal static readonly DiagnosticDescriptor AC0004_DynamicModInt = new DiagnosticDescriptor(
            "AC0004",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0004_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0004_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
            );

        internal static readonly DiagnosticDescriptor AC0005_SegtreeOperator = new DiagnosticDescriptor(
            "AC0005",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0005_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0005_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
            );

        internal static readonly DiagnosticDescriptor AC0006_LazySegtreeOperator = new DiagnosticDescriptor(
            "AC0006",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0006_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0006_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
            );

        internal static readonly DiagnosticDescriptor AC0007_AgressiveInlining = new DiagnosticDescriptor(
            "AC0007",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0007_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0007_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Info,
            isEnabledByDefault: true
            );
    }
}
