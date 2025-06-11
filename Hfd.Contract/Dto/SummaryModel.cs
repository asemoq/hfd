using System.Collections.Generic;
using System.Dynamic;
using Hfd.Common.Contract.Helpers;

namespace Hfd.Contract.Dto;

public class SummaryModel
{
    public List<ColumnModel> Columns { get; set; } = [];

    public List<DynamicRow> Rows { get; set; } = [];
}

public class ColumnModel
{
    public string? Name { get; set; }

    public string? Header { get; set; }

    public string? GroupHeader { get; set; }
}

public sealed partial class DynamicRow : DynamicObject, IDictionary<string, object>
{
    private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
    private string _name;

    public static DynamicRow CreateInstance(string name)
    {
        var instance = new DynamicRow();
        if (name == null)
        {
            return instance;
        }

        instance.Name = name.ToLowerFirstChar();
        instance["header"] = string.Empty; // Uppercase().Replace(name, " $0");
        return instance;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return this.dictionary.TryGetValue(binder.Name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        if (binder.Name == "Name")
        {
            this._name = Convert.ToString(value);
            return true;
        }

        this.dictionary[binder.Name] = value;
        return true;
    }

    public override IEnumerable<string> GetDynamicMemberNames()
    {
        return this.dictionary.Keys;
    }

    public void Add(string key, object value) => this.dictionary.Add(key, value);

    public bool ContainsKey(string key) => this.dictionary.ContainsKey(key);

    public bool Remove(string key) => this.dictionary.Remove(key);

    public bool TryGetValue(string key, out object value) => this.dictionary.TryGetValue(key, out value);

    public object this[string key]
    {
        get => this.dictionary[key];
        set => this.dictionary[key] = value;
    }

    public ICollection<string> Keys => this.dictionary.Keys;

    public ICollection<object> Values => this.dictionary.Values;

    public int Count => this.dictionary.Count;

    public bool IsReadOnly => false;

    public void Add(KeyValuePair<string, object> item) => this.dictionary.Add(item.Key, item.Value);

    public void Clear()
    {
        this.dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item) => this.dictionary.ContainsKey(item.Key);

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        foreach (var kvp in this.dictionary)
        {
            array[arrayIndex++] = new KeyValuePair<string, object>(kvp.Key, kvp.Value);
        }
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.dictionary.GetEnumerator();

    public bool Remove(KeyValuePair<string, object> item) => this.dictionary.Remove(item.Key);

    public string Name
    {
        get => this._name;
        set
        {
            if (this._name == value)
            {
                return;
            }

            this._name = value;
            this.dictionary["Name"] = value;
        }
    }

    private IEnumerator<KeyValuePair<string, object>> GetEnumerator() => this.dictionary.GetEnumerator();

    // [GeneratedRegex("(?<=.)[A-Z]")]
    // private static partial Regex Uppercase();
}
