using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Apoli;
using Apoli.Powers;
using Apoli.States;

namespace Calio.Parser;
public static class StateMachineParser
{
    public static StateMachine Parse(string input)
    {
        StateMachine stateMachine = new(null);

        JsonNode jsonData = JsonNode.Parse(input);
        if (jsonData is JsonObject mainObject)
        {
            JsonArray layers = mainObject.GetProperty<JsonArray>("Layers");
            foreach (JsonNode node in layers.ToArray())
            {
                if (node is JsonObject layerData)
                {
                    StateLayer layer = StateLayerParser.Parse(layerData);
                    stateMachine.AddLayer(layer);
                }
                throw new JsonException("Layer not a JsonObject");
            }
        }
        else
        {
            throw new JsonException("Machine not a JsonObject");
        }
        return stateMachine;
    }
}
public static class StateLayerParser
{
    public static StateLayer Parse(JsonObject layerData)
    {
        string initialState = layerData.GetProperty<JsonValue>("InitialState").GetValue<string>();
        string name = layerData.GetProperty<JsonValue>("Name").GetValue<string>();

        bool hasInitial = false;
        StateLayer layer = new(name,initialState);
        JsonArray states = layerData.GetProperty<JsonArray>("States");
        foreach (JsonNode node in states.ToArray())
        {
            if (node is JsonObject stateData)
            {
                State state = StateParser.Parse(stateData);
                layer.AddState(state);
                if (state.name == initialState)
                {
                    hasInitial = true;
                }
            }
            throw new JsonException("Layer not a JsonObject");
        }
        if (!hasInitial)
        {
            throw new JsonException("Invalid InitialState: No states named " + initialState + " found");
        }
        return layer;
    }
}
public static class StateParser
{
    public static State Parse(JsonObject stateData)
    {
        string name = stateData.GetProperty<JsonValue>("Name").GetValue<string>();

        State state = new(name);
        JsonArray states = stateData.GetProperty<JsonArray>("Powers");
        foreach (JsonNode node in states.ToArray())
        {
            if (node is JsonObject powerData)
            {
                Power power = (Power)ApoliObjectParser.ParsePower(powerData);
                state.AddPower(power);
            }
            throw new JsonException("Layer not a JsonObject");
        }
        return state;
    }
}
public static class ApoliObjectParser
{
    public static ApoliObject ParsePower(JsonObject powerData)
    {
        ApoliObjectBuilder builder = PowerBuilderFactory.FromType(TypeIndex.FromString(powerData.GetProperty<JsonValue>("Type").GetValue<string>(),"Apoli.Powers"));


        return builder.Build();
    }
    public static ApoliObject Parse<Type>(JsonObject objectData)
    {
        ApoliObjectBuilder<Type> builder = null;


        return builder.Build();
    }
}