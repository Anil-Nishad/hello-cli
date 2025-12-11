using System.Text.Json;

// Example JSON file path
string jsonFilePath = "data.json";

try
{
    // Check if file exists
    if (!File.Exists(jsonFilePath))
    {
        Console.WriteLine($"File not found: {jsonFilePath}");
        Console.WriteLine("Creating a sample data.json file...");
        CreateSampleJsonFile(jsonFilePath);
    }

    // Read JSON file
    string jsonContent = File.ReadAllText(jsonFilePath);
    
    // Parse JSON
    using JsonDocument doc = JsonDocument.Parse(jsonContent);
    JsonElement root = doc.RootElement;

    Console.WriteLine("JSON Content:");
    Console.WriteLine(root.GetRawText());
    Console.WriteLine("\n--- Parsed Data ---");

    // Example: If JSON is an array
    if (root.ValueKind == JsonValueKind.Array)
    {
        foreach (JsonElement item in root.EnumerateArray())
        {
            Console.WriteLine($"Item: {item.GetRawText()}");
        }
    }
    // Example: If JSON is an object
    else if (root.ValueKind == JsonValueKind.Object)
    {
        foreach (JsonProperty property in root.EnumerateObject())
        {
            Console.WriteLine($"{property.Name}: {property.Value}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// Helper method to create a sample JSON file
static void CreateSampleJsonFile(string filePath)
{
    var sampleData = new
    {
        name = "John Doe",
        age = 30,
        email = "john@example.com",
        hobbies = new[] { "reading", "coding", "gaming" }
    };

    string json = JsonSerializer.Serialize(sampleData, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, json);
    Console.WriteLine($"Sample file created: {filePath}\n");
}
