using System.Collections.Generic;
namespace BossRush2;
public interface IInputMachine
{
	public List<string> inputs { get; set; }
	public List<string> variantInputs { get; set; }
}