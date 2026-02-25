using System;
using UnityEngine;

/// <summary>
/// Holds a reference to a type and serializes it's name so it can be displayed in the inspector
/// Be aware that if you rename a class that is serialized by some MonoBehaviour, the reference will be broken
/// </summary>
[Serializable]
public class SerializableType : ISerializationCallbackReceiver
{
    [SerializeField] string assemblyQualifiedName = string.Empty;
    public Type Type { get; private set; }
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        if(Type != null)
            assemblyQualifiedName = Type.AssemblyQualifiedName;
    }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        if(!TryGetTypeFromName(assemblyQualifiedName, out Type readType))
        {
            Debug.LogError($"Type {assemblyQualifiedName} was not found when deserializing a SerializableType");
            return;
        }
        this.Type = readType;
    }


    static bool TryGetTypeFromName(string assemblyQualifiedName, out Type type)
    {
        type = Type.GetType(assemblyQualifiedName);
        return type != null;
    }
}
