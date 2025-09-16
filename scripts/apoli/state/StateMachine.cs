using System;
using System.Collections.Generic;
using Godot;

namespace Apoli.States;

public class StateMachine
{
    public List<StateLayer> stateLayers = new();
    public Node subject;

    public void AddLayer(StateLayer layer)
    {
        if (stateLayers.FindIndex((_layer) => _layer.name == layer.name) > -1)
        {
            throw new DuplicateKeyException("StateMachine already has layer called " + layer.name);
        }
        layer.stateMachine = this;
        stateLayers.Add(layer);
    }
    public StateLayer GetLayer(string name)
    {
        return stateLayers.Find((layer)=>layer.name == name);
    }
    public StateMachine(Node _subject)
    {
        subject = _subject;
    }
    public void Init()
    {
        foreach (StateLayer layer in stateLayers)
        {
            layer.Init();
        }
    }
}