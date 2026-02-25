using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SerializableTypeChecker : EditorWindow
{
    [SerializeField] VisualTreeAsset view;
    
    Button CheckBtn;
    Label pathsLabel;
    Label allGoodLabel;
    ListView foundObjectObsList;

    [MenuItem("ClickKitchen/Tools/SerializableTypeChecker")]
    public static void ShowWindow()
    {
        SerializableTypeChecker window = GetWindow<SerializableTypeChecker>();
    }

    public void CreateGUI()
    {
        rootVisualElement.Add(view.CloneTree());
        CheckBtn = rootVisualElement.Q<Button>("CheckButton");
        pathsLabel = rootVisualElement.Q<Label>("DetectedPaths");
        allGoodLabel = rootVisualElement.Q<Label>("AllGoodLabel");
        foundObjectObsList = rootVisualElement.Q<ListView>("FoundGameObjectsList");

        pathsLabel.style.display = DisplayStyle.None;
        allGoodLabel.style.display = DisplayStyle.None;
        foundObjectObsList.style.display = DisplayStyle.None;
        CheckBtn.RegisterCallback<ClickEvent>(CheckSerializableRefs);
    }

    void CheckSerializableRefs(ClickEvent evt)
    {
        Scene scene = EditorSceneManager.GetActiveScene();
        Transform[] roots = scene.GetRootGameObjects().Select(go => go.transform).ToArray();

        List<MonoBehaviour> allMonoBehaviours = new List<MonoBehaviour>();
        foreach (Transform root in roots)
        {
            allMonoBehaviours.AddRange(root.GetComponentsInChildren<MonoBehaviour>(true));
        }

        Dictionary<MonoBehaviour, FieldInfo[]> AllSerializableTypeFields = new Dictionary<MonoBehaviour, FieldInfo[]>();
        foreach(MonoBehaviour monoBehaviour in allMonoBehaviours)
        {
            Type type = monoBehaviour.GetType();
                
            FieldInfo[] allSeiralizableTypesFieldsOfMonoBehaviour =
                type.GetFields(BindingFlags.Public | 
                               BindingFlags.NonPublic | 
                               BindingFlags.Instance)
                .Where(field => field.FieldType == typeof(SerializableType))
                .ToArray();              

            
            if(allSeiralizableTypesFieldsOfMonoBehaviour != null && allSeiralizableTypesFieldsOfMonoBehaviour.Length > 0)
            {
                AllSerializableTypeFields.Add(monoBehaviour, allSeiralizableTypesFieldsOfMonoBehaviour);
            }
        }
        
        List<MonoBehaviour> monoBehavioursWithBrokenTypeRefs = new List<MonoBehaviour>();
        
        FieldInfo assemblyQualifiedNameField = 
            typeof(SerializableType).GetField("assemblyQualifiedName", 
                                              BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance);

        foreach(KeyValuePair<MonoBehaviour, FieldInfo[]> monoBehaviour_fieldInfos in AllSerializableTypeFields)
        {
            foreach(FieldInfo field in monoBehaviour_fieldInfos.Value)
            {
                SerializableType serializableTypeRef = field.GetValue(monoBehaviour_fieldInfos.Key) as SerializableType;
                string assemblyQualifiedNameValue = assemblyQualifiedNameField.GetValue(serializableTypeRef) as string;
                Type serializedType = Type.GetType(assemblyQualifiedNameValue);
                if(serializedType == null)
                    monoBehavioursWithBrokenTypeRefs.Add(monoBehaviour_fieldInfos.Key);
            }
        }

        List<string> GameObjectsPathsWithBrokenRefs = new List<string>();
        foreach(MonoBehaviour monoBehaviour in monoBehavioursWithBrokenTypeRefs)
        {
            string path = string.Empty;
            foreach(string pathStep in GetPathToTransform(monoBehaviour.transform))
            {
                path += $"/{pathStep}";
            }
            GameObjectsPathsWithBrokenRefs.Add(path);
        }

        bool anyBrokenRefsExist = GameObjectsPathsWithBrokenRefs.Count > 0; 
        pathsLabel.style.display = anyBrokenRefsExist ? DisplayStyle.Flex : DisplayStyle.None;
        foundObjectObsList.style.display = anyBrokenRefsExist ? DisplayStyle.Flex : DisplayStyle.None;
        allGoodLabel.style.display = anyBrokenRefsExist ? DisplayStyle.None : DisplayStyle.Flex;

        foundObjectObsList.itemsSource = 
            GameObjectsPathsWithBrokenRefs.ToArray();

        foundObjectObsList.RefreshItems();
    }
    string[] GetPathToTransform(Transform trans)
    {
        List<string> pathReversed = new List<string>();
        Transform currentTransform = trans;
        while(currentTransform != null)
        {
            pathReversed.Add(currentTransform.gameObject.name);
            currentTransform = currentTransform.parent;            
        }

        pathReversed.Reverse();
        return pathReversed.ToArray();
    }
}
