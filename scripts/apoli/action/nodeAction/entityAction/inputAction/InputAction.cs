using BossRush2;

namespace Apoli.Actions;

public class InputAction : EntityAction
{
    protected string input;
    public new static ParameterCollection parameterSet = new(
        new ParameterInit<string>("Input", "")
    );
    public override void Init()
    {
        input = parameters.GetValue<string>("Input");
    }
}
public class BeginInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.BeginInput;
    public override void _DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputEnable(input);
    }
}
public class EndInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.EndInput;
    public override void _DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputDisable(input);
    }
}
public class FireInput : InputAction
{
    public override ActionId type { get; set; } = ActionId.FireInput;
    public override void _DoEntityAction(Entity subject)
    {
        subject.inputMachine.InputFire(input);
    }
}