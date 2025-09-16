using System;
using System.Collections.Generic;
using Apoli.Powers;

namespace Calio;

public static class TypeIndex
{
    public static Dictionary<string, Type> types = new(){
        {"ActionOnCallback",typeof(ActionOnCallback)}
    };
    public static Type FromString(string type, string ns)
    {
        if (types.ContainsKey(type))
        {
            return types[type];
        }
        return Type.GetType(ns + "." + type);
    }
}