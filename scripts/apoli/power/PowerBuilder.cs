using Apoli.Types;

namespace Apoli.Powers;

public class PowerBuilder<PowerType> : ApoliObjectBuilder<PowerType> where PowerType : Power
{
    public PowerBuilder<PowerType> SetParam<T>(string Key, IValue<T> Value)
    {
        _SetParam<T>(Key, Value);
        return this;
    }
    public PowerBuilder<PowerType> SetParam<T>(string Key, T Value)
    {
        _SetParam(Key, Value);
        return this;
    }
    public new Power Build()
    {
        return (Power)_Build();
    }
}

public static class PowerBuilderFactoryHelper
{
    public static PowerBuilder<T> NewBuilder<T>() where T : Power
    {
        return new();
    }
}

public static class PowerBuilderFactory
{
    public static ApoliObjectBuilder FromType(System.Type type)
    {
        ApoliObjectBuilder builder = (ApoliObjectBuilder)typeof(PowerBuilderFactoryHelper).GetMethod("NewBuilder").MakeGenericMethod([type]).Invoke(null, []);
        return builder;
    }
}