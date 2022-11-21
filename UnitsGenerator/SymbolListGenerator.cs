using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using System.Diagnostics;

namespace UnitsGenerator
{
    [Generator]
    class SymbolListGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            string attribute = "PhysicalUnits.SymbolAttribute";
            Debug.WriteLine("Execute code generator");
            Dictionary<string, string> dict = SetAttributesDictionary(context, attribute);
            StringBuilder sb = MakeSource(dict);
            context.AddSource($"Units.g.cs", SourceText.From(sb.ToString(), Encoding.BigEndianUnicode));
        }

        private StringBuilder MakeSource(Dictionary<string, string> dict)
        {
            var sb = new StringBuilder();
            sb.Append(@"
using System;
using System.Collections.Generic;

namespace PhysicalUnits
{
    public static partial class Units");

            sb.Append(@"
    {
        private static partial void FillDictionary()
        {
");
            foreach (var key in dict.Keys)
            {
                sb.AppendLine(
$"           Units.BySymbol.Add(\"{key}\",{dict[key]});");
            }
            sb.Append(
@"      }
    }
}");
            return sb;
        }

        private static Dictionary<string, string> SetAttributesDictionary(GeneratorExecutionContext context, string attribute)
        {
            INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName(attribute);
            List<ISymbol> PropertySymbols = context.Compilation.GetTypeByMetadataName("PhysicalUnits.Units")
                .GetMembers().
                Where(m =>
                m.Kind == SymbolKind.Property &&
                m.GetAttributes().Any(a => a.AttributeClass.MetadataName == attributeSymbol.MetadataName)).ToList();
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var property in PropertySymbols)
            {
                var symbol = property.GetAttributes().First(a => a.AttributeClass.MetadataName == attributeSymbol.MetadataName);
                var arg = symbol.ConstructorArguments.First().Value.ToString();
                Debug.WriteLine($"{arg},{property.Name}");
                keyValuePairs.Add(arg, "Units." + property.Name);
            }
            return keyValuePairs;
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

    }
}
