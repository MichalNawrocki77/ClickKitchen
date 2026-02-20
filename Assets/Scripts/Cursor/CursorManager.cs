using UnityEngine;
using UnityEngine.InputSystem;
using InteractiveKitchen;
using static UnityEngine.InputSystem.InputAction;
using System.Linq;
using InteractiveKitchen.Debugging;

namespace InteractiveKitchen.Cursors
{    
    public class CursorManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] RectTransform cursorPointerRect; 

        #if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField] bool HideCursor;
        #endif

        CustomCursor[] allCustomCursors;
        public CustomCursor CurrentCursor { get; private set; }


        GlobalGameInput input;
        void Awake()
        {
            input = new GlobalGameInput();
            input.UI.Enable();
            allCustomCursors = GetComponentsInChildren<CustomCursor>(true);

            foreach(CustomCursor cursor in allCustomCursors)
            {
                cursor.Inject(this, input);
                cursor.gameObject.SetActive(false);
            }
        }

        void Start()
        {
            if(CurrentCursor == null)
                ChangeCursor<ForkCursor>();
        }

        void OnEnable()
        {
            //Hides hardware cursor
            //In builds: Alawys
            //In Editor: if you so choose 
            bool shoudlHideCursor = true;
            #if UNITY_EDITOR
            shoudlHideCursor = HideCursor;
            #endif
            Cursor.visible = !shoudlHideCursor;

            input.UI.Point.performed += MousePositionActionCallback;
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ForceChangeCursorPosition(new Vector2(Screen.width/2, Screen.height/2));
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                ChangeCursor<ForkCursor>();
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                ChangeCursor<KnifeCursor>();
            }
        }
        void OnDisable()
        {
            input.UI.Point.performed -= MousePositionActionCallback;
            Cursor.visible = true;
            
        }
        void MousePositionActionCallback(CallbackContext ctx)
        {
            UpdateCursorPosition(ctx.ReadValue<Vector2>());
        }

        void UpdateCursorPosition(Vector2 ScreenPosition)
        {
            cursorPointerRect.position = ScreenPosition;
        }

        /// <summary>
        /// Forcibly change the position of the cursor.
        /// NOTE: Since the cursor manager uses the same InputAction as Unity's InputModules, in order to change the position
        /// this method uses Mouse.WarpCursorPosition in order to achieve it.
        /// </summary>
        public void ForceChangeCursorPosition(Vector2 screenSpacePos)
        {
            Mouse.current.WarpCursorPosition(screenSpacePos);
        }

        public T GetCustomCursor<T>() where T : CustomCursor =>
            allCustomCursors.FirstOrDefault(cursor => cursor is T) as T;

        public void ChangeCursor<T>() where T : CustomCursor
        {
            CustomCursor newCursor = GetCustomCursor<T>();
            if(newCursor == null)
            {
                DebugUtils.LogError($"{GetType().Name} does not have a CustomCursor of type {typeof(T).Name} in it's array of CustomCursors!");
                return;
            }
        
            if(CurrentCursor != null)
            {
                CurrentCursor.OnDeselected();
                CurrentCursor.gameObject.SetActive(false);
            }
            CurrentCursor = newCursor;
            CurrentCursor.OnSelected();
            CurrentCursor.gameObject.SetActive(true);
        }
    }
}