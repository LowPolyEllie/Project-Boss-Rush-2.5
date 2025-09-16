using System.Text.Json;
using System.Text.Json.Nodes;

namespace Calio;

public static class Utils
{
    public static T GetProperty<T>(this JsonObject jsonObject, string name) where T : JsonNode
    {
        JsonNode property = jsonObject![name];
        if (property is null)
        {
            throw new JsonException("No property named " + name);
        }
        if (property is T tP)
        {
            return tP;
        }
        throw new JsonException("Property " + name + " is type " + property.GetType().Name + ", expected" + typeof(T).Name);
    }
}