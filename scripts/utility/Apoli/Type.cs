using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
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
public abstract class Type {
    public string id;
    public abstract TypeId type {
        get;
        set;
    }
    public abstract object value {
        get;
        set;
    }
}
public class String: Type {
    public string _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (string) value;
        }
    }
    public override TypeId type {
        get {
            return TypeId.String;
        }
        set {}
    }
    public String(string __value = "") {
        value = __value;
    }
}
public class Power: Type {
    public Powers.Power _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (Powers.Power) value;
        }
    }
    public override TypeId type {
        get {
            return TypeId.Power;
        }
        set {}
    }
    public Power(Powers.Power __value = null) {
        value = __value;
    }
}
public class Action: Type {
    public Actions.Action _value;
    public override object value {
        get {
            return _value;
        }
        set {
            _value = (Actions.Action) value;
        }
    }
    public override TypeId type {
        get {
            return TypeId.Action;
        }
        set {}
    }
    public Action(Actions.Action __value = null) {
        value = __value;
    }
}