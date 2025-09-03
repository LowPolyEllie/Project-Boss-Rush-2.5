using Godot;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using Apoli.States;
namespace BossRush2;

[GlobalClass]
public partial class Tutorial : Basic
{
	public override void _Ready()
	{
		Init();
		stateMachine = new(this);
		stateMachine.AddLayer("Base", new StateLayer("Idle")
		{
			new State("Idle"){
				new PowerBuilder<ActionOnPhysicsTick>()
					.SetParam("EntityAction", new Type<IAction<Entity>>(
						(IAction<Entity>)new ActionBuilder<Print>()
							.SetParam("Message",
								Type.FromValue("CMONNNN")
							)
						.Build()))
				.Build()
			}
		}
		);
		stateMachine.Init();
	}
}
