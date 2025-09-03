using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtend
{
    public static K GetFirstKey<K,V>(this Dictionary<K,V> dict, V val)
    {
        return dict.FirstOrDefault(x => x.Value.Equals(val)).Key;
    }
}