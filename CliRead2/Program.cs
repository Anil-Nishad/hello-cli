// Program.cs (System.Text.Json version)
using System.Text.Json;

var json = await File.ReadAllTextAsync("data.json");
using var doc = JsonDocument.Parse(json);
var root = doc.RootElement;
Console.WriteLine(root.GetProperty("name").GetString());