using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Apoli;
using Apoli.Powers;
using Apoli.States;
using Godot;

namespace Calio.Parser;
public static class StateMachineParser {
    public static StateMachine Parse(string input) {
        StateMachine stateMachine = new(null);

        JsonNode jsonData = JsonNode.Parse(input);
        if (jsonData is JsonObject mainObject) {
            JsonArray layers = mainObject.GetProperty < JsonArray > ("Layers");
            foreach(JsonNode node in layers.ToArray()) {
                if (node is JsonObject layerData) {
                    StateLayer layer = StateLayerParser.Parse(layerData);
                    stateMachine.AddLayer(layer);
                }
                throw new JsonException("Layer not a JsonObject");
            }
        } else {
            throw new JsonException("Machine not a JsonObject");
        }
        return stateMachine;
    }
}
public static class StateLayerParser {
    public static StateLayer Parse(JsonObject layerData) {
        string initialState = layerData.GetProperty < JsonValue > ("InitialState").GetValue < string > ();
        string name = layerData.GetProperty < JsonValue > ("Name").GetValue < string > ();

        bool hasInitial = false;
        StateLayer layer = new(name, initialState);
        JsonArray states = layerData.GetProperty < JsonArray > ("States");
        foreach(JsonNode node in states.ToArray()) {
            if (node is JsonObject stateData) {
                State state = StateParser.Parse(stateData);
                layer.AddState(state);
                if (state.name == initialState) {
                    hasInitial = true;
                }
            }
            throw new JsonException("Layer not a JsonObject");
        }
        if (!hasInitial) {
            throw new JsonException("Invalid InitialState: No states named " + initialState + " found");
        }
        return layer;
    }
}
public static class StateParser {
    public static State Parse(JsonObject stateData) {
        string name = stateData.GetProperty < JsonValue > ("Name").GetValue < string > ();

        State state = new(name);
        JsonArray states = stateData.GetProperty < JsonArray > ("Powers");
        foreach(JsonNode node in states.ToArray()) {
            if (node is JsonObject powerData) {
                Power power = (Power) ApoliObjectParser.ParsePower(powerData);
                state.AddPower(power);
            }
            throw new JsonException("Layer not a JsonObject");
        }
        return state;
    }
}
public static class ApoliObjectParser {
    public static ApoliObject ParsePower(JsonObject powerData) {
        ApoliObjectBuilder builder = PowerBuilderFactory.FromType(TypeIndex.FromString(powerData.GetProperty < JsonValue > ("Type").GetValue < string > (), "Apoli.Powers"));


        return builder.Build();
    }
    public static ApoliObject Parse < Type > (JsonObject objectData) {
        ApoliObjectBuilder < Type > builder = null;


        return builder.Build();
    }
}

public class JsonParseParam {
    public string name;
    public System.Type type;
    public bool required = true;
    public bool IsValid() {
        return name != "" && type is not null;
    }
}
public static class JsonParser
{
    public static Dictionary<string, object> ParseJsonObject(JsonNode jsonNode, List<JsonParseParam> schema)
    {
        if (jsonNode is JsonValue jsonValue)
        {
            throw new InvalidParserException("Wrong parser dumbass, don't pass values with this");
        }
        if (jsonNode is JsonArray or JsonObject)
        {
            throw new InvalidParserException("Wrong parser dumbass, don't pass arrays with this");
        }

        Dictionary<string, object> output = new()
        {
            ["constant"] = false
        };
        JsonObject jsonObject = jsonNode.AsObject();
        foreach (JsonParseParam parseOption in schema)
        {
            if (!parseOption.IsValid())
            {
                throw new InvalidParseParamException("Invalid parse parameter, missing name or type");
            }
            JsonNode parsedNode = jsonObject[parseOption.name];
            if (parseOption.required && parsedNode is null)
            {
                throw new JSONKeyNotFoundException("Found no JSON keys named " + parseOption.name);
            }
            if (parseOption.type.IsPrimitive)
            {
                output.Add(parseOption.name, ParseJsonValue(parsedNode, parseOption));
                continue;
            }
            if (parseOption.type.IsAssignableTo(typeof(ICollection)))
            {
                //Handle arrays and dicts
                GD.Print("UNhandled");
                continue;
            }

            System.Reflection.FieldInfo[] fieldInfos = parseOption.type.GetFields();
            


            output.Add(parseOption.name, parsedNode);
        }
        return output;
    }
    public static object ParseJsonValue(JsonNode jsonNode, JsonParseParam parseOption)
    {
        return Convert.ChangeType(jsonNode[parseOption.name],parseOption.type);
    }
}