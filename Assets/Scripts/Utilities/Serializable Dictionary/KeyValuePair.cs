using System;
using UnityEngine;

namespace InteractiveKitchen.Utilities
{
    [Serializable]
    public class KeyValuePair<TKey, TValue> : ISerializationCallbackReceiver
    {
        public KeyValuePair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public TKey Key { get; }
        public TValue Value { get; }

        public void OnAfterDeserialize()
        {
        }

        public void OnBeforeSerialize()
        {
        }
    }
}
