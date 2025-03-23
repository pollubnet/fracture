using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fracture.Server.Modules.MapGenerator;

public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var colorName = reader.GetString();

        if (string.IsNullOrWhiteSpace(colorName))
            throw new JsonException("Color name cannot be null or empty.");

        var color = Color.FromName(colorName);

        if (color.IsKnownColor)
            return color; // Valid known color name
        throw new JsonException($"Unknown color name: {colorName}");
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        var colorName = value.Name;

        if (value.IsKnownColor)
        {
            writer.WriteStringValue(colorName);
        }
        else
        {
            var hex = ColorTranslator.ToHtml(value);
            writer.WriteStringValue(hex);
        }
    }
}
