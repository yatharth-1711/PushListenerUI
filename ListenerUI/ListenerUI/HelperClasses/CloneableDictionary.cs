using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDesktopWFA
{
    public class CloneableDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : ICloneable
    {
        public CloneableDictionary<TKey, TValue> Clone()
        {
            CloneableDictionary<TKey, TValue> clone = new CloneableDictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> kvp in this)
            {
                clone.Add(kvp.Key, (TValue)kvp.Value.Clone());
            }
            return clone;
        }
    }
}
