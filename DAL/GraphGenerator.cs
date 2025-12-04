using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Linq;

public class MermaidErGenerator
{
    public static string Generate(DbContext db)
    {
        var model = db.Model;
        var sb = new StringBuilder();
        sb.AppendLine("erDiagram");

        // Entities + Fields
        foreach (var entity in model.GetEntityTypes())
        {
            var entName = entity.ClrType.Name.ToUpperInvariant();

            var props = entity.GetProperties()
                .OrderBy(p => p.Name)
                .Select(p =>
                {
                    var typeName = MapTypeName(p.ClrType);
                    var name = p.Name;
                    var pk = name == "Id" ? " PK" : string.Empty;
                    var nullable = p.IsNullable ? " \"nullable\"" : string.Empty;
                    return $"{typeName} {name}{pk}{nullable}";
                })
                .ToList();

            // compact single-line for short entities (to match sample layout), otherwise multi-line
            if (props.Count <= 4)
            {
                sb.AppendLine($"    {entName} {{ {string.Join(' ', props)} }}");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"    {entName} {{");
                foreach (var line in props)
                {
                    sb.AppendLine($"        {line}");
                }
                sb.AppendLine("    }");
                sb.AppendLine();
            }
        }

        // Relationships
        foreach (var entity in model.GetEntityTypes())
        {
            foreach (var fk in entity.GetForeignKeys())
            {
                var child = entity.ClrType.Name.ToUpperInvariant();
                var parent = fk.PrincipalEntityType.ClrType.Name.ToUpperInvariant();

                // Use FK property name if available, otherwise navigation name
                var fkPropName = fk.Properties.FirstOrDefault()?.Name ?? fk.DependentToPrincipal?.Name ?? string.Empty;

                // Use unique constraint to indicate one-to-one, otherwise many relationship
                var rel = fk.IsUnique ? "||--||" : "||--o{";

                sb.AppendLine($"    {parent} {rel} {child} : \"{fkPropName}\"");
            }
        }

        return sb.ToString();
    }

    private static string MapTypeName(System.Type clrType)
    {
        var type = clrType;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = Nullable.GetUnderlyingType(type)!;
        }

        if (type == typeof(int)) return "int";
        if (type == typeof(string)) return "string";
        if (type == typeof(decimal)) return "decimal";
        if (type == typeof(bool)) return "bool";
        if (type == typeof(byte)) return "byte";
        if (type == typeof(DateTime)) return "DateTime";
        // fallback to CLR name
        return type.Name;
    }
}
