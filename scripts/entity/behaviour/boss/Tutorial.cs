using Godot;
using Apoli.Powers;
using Apoli.Types;
using Apoli.Actions;
using Apoli.States;
using Apoli.ValueFunctions;
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
				.SetParam<IAction<Entity>>("EntityAction",
					new ActionBuilder<Print<Entity>>()
					.SetParam<string>("Message",
						new ValueFunctionBuilder<Concat<Entity>>()
						.SetParam("String1","MY NAME IS ")
						.SetParam<string>("String2",
							new ValueFunctionBuilder<GetName>()
							.Build()
						)
						.Build()
					)
					.Build()
				)
				.Build()
			}
		}
		);
		stateMachine.Init();
	}
}
