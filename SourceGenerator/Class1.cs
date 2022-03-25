using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator
{
    [Generator]
    public class ValueObjectsGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // begin creating the source we'll inject into the users compilation

            //context.AddSource($"logs.g.cs", SourceText.From(string.Join(Environment.NewLine, context.Compilation.SyntaxTrees.Select(s => s.FilePath)), Encoding.UTF8));

            var regex = new Regex(@"<(.*?)>");

            foreach (var tree in context.Compilation.SyntaxTrees)
            {
                var records = tree.GetRoot().DescendantNodes()
                    .Where(x => x is RecordDeclarationSyntax)
                    .Cast<RecordDeclarationSyntax>()
                    .ToList();

                if (!records.Any())
                {
                    continue;
                }

                var usings = tree.GetRoot().DescendantNodes()
                    .Where(x => x is UsingDirectiveSyntax)
                    .Cast<UsingDirectiveSyntax>()
                    .Select(x => x.ToString()).ToList();

                var usingsText = string.Join(" ", usings);

                var valueObject = records
                    .FirstOrDefault(r => r.BaseList != null && r.BaseList.Types.Any(t => t.ToString().Contains("ValueObject")));

                if (valueObject == null)
                {
                    continue;
                }

                var name = valueObject.Identifier.ToString();

                if (name != "Margin")
                {
                    continue;
                }

                var generic = valueObject.BaseList.Types.Select(t => regex.Match(t.ToString()).Groups[1].Value).First();
                var @namespace = (valueObject.Parent as FileScopedNamespaceDeclarationSyntax).Name.ToString();

                var sourceBuilder = new StringBuilder($@"
{usingsText}

namespace {@namespace};

public partial record {name} : ValueObject<{generic}>
{{
    public {generic} Test({generic} value)
    {{
        return value;
    }}
}}
");
                var dbg = sourceBuilder.ToString();
                // inject the created source into the users compilation
                context.AddSource($"{name}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}