using Godot;
using System;
using Apoli;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
namespace BossRush2;

/// <summary>
/// Node must have the name "Player" to be detected
/// </summary>
public partial class Player : Entity
{
	/// <summary>
	/// Moves the player based on set inputs
	/// </summary>
	
    protected StateMachine _StateMachine = new()
    {
        stateLayers = [
            new StateLayer(
                [
                    new State("Idle",[
                        new PowerBuilder()
                        .SetType(PowerId.action_on_callback)
                        .SetParam("ActionOnEnterState",new Apoli.Types.Action(
                            new ActionBuilder()
                            .SetType(ActionId.print)
                            .SetParam("Message",new Apoli.Types.String("Meow"))
                            .Build())
                        )
                        .Build()
                    ]),
                    new State("firing")
                ],
                initialState : "Idle"
            )
        ]
    };
	public void MovementControls()
	{
		//Create a directional vector
		Vector2 inputDir = new(
			Input.GetActionStrength("moveRight") - Input.GetActionStrength("moveLeft"),
			Input.GetActionStrength("moveDown") - Input.GetActionStrength("moveUp")
		);

		//Normalise and apply if not zero
		if (inputDir != Vector2.Zero)
		{
			AccRate += inputDir.Normalized() * GetAcceleration();
		}
	}

	public override void _Process(double delta)
	{
		float deltaF = (float)delta;
		Vector2 mousePos = GetGlobalMousePosition();
		Rotation = Position.AngleToPoint(mousePos);
	}

	public override void _PhysicsProcess(double delta)
	{
		float deltaF = (float)delta;
		//MovementControls();
		UpdateVelocity(deltaF);
	}
}
