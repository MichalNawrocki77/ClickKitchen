using InteractiveKitchen.Cursors;
using UnityEngine;
using Zenject;

public class CursorCheckTest : MonoBehaviour
{
    [Inject]
    CursorManager cursorManager;

    [TypeFilter(typeof(CustomCursor))]    
    [SerializeField] SerializableType cursorType;

    [TypeFilter(typeof(CustomCursor))]
    [SerializeField] SerializableType cursorType2;

    [field: TypeFilter(typeof(CustomCursor))]
    [field: SerializeField] public SerializableType cursorTypeProp { get; private set; }

    void OnEnable()
    {
        Debug.Log(cursorManager.GetCustomCursor(cursorType.Type).gameObject.name);
        ForkCursor fork = cursorManager.GetCustomCursor<ForkCursor>();
        Debug.Log(fork.gameObject.name);
    }
}
