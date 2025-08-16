using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Apoli.Types;

namespace Apoli;

public class ParameterCollection : IEnumerable
{
	private Dictionary<string, Parameter> parameters;

	public IEnumerator GetEnumerator()
	{
		return new ParameterEnumerator(parameters);
	}
	public object GetValue(string key)
	{
		return parameters[key].value;
	}
	public TypeId GetType(string key)
	{
		return parameters[key].type;
	}
	public bool HasParam(string key)
	{
		return parameters.ContainsKey(key);
	}
	public void SetParam(string key, Types.Type value)
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
	public ParameterCollection Clone()
	{
		ParameterCollection newCollection = new();
		foreach (KeyValuePair<string,Parameter> kp in parameters)
		{
			parameters.Add(kp.Key, new(kp.Value.type, kp.Value.value));
		}
		return newCollection;
	}
	public ParameterCollection(params ParameterCollectionInitParam[] args)
	{
		foreach (ParameterCollectionInitParam arg in args)
		{
			if (arg.value is not null)
			{
				parameters.Add(arg.name, new(arg.type, arg.value));
			}
			else
			{
				parameters.Add(arg.name, new(arg.type));
			}
		}
	}
}
public class ParameterCollectionInitParam
{
	public string name;
	public TypeId type;
	public object value;
	public ParameterCollectionInitParam(string _name, TypeId _type, object _value = null)
	{
		name = _name;
		type = _type;
		value = _value;
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
			catch (IndexOutOfRangeException)
			{
				throw new InvalidOperationException();
			}
		}
	}
}
