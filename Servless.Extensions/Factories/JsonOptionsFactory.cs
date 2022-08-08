using System.Text.Encodings.Web;
using System.Text.Json;

namespace Serverless.Extensions.Factories;

public static class JsonOptionsFactory
{
    public static JsonSerializerOptions GetSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

    }
}
