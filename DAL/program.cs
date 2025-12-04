using DAL;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("TempDb")
    .Options;

using var db = new AppDbContext(options);
// Ensure model is created so EF Core builds the model metadata
db.Database.EnsureCreated();

var diagram = MermaidErGenerator.Generate(db);

var outPath = Path.Combine(Directory.GetCurrentDirectory(), "erDiagram.md");
File.WriteAllText(outPath, diagram);
Console.WriteLine($"Wrote {outPath}");