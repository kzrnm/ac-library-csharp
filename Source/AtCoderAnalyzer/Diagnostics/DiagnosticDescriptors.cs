using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace AtCoderAnalyzer.Diagnostics
{
    public static class DiagnosticDescriptors
    {
#pragma warning disable IDE0090 // Avoid 'new(...)' for Shipped.md
        internal static Diagnostic AC0001_MultiplyOverflowInt32(SyntaxNode node)
            => Diagnostic.Create(AC0001_MultiplyOverflowInt32_Descriptor, node.GetLocation(), node.ToString());
        internal static readonly DiagnosticDescriptor AC0001_MultiplyOverflowInt32_Descriptor = new DiagnosticDescriptor(
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
        internal static Diagnostic AC0002_LeftShiftOverflowInt32(SyntaxNode node)
            => Diagnostic.Create(AC0002_LeftShiftOverflowInt32_Descriptor, node.GetLocation(), node.ToString());
        internal static readonly DiagnosticDescriptor AC0002_LeftShiftOverflowInt32_Descriptor = new DiagnosticDescriptor(
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

        internal static Diagnostic AC0007_AgressiveInlining(Location location, IEnumerable<string> methods)
            => Diagnostic.Create(AC0007_AgressiveInlining_Descriptor, location, string.Join(", ", methods));

        internal static readonly DiagnosticDescriptor AC0007_AgressiveInlining_Descriptor = new DiagnosticDescriptor(
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

        internal static Diagnostic AC0008_DefineOperatorType(Location location, IEnumerable<string> types)
            => Diagnostic.Create(AC0008_DefineOperatorType_Descriptor, location, string.Join(", ", types));
        internal static readonly DiagnosticDescriptor AC0008_DefineOperatorType_Descriptor = new DiagnosticDescriptor(
            "AC0008",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0008_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0008_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Type Define",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true
            );
#pragma warning restore IDE0090
    }
}
