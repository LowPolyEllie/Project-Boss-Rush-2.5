using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apoli.Types;
using Apoli.ValueFunctions;
using Godot;

namespace Apoli;

public class ParameterCollection : IEnumerable
{
	public Dictionary<string, Parameter> parameters = new();

	public IEnumerator GetEnumerator()
	{
		return new ParameterEnumerator(parameters);
	}
	public T GetValue<T>(string key)
	{
		IValue<T> value = parameters[key].value as IValue<T>;
		if (value is null) {
			throw new System.Exception("Value at key " + key + " not of type " + typeof(T).Name);
		}
		return value.GetValue();
	}
	public T GetValue<T,Subject>(string key, Subject subject)
	{
		IValueFunction<T,Subject> value = parameters[key].value as IValueFunction<T,Subject>;
		if (value is null)
		{
			return GetValue<T>(key);
		}
		return value.GetValue(subject);
	}
	public System.Type GetType(string key)
	{
		if (HasParam(key))
		{
			return parameters[key].type;
		}
		else
		{
			throw new KeyNotFoundException("No key called " + key);
		}
	}
	public Parameter GetParam(string key)
	{
		return parameters[key];
	}
	public bool HasParam(string key)
	{
		return parameters.ContainsKey(key);
	}
	public void SetValue<T>(string key, IValue<T> value)
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
			output += keyValuePair.Key + " : " + keyValuePair.Value?.ToString() + "\n";
		}
		return output;
	}
}
public class ParameterCollectionInitParam
{
	public string name;
	public object value { get; set; }
	public virtual Parameter ToParameter() { return default; }
}
public class ParameterInit<T> : ParameterCollectionInitParam
{
	public new T value;
	public ParameterInit(string _name, T _value)
	{
		name = _name;
		value = _value;
	}
	public ParameterInit(string _name, Type<T> _value)
	{
		name = _name;
		value = _value.value;
	}
	public ParameterInit(string _name)
	{
		name = _name;
	}
	public override Parameter ToParameter()
	{
		return new Parameter<T>()
		{
			value = new Type<T>(value)
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
