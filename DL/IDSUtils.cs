using System;
using System.Collections.Concurrent; // ConcurrentDictionary
using System.Collections.Generic; // Ienumerable
using System.Collections.Specialized; // NameValueCollection

namespace CurrencyConversion
{
    /// <summary>
    /// Deserialize data source that has a list of iterElement where each 
    /// iterElement contains at least 2 child elements, one that will be used as key and on that'll be used as value.
    /// to dictionary of {"key": val ....} pairs.
    /// </summary>
    public interface IDSUtils<TKey, TVal>
    {
        TVal GetVal(TKey key);
        bool UpdateDS(NameValueCollection dsConfig);
    }


    static class ToDictionaryExtentions
    {
        //extends the original .ToDictionary()
        //duplicate keys are ignored, method doesn't throw "same key exception"
        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionaryIgnoreDups<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");
            ConcurrentDictionary<TKey, TElement> d = new ConcurrentDictionary<TKey, TElement>();
            foreach (TSource element in source)
            {
                d.GetOrAdd(keySelector(element), elementSelector(element)); //ignores duplicates, adds only new keys
            }
            return d;
        }

    }
}
