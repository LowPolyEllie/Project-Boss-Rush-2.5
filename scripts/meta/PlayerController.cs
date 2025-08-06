using Godot;
using Godot.Collections;

namespace BossRush2;

public class PlayerController : Controller
{
    public Camera camera;
    private bool _active = false;
    public override bool active
    {
        set
        {
            _active = value;
            camera.Enabled = _active;
        }
    }
    public override void InitInputMachine()
    {
        if (!keyMapping.ContainsKey("Up")){ keyMapping.Add("Up", Key.W); }
        if (!keyMapping.ContainsKey("Down")){ keyMapping.Add("Up", Key.S); }
        if (!keyMapping.ContainsKey("Left")){ keyMapping.Add("Up", Key.A); }
        if (!keyMapping.ContainsKey("Right")){ keyMapping.Add("Up", Key.D); }
        if (!keyMapping.ContainsKey("Fire")){ keyMapping.Add("Up", Key.Launch1); }
        variantInput = ["Target"];

        base.InitInputMachine();

        camera.TargetEntity = source;
        
    }
    public override void ProcessInput(double delta)
    {
        //Kinda jank but it works
        inputMachine.SetVariantInput("Target", source.GetGlobalMousePosition());
    }
}