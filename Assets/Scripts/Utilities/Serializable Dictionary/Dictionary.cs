using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
namespace InteractiveKitchen.Utilities
{
    [Serializable]
    public class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] List<KeyValuePair<TKey, TValue>> serializedKVPs;
        public void OnBeforeSerialize()
        {
            serializedKVPs = new List<KeyValuePair<TKey, TValue>>();
            foreach(var systemKVP in this)
            {
                serializedKVPs.Add(new KeyValuePair<TKey, TValue>(systemKVP.Key, systemKVP.Value));
            }
        }
        public void OnAfterDeserialize()
        {
            Clear();
            for(int i = 0; i < serializedKVPs.Count; i++)
            {
                if (!ContainsKey(serializedKVPs[i].Key))
                {
                    Add(serializedKVPs[i].Key, serializedKVPs[i].Value);
                }
            }
        }
    }
}

