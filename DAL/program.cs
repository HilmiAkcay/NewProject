using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

class Program
{
    static void Main()
    {
        var projectRoot = FindProjectRoot();
        var entitiesFolder = Path.Combine(projectRoot, "Entities");

        if (!Directory.Exists(entitiesFolder))
        {
            Console.WriteLine($"Entities klasörü bulunamadı: {entitiesFolder}");
            return;
        }

        Console.WriteLine($"Entities klasörü bulunuydu: {entitiesFolder}");

        var csFiles = Directory.GetFiles(entitiesFolder, "*.cs", SearchOption.AllDirectories);
        if (!csFiles.Any())
        {
            Console.WriteLine("Entities klasöründe .cs dosyası yok.");
            return;
        }

        // 1) Compile POCO classes dynamically
        var assembly = CompileEntities(csFiles);
        var allTypes = assembly.GetTypes()
            .Where(t => t.IsClass && t.IsPublic && !t.IsAbstract)
            .ToList();

        var output = Path.Combine(projectRoot, "mermaid-er.mmd");

        using var sw = new StreamWriter(output, false);
        sw.WriteLine("erDiagram\n");

        // --- ENTITIES ---
        foreach (var type in allTypes.OrderBy(t => t.Name))
        {
            sw.WriteLine($"    {type.Name} {{");

            foreach (var prop in type.GetProperties())
            {
                sw.WriteLine($"        {GetFriendlyType(prop.PropertyType)} {prop.Name}");
            }

            sw.WriteLine("    }\n");
        }

        // --- RELATIONSHIPS ---
        foreach (var type in allTypes)
        {
            foreach (var prop in type.GetProperties())
            {
                // One-to-one
                if (allTypes.Contains(prop.PropertyType))
                {
                    sw.WriteLine($"    {type.Name} ||--|| {prop.PropertyType.Name} : \"{prop.Name}\"");
                }

                // One-to-many (ICollection<T>, List<T>, etc)
                if (IsCollection(prop.PropertyType, out var elementType)
                    && allTypes.Contains(elementType))
                {
                    sw.WriteLine($"    {type.Name} ||--o{{ {elementType.Name} : \"{prop.Name}\"");
                }
            }
        }

        Console.WriteLine($"\nMermaid ER oluşturuldu: {output}");
    }

    // Finds the actual project directory (tracing up from bin/Debug/netX)
    static string FindProjectRoot()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null && !Directory.GetFiles(dir, "*.csproj").Any())
        {
            dir = Directory.GetParent(dir)?.FullName;
        }
        return dir!;
    }

    static Assembly CompileEntities(string[] csFiles)
    {
        var syntaxTrees = csFiles
            .Select(f => CSharpSyntaxTree.ParseText(File.ReadAllText(f)))
            .ToList();

        var refs = AppDomain.CurrentDomain.GetAssemblies()
     .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
     .Select(a => MetadataReference.CreateFromFile(a.Location))
     .ToList();

        // Kesinlikle System.Collections.dll ve netstandard.dll ekle
        var systemCollections = typeof(List<>).Assembly.Location;
        var systemRuntime = typeof(object).Assembly.Location;

        refs.Add(MetadataReference.CreateFromFile(systemCollections));
        refs.Add(MetadataReference.CreateFromFile(systemRuntime));

        var compilation = CSharpCompilation.Create(
            "EntitiesDynamicAssembly",
            syntaxTrees,
            refs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            Console.WriteLine("Derleme hataları:");
            foreach (var d in result.Diagnostics)
                Console.WriteLine(d);
            throw new Exception("Entity compilation failed.");
        }

        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }

    static bool IsCollection(Type t, out Type element)
    {
        element = null;

        if (t.IsGenericType &&
            t.GetInterfaces().Any(i => i.Name.StartsWith("ICollection") || i.Name.StartsWith("IEnumerable")))
        {
            element = t.GetGenericArguments()[0];
            return true;
        }

        return false;
    }

    static string GetFriendlyType(Type t)
    {
        if (t == typeof(int)) return "int";
        if (t == typeof(long)) return "long";
        if (t == typeof(short)) return "short";
        if (t == typeof(string)) return "string";
        if (t == typeof(decimal)) return "decimal";
        if (t == typeof(bool)) return "bool";
        if (t == typeof(DateTime)) return "DateTime";

        if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            return GetFriendlyType(t.GetGenericArguments()[0]);

        return t.Name;
    }
}
