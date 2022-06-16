using System.Collections.Generic;
using System.Linq;
using CSharpToTypeScript.Core.Options;
using CSharpToTypeScript.Core.Transformations;
using CSharpToTypeScript.Core.Utilities;
using static CSharpToTypeScript.Core.Utilities.StringUtilities;

namespace CSharpToTypeScript.Core.Models
{
    internal class FileNode
    {
        public FileNode(IEnumerable<RootNode> rootNodes)
        {
            RootNodes = rootNodes;
        }

        public IEnumerable<RootNode> RootNodes { get; }

        public IEnumerable<string> Requires => RootNodes.SelectMany(r => r.Requires).Distinct();

        public IEnumerable<string> Imports => Requires.Except(RootNodes.Select(r => r.Name));

        public string WriteTypeScript(CodeConversionOptions options)
        {
            var context = new Context();

            return // imports
                (
                    GetImports(options).LineByLine() + EmptyLine
                ).If(Imports.Any() && options.ImportGenerationMode != ImportGenerationMode.None)
                // types
                + RootNodes.WriteTypeScript(options, context).ToEmptyLineSeparatedList()
                // empty line at the end
                + NewLine.If(options.AppendNewLine);
        }

        private IEnumerable<string> GetImports(CodeConversionOptions options)
        {
            return Imports.Select(i => GenerateImport(i, options)).Distinct();
        }

        private string GenerateImport(string name, CodeConversionOptions options)
        {
            var importName = name.TransformIf(options.RemoveInterfacePrefix, StringUtilities.RemoveInterfacePrefix);
            var importPath = ("./" + ModuleNameTransformation.Transform(name, options));
            if (options.GlobalImport.ContainsKey(importName))
            {
                importPath = options.GlobalImport[importName];
            }
            // type
            return "import { " + importName + " }"
            // module
            + " from " + importPath.InQuotes(options.QuotationMark) + ";";
        }
    }
}
