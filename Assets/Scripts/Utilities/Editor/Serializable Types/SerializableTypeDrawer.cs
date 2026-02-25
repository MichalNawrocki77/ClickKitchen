using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SerializableType))]
public class SerializableTypeDrawer : PropertyDrawer
{
    TypeFilterAttribute typeFilter;
    string[] typeNames; 
    string[] typeAssemblyQualifiedNames;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize();

        var typeIDProperty = property.FindPropertyRelative("assemblyQualifiedName");

        if (string.IsNullOrEmpty(typeIDProperty.stringValue))
        {
            typeIDProperty.stringValue = typeAssemblyQualifiedNames.First();
            property.serializedObject.ApplyModifiedProperties();
        }

        int currentIndex = Array.IndexOf(typeAssemblyQualifiedNames, typeIDProperty.stringValue);
        int chosenIndex = EditorGUI.Popup(position, label.text, currentIndex, typeNames);

        if(chosenIndex >= 0 && chosenIndex != currentIndex)
        {
            typeIDProperty.stringValue = typeAssemblyQualifiedNames[chosenIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    void Initialize()
    {
        if(typeAssemblyQualifiedNames != null) return;

        typeFilter = (TypeFilterAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeFilterAttribute));
        if(typeFilter == null)
        {
            typeNames = new string[]{ "Error! CheckConsole!" };
            typeAssemblyQualifiedNames = new string[1];
            Debug.LogError($"Add a [TypeFilter(typeof(YourType))] above your SerializableType field at {fieldInfo.DeclaringType.Name} class so that a list of all classes inheriting from YourType can be displayed");
            return;
        }
        var allFilteredTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeFilter != null ? typeFilter.Filter(type) : DefaultFilter(type))
            .ToArray();

        typeNames = new string[allFilteredTypes.Length];
        typeAssemblyQualifiedNames = new string[allFilteredTypes.Length];
        for (int i = 0; i < allFilteredTypes.Length; i++)
        {
            typeNames[i] = allFilteredTypes[i].Name;
            typeAssemblyQualifiedNames[i] = allFilteredTypes[i].AssemblyQualifiedName;
        }
    }

    static bool DefaultFilter(Type type)
    {
        return !type.IsAbstract && !type.IsInterface && !type.IsGenericType;
    }
}
