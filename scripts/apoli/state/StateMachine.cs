using System.Collections.Generic;
using Godot;

namespace Apoli.States;

public class StateMachine
{
    public Dictionary<string, StateLayer> stateLayers = new();
    public Node subject;

    public void AddLayer(string name, StateLayer layer)
    {
        layer.stateMachine = this;
        stateLayers.Add(name, layer);
    }
    public StateLayer GetLayer(string name)
    {
        return stateLayers[name];
    }
    public StateMachine(Node _subject)
    {
        subject = _subject;
    }
    public void Init()
    {
        foreach (KeyValuePair<string,StateLayer> keyValuePair in stateLayers)
        {
            keyValuePair.Value.Init();
        }
    }
}