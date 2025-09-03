using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apoli.Types;
using Godot;

namespace Apoli;

public class ParameterCollection : IEnumerable
{
	public Dictionary<string, Parameter> parameters = new();

	public IEnumerator GetEnumerator()
	{
		return new ParameterEnumerator(parameters);
	}
	public object GetValue(string key)
	{
		return parameters[key].value.value;
	}
	public T GetValue<T>(string key)
	{
		if (parameters[key].value.value is T tValue)
		{
			return tValue;
		}
		throw new System.Exception("Value at key " + key + " not of type " + typeof(T).Name);
	}
	public T GetValue<T>(string key, Node subject)
	{
		if (parameters[key].value.value is T)
		{
			return (T)parameters[key].value.GetValue(subject);
		}
		throw new System.Exception("Value at key " + key + " not of type " + typeof(T).Name);
	}
	public System.Type GetType(string key)
	{
		return parameters[key].type;
	}
	public Parameter GetParam(string key)
	{
		return parameters[key];
	}
	public bool HasParam(string key)
	{
		return parameters.ContainsKey(key);
	}
	public void SetValue(string key, Type value)
	{
		if (HasParam(key))
		{
			parameters[key].value = value;
		}
		else
		{
			throw new KeyNotFoundException("No key called " + key);
		}
	}
	public void SetParam(string key, Parameter parameter)
	{
		parameters[key] = parameter;
	}
	public void AddFrom(ParameterCollection collection)
	{
		foreach (var keyValuePair in collection.parameters)
		{
			SetParam(keyValuePair.Key, keyValuePair.Value);
		}
	}
	public ParameterCollection Clone()
	{
		ParameterCollection newCollection = new();
		foreach (KeyValuePair<string, Parameter> keyValuePair in parameters)
		{
			parameters.Add(keyValuePair.Key, new Parameter()
			{
				value = keyValuePair.Value.value
			});
		}
		return newCollection;
	}
	public ParameterCollection(params ParameterCollectionInitParam[] args)
	{
		foreach (ParameterCollectionInitParam arg in args)
		{
			parameters.Add(arg.name, arg.ToParameter());
		}
	}
	public override string ToString()
	{
		string output = "";
		foreach (KeyValuePair<string, Parameter> keyValuePair in parameters)
		{
			output += keyValuePair.Key + " : " + keyValuePair.Value.ToString() + "\n";
		}
		return output;
	}
}
public class ParameterCollectionInitParam
{
	public string name;
	public virtual object value { get; set; }
	public virtual Parameter ToParameter()
	{
		return new()
		{
			value = Types.Type.FromValue(value)
		};
	}
}
public class ParameterInit<T> : ParameterCollectionInitParam
{
	public T _value;
    public override object value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = (T)value;
		}
	}
	public ParameterInit(string _name, T __value)
	{
		name = _name;
		value = __value;
	}
	public ParameterInit(string _name, Type<T> __value)
	{
		name = _name;
		value = __value.value;
	}
	public ParameterInit(string _name)
	{
		name = _name;
	}
    public override Parameter ToParameter()
    {
        return new Parameter<T>()
		{
			value = new Type<T>(_value)
		};
    }
}
public class ParameterEnumerator : IEnumerator
{
	public KeyValuePair<string,Parameter>[] _parameter;

	// Enumerators are positioned before the first element
	// until the first MoveNext() call.
	int position = -1;

	public ParameterEnumerator(Dictionary<string,Parameter> list)
	{
		_parameter = list.ToArray();
	}
	public ParameterEnumerator()
	{
	}

	public bool MoveNext()
	{
		position++;
		return (position < _parameter.Length);
	}

	public void Reset()
	{
		position = -1;
	}

	object IEnumerator.Current
	{
		get
		{
			return Current;
		}
	}

	public KeyValuePair<string,Parameter> Current
	{
		get
		{
			try
			{
				return _parameter[position];
			}
			catch (System.IndexOutOfRangeException)
			{
				throw new System.InvalidOperationException();
			}
		}
	}
}
