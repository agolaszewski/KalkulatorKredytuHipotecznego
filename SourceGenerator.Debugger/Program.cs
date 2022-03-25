using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGenerator;
using System.Reflection;

Compilation inputCompilation = CreateCompilation(@"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
");


ValueObjectsGenerator generator = new ValueObjectsGenerator();

// Create the driver that will control the generation, passing in our generator
GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

// Run the generation pass
driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

static Compilation CreateCompilation(string source)
{
    var testClass = @"

    using System;
    using TestNamespace;
    namespace KalkulatorKredytuHipotecznego.Domain;

    public partial record TestObj : ValueObject<TestStruct>
    {
    }";

    return CSharpCompilation.Create("compilation",
        new[] {CSharpSyntaxTree.ParseText(source), CSharpSyntaxTree.ParseText(testClass) },
        new[]
        {
            MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(KalkulatorKredytuHipotecznego.Domain.Margin).GetTypeInfo().Assembly
                .Location)
        },
        new CSharpCompilationOptions(OutputKind.ConsoleApplication));
}