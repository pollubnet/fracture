using System.Text.Json;
using Fracture.Server.Modules.NoiseGenerator.Models;

namespace Fracture.Server.Modules.NoiseGenerator.Services;

using System;
using System.IO;
using System.Text.Json;
using Fracture.Server.Modules.NoiseGenerator.Models;

public class NoiseParametersReader
{
    public static NoiseParameters? ReadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file '{filePath}' was not found.");
        }

        try
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<NoiseParameters>(jsonString);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize the JSON content.", ex);
        }
    }

    public static NoiseParameters? ReadFromString(string jsonString)
    {
        try
        {
            return JsonSerializer.Deserialize<NoiseParameters>(jsonString);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to deserialize the JSON content.", ex);
        }
    }

    public static void WriteToFile(NoiseParameters parameters, string filePath)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(
                parameters,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(filePath, jsonString);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to serialize and write to the file.", ex);
        }
    }
}
