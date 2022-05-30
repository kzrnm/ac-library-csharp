using System;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AtCoderAnalyzer
{
    public record AtCoderAnalyzerConfig(bool UseMethodImplNumeric)
    {
        public static AtCoderAnalyzerConfig Parse(AnalyzerConfigOptions analyzerConfigOptions)
        {
            var useMethodImplNumeric = analyzerConfigOptions.TryGetValue("build_property.AtCoderAnalyzer_UseMethodImplNumeric", out var v) &&
                StringComparer.OrdinalIgnoreCase.Equals(v, "true");

            return new(useMethodImplNumeric);
        }
    }
}
