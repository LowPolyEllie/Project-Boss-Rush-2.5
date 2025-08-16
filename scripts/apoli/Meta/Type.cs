using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using Apoli.Actions;
using Apoli.Powers;
using Godot;

namespace Apoli.Types;

public enum TypeId
{
    Default,
    String,
    Float,
    Power,
    PowerCollection,
    Action,
    ActionCollection,
    Condition,
    ConditionCollection,
    Bool,
    TypeIdType,
    Variant
}
public class Type
{
    public string id;
    public virtual TypeId type { get; set; }
    public virtual object value { get; set; }
    public static Type FromValue(object value)
    {
        switch (value)
        {
            case bool boolValue:
                return new Bool(boolValue);
            case string strValue:
                return new String(strValue);
        }
        return new Type();
    }
}
public class Bool: Type {
    public override TypeId type { get; set; } = TypeId.Bool;
    public bool _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (bool) value;
        }
    }
    public Bool(bool __value = true) {
        value = __value;
    }
}
public class TypeIdType: Type {
    public override TypeId type { get; set; } = TypeId.TypeIdType;
    public TypeId _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (TypeId) value;
        }
    }
    public TypeIdType(TypeId __value) {
        value = __value;
    }
}
public class String: Type {
    public override TypeId type { get; set; } = TypeId.String;
    public string _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (string) value;
        }
    }
    public String(string __value = "") {
        value = __value;
    }
}
public class PowerType: Type {
    public override TypeId type { get; set; } = TypeId.Power;
    public Powers.Power _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (Powers.Power) value;
        }
    }
    public PowerType(Powers.Power __value = null) {
        value = __value;
    }
}
public class ActionType: Type {
    public override TypeId type { get; set; } = TypeId.Action;
    public Actions.Action _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (Actions.Action) value;
        }
    }
    public ActionType(Actions.Action __value = null) {
        value = __value;
    }
}
public class ActionCollection: Type {
    public override TypeId type { get; set; } = TypeId.ActionCollection;
    public List<Actions.Action> _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (List<Actions.Action>) value;
        }
    }
    public ActionCollection(List<Actions.Action> __value = null) {
        value = __value == null? [] : __value;
    }
}