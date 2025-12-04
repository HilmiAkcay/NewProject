using System.CommandLine;
using System.CommandLine.Invocation;
// Add this using directive for System.CommandLine.NamingConventionBinder if needed
// using System.CommandLine.NamingConventionBinder;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// ...existing usings...

var rootCommand = new RootCommand("Generate Mermaid class diagrams from C# source files")
{
    new Option<string?>(new[] {"--src", "-s"}, () => null, "Source folder (defaults to current directory)"),
    new Option<string?>(new[] {"--out", "-o"}, () => "classes.md", "Output Markdown file path"),
};

rootCommand.SetHandler(async (InvocationContext context) =>
{
    var src = context.ParseResult.GetValueForOption<string?>(rootCommand.Options[0]);
    var outFile = context.ParseResult.GetValueForOption<string?>(rootCommand.Options[1]);
    var srcDir = string.IsNullOrEmpty(src) ? Directory.GetCurrentDirectory() : Path.GetFullPath(src);
    var outPath = Path.GetFullPath(outFile ?? "classes.md");

    var csFiles = Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories);

    var sb = new StringBuilder();
    sb.AppendLine("# Mermaid class diagram generated");
    sb.AppendLine();
    sb.AppendLine("```mermaid");
    sb.AppendLine("classDiagram");

    var syntaxTrees = new List<SyntaxTree>();
    foreach (var file in csFiles)
    {
        var text = await File.ReadAllTextAsync(file);
        var tree = CSharpSyntaxTree.ParseText(text);
        syntaxTrees.Add(tree);
    }

    var compilation = CSharpCompilation.Create("_gen", syntaxTrees, new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) });

    var classes = new Dictionary<string, ClassDeclarationSyntax>();

    foreach (var tree in syntaxTrees)
    {
        var root = await tree.GetRootAsync();
        var classNodes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
        foreach (var cls in classNodes)
        {
            var fullName = GetFullName(cls);
            if (!classes.ContainsKey(fullName)) classes[fullName] = cls;
        }
    }

    foreach (var kv in classes)
    {
        var name = kv.Key.Replace("<", "_").Replace(">", "_");
        sb.AppendLine($"    class {name} {{");
        foreach (var member in kv.Value.Members)
        {
            if (member is PropertyDeclarationSyntax prop)
            {
                var type = prop.Type.ToString();
                var propName = prop.Identifier.Text;
                sb.AppendLine($"        +{type} {propName}");
            }
            else if (member is FieldDeclarationSyntax field)
            {
                var type = field.Declaration.Type.ToString();
                foreach (var v in field.Declaration.Variables)
                {
                    sb.AppendLine($"        -{type} {v.Identifier.Text}");
                }
            }
            else if (member is MethodDeclarationSyntax method)
            {
                var ret = method.ReturnType.ToString();
                var mName = method.Identifier.Text;
                sb.AppendLine($"        +{ret} {mName}()");
            }
        }
        sb.AppendLine("    }");
    }

    foreach (var kv in classes)
    {
        var cls = kv.Value;
        var from = GetFullName(cls).Replace("<", "_").Replace(">", "_");
        if (cls.BaseList != null)
        {
            foreach (var baseType in cls.BaseList.Types)
            {
                var to = baseType.Type.ToString().Replace("<", "_").Replace(">", "_");
                sb.AppendLine($"    {to} <|-- {from}");
            }
        }

        foreach (var member in cls.Members.OfType<PropertyDeclarationSyntax>())
        {
            var type = member.Type.ToString().Replace("<", "_").Replace(">", "_");
            if (classes.ContainsKey(type))
            {
                sb.AppendLine($"    {from} --> {type} : has");
            }
        }
    }

    sb.AppendLine("```");

    await File.WriteAllTextAsync(outPath, sb.ToString());

    Console.WriteLine($"Wrote {outPath} ({csFiles.Length} .cs files scanned)");
},
rootCommand.Options.ToArray());

await rootCommand.InvokeAsync(args);

static string GetFullName(ClassDeclarationSyntax cls)
{
    var names = new Stack<string>();
    names.Push(cls.Identifier.Text);
    var parent = cls.Parent;
    while (parent != null)
    {
        if (parent is NamespaceDeclarationSyntax ns)
        {
            names.Push(ns.Name.ToString());
            break;
        }
        else if (parent is ClassDeclarationSyntax pc)
        {
            names.Push(pc.Identifier.Text);
        }
        parent = parent.Parent;
    }
    return string.Join('.', names);
}
