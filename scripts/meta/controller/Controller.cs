using Godot.Collections;

namespace BossRush2;

public class Controller : IBrObject
{
	public InputMachine inputMachine;
	public Basis2D basis = new();
	public Entity source;
	private bool _active = false;
	public virtual bool active
	{
		get
		{
			return _active;
		}
		set
		{
			_active = value;
		}
	}
	public Array<string> inputs = [];
	public Array<string> variantInputs = [];
	public virtual void InitInputMachine()
	{
		inputMachine = source.inputMachine;
		
		foreach (string id in inputs)
		{
			inputMachine.TryRegisterInput(id);
		}
		foreach (string id in variantInputs)
		{
			inputMachine.TryRegisterVariantInput(id);
		}
	}
	public virtual void Init()
	{
		InitInputMachine();
	}
}
