#nullable disable
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Globalization;
using System.Text.RegularExpressions;
using Hfd.Common.Contract.Helpers;

namespace Hfd.Contract.Dto;

public class SummaryModel
{
    public Collection<ColumnModel> Columns { get; } = [];

    public Collection<DynamicRowDictionary> Rows { get; } = [];
}

public class ColumnModel
{
    public string Name { get; set; }

    public string Header { get; set; }

    public string GroupHeader { get; set; }
}

public sealed partial class DynamicRowDictionary : DynamicObject, IDictionary<string, object>
{
    private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();
    private string name;

    public ICollection<string> Keys => this.dictionary.Keys;

    public ICollection<object> Values => this.dictionary.Values;

    public int Count => this.dictionary.Count;

    public bool IsReadOnly => false;

    public string Name
    {
        get => this.name;
        set
        {
            if (this.name == value)
            {
                return;
            }

            this.name = value;
            this.dictionary["Name"] = value;
        }
    }

    public object this[string key]
    {
        get => this.dictionary[key];
        set => this.dictionary[key] = value;
    }

    public static DynamicRowDictionary CreateInstance(string name)
    {
        var instance = new DynamicRowDictionary();
        if (name == null)
        {
            return instance;
        }

        instance.Name = name.ToLowerFirstChar();
        instance["header"] = Uppercase().Replace(name, " $0");
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
            this.name = Convert.ToString(value, CultureInfo.CurrentCulture);
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

    [GeneratedRegex("(?<=.)[A-Z]")]
    private static partial Regex Uppercase();

    private IEnumerator<KeyValuePair<string, object>> GetEnumerator() => this.dictionary.GetEnumerator();
}
