using System.Collections.Generic;
using System.Dynamic;
using Hfd.Base.Contract.Helpers;

namespace Hfd.Contract.Dto;

public class SummaryModel
{
	public List<ColumnModel> Columns { get; set; } = [];
	public List<DynamicRow> Rows { get; set; } = [];
}

public class ColumnModel
{
	public string Name { get; set; }
	public string Header { get; set; }
	public string GroupHeader { get; set; }
}

public class DynamicRow : DynamicObject, IDictionary<string, object>
{
	private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();
	private string _name;

	public static DynamicRow CreateInstance(string name)
	{
		var instance = new DynamicRow();
		if (name != null)
		{
			instance.Name = name.ToLowerFirstChar();
			instance["header"] = name;
		}

		return instance;
	}

	public override bool TryGetMember(GetMemberBinder binder, out object result)
	{
		return _dictionary.TryGetValue(binder.Name, out result);
	}

	public override bool TrySetMember(SetMemberBinder binder, object value)
	{
		if (binder.Name == "Name")
		{
			_name = Convert.ToString(value);
			return true;
		}

		_dictionary[binder.Name] = value;
		return true;
	}

	public override IEnumerable<string> GetDynamicMemberNames()
	{
		return _dictionary.Keys;
	}

	public void Add(string key, object value) => _dictionary.Add(key, value);
	public bool ContainsKey(string key) => _dictionary.ContainsKey(key);
	public bool Remove(string key) => _dictionary.Remove(key);
	public bool TryGetValue(string key, out object value) => _dictionary.TryGetValue(key, out value);

	public object this[string key]
	{
		get => _dictionary[key];
		set => _dictionary[key] = value;
	}

	public ICollection<string> Keys => _dictionary.Keys;
	public ICollection<object> Values => _dictionary.Values;
	public int Count => _dictionary.Count;
	public bool IsReadOnly => false;

	public void Add(KeyValuePair<string, object> item) => _dictionary.Add(item.Key, item.Value);

	public void Clear()
	{
		_dictionary.Clear();
	}

	public bool Contains(KeyValuePair<string, object> item) => _dictionary.ContainsKey(item.Key);

	public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
	{
		foreach (var kvp in _dictionary)
		{
			array[arrayIndex++] = new KeyValuePair<string, object>(kvp.Key, kvp.Value);
		}
	}

	IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _dictionary.GetEnumerator();
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

	public bool Remove(KeyValuePair<string, object> item) => _dictionary.Remove(item.Key);

	public string Name
	{
		get => _name;
		set
		{
			if (_name == value) return;
			_name = value;
			_dictionary["Name"] = value;
		}
	}
}