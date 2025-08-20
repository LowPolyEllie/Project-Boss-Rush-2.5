using Apoli.States;
using Godot;

namespace Apoli.States;

public interface IStateMachine
{
    public StateMachine stateMachine { get; set; }
}