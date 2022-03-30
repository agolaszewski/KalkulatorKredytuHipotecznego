using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SourceGenerator
{
    [Generator]
    public class ValueObjectsGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
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

                    var usingText = string.Join(" ", usings);

                    var valueObject = records
                        .FirstOrDefault(r => r.BaseList != null && r.BaseList.Types.Any(t => t.ToString().Contains("ValueObject")));

                    if (valueObject == null)
                    {
                        continue;
                    }

                    var name = valueObject.Identifier.ToString();

                    var generic = valueObject.BaseList.Types.Select(t => regex.Match(t.ToString()).Groups[1].Value).FirstOrDefault();
                    var @namespace = (valueObject.Parent as FileScopedNamespaceDeclarationSyntax).Name.ToString();

                    var sourceBuilder = new StringBuilder($@"
{usingText}

namespace {@namespace};

public partial record {name} : ValueObject<{generic}>
{{
    public {name}({generic} value)
    {{
        Value = value;
    }}

    public static implicit operator {name}({generic} value)
    {{
        return new {name}(value);
    }}

    public static explicit operator {generic}({name} value)
    {{
        return value.Value;
    }}
}}
");
                    var dbg = sourceBuilder.ToString();
                    // inject the created source into the users compilation
                    context.AddSource($"{name}.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.StackTrace);
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}