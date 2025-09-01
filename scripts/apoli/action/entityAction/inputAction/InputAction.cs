using Godot;
using Apoli.Types;
using BossRush2;
using Apoli.Powers;

namespace Apoli.Actions;

public class InputAction : EntityAction
{
    protected string input;
	public new static ParameterCollection parameterSet = new(
        new ParameterCollectionInitParam("Input", TypeId.String, "")
    );
    public override void Init()
    {
        input = parameters.GetValue<string>("Input");
    }
}
public class BeginInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.BeginInput;
    public override void DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputEnable(input);
    }
}
public class EndInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.EndInput;
    public override void DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputDisable(input);
    }
}
public class FireInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.FireInput;
    public override void DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputFire(input);
    }
}