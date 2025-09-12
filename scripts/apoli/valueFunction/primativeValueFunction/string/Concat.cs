using Godot;
using BossRush2;
using System;

namespace Apoli.ValueFunctions;

public class Concat<Subject> : ValueFunction<string, Subject>
{
    public new static ParameterCollection parameterSet = new(
        new ParameterInit<string>("String1", ""),
        new ParameterInit<string>("String2", "")
    );
    public override string GetValue(Subject subject)
    {
        return GetValue<string>("String1", subject) + GetValue<string>("String2", subject);
    }
}
