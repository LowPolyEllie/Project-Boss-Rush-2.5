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
public enum TypeId {
    String,
    Float,
    Power,
    Action
}
public class Type {
    public string id;
    public virtual TypeId type { get; set; }
    public virtual object value { get; set; }
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
public class Power: Type {
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
    public Power(Powers.Power __value = null) {
        value = __value;
    }
}
public class Action: Type {
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
    public Action(Actions.Action __value = null) {
        value = __value;
    }
}