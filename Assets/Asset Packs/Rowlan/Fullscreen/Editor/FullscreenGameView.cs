using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rowlan.Fullscreen
{
    /// <summary>
    /// True fullscreen Game View in the Unity Editor (Windows + macOS).
    /// No menus, no toolbar, no status bar, no tabs, no OS taskbar/dock.
    ///
    /// USAGE:
    ///   - Press F11 (configurable) during Play Mode to toggle fullscreen on/off
    ///   - Press Escape (configurable) to exit fullscreen without stopping play
    ///   - Menu: Tools → Rowlan → Fullscreen → Fullscreen On Play — toggles auto-fullscreen on play
    ///   - Menu: Tools → Rowlan → Fullscreen → Fullscreen Reset — safety fallback to restore layout
    ///   - Preferences: Edit → Preferences → Rowlan/Fullscreen — configure keybinds and behavior
    ///
    /// REQUIREMENTS:
    ///   - Unity 6.000.40+ (for hiding GameView toolbar via showToolbar property)
    /// </summary>
    public static class FullscreenGameView
    {
        #region Private State

        private static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");

        private static readonly PropertyInfo ShowToolbarProperty =
            GameViewType?.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo ShowStatsField =
            GameViewType?.GetField("m_Stats", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo ShowGizmosField =
            GameViewType?.GetField("m_Gizmos", BindingFlags.Instance | BindingFlags.NonPublic);

        private const int DisplayActive = 0;
        private const int DisplayInactive = 1;

        private static EditorWindow fullscreenInstance;

        #endregion

        #region Public Properties

        /// <summary>
        /// Whether the editor is currently in fullscreen mode.
        /// </summary>
        public static bool IsFullscreen => fullscreenInstance != null;

        #endregion

        #region Menu Items

        /// <summary>
        /// Toggles the auto-fullscreen preference. When checked, entering Play Mode
        /// will automatically activate fullscreen. This menu item performs no immediate action.
        /// </summary>
        [MenuItem("Tools/Rowlan/Fullscreen/Fullscreen On Play", priority = 2)]
        private static void ToggleAutoFullscreen()
        {
            FullscreenSettings.AutoFullscreen = !FullscreenSettings.AutoFullscreen;
        }

        /// <summary>
        /// Validates the menu item checkmark state to reflect the current preference.
        /// </summary>
        [MenuItem("Tools/Rowlan/Fullscreen/Fullscreen On Play", true)]
        private static bool ToggleAutoFullscreenValidate()
        {
            Menu.SetChecked("Tools/Rowlan/Fullscreen/Fullscreen On Play", FullscreenSettings.AutoFullscreen);
            return true;
        }

        /// <summary>
        /// Exits fullscreen and resets the Unity editor layout to default.
        /// </summary>
        [MenuItem("Tools/Rowlan/Fullscreen/Fullscreen Reset", priority = 3)]
        private static void ResetUnityLayout()
        {
            if (fullscreenInstance != null)
            {
                FullscreenPlatform.OnExitFullscreen();
                fullscreenInstance.Close();
                fullscreenInstance = null;
            }

            SetGameViewTargetDisplay(DisplayActive);
            EditorApplication.ExecuteMenuItem("Window/Layouts/Default");
        }

        #endregion

        #region Public API

        /// <summary>
        /// Toggles fullscreen mode on or off. Called by the runtime key listener via delegate.
        /// </summary>
        public static void Toggle()
        {
            if (GameViewType == null)
            {
                Debug.LogError("[Fullscreen] GameView type not found.");
                return;
            }

            if (ShowToolbarProperty == null)
            {
                Debug.LogWarning("[Fullscreen] GameView.showToolbar not found. " +
                                 "Play toolbar may remain visible. Requires Unity 6.000.40+.");
            }

            if (fullscreenInstance != null)
                ExitFullscreen();
            else
                EnterFullscreen();
        }

        #endregion

        #region Core Logic

        /// <summary>
        /// Creates a borderless popup GameView covering the entire screen
        /// and invokes platform-specific logic to cover the OS taskbar/dock.
        /// </summary>
        private static void EnterFullscreen()
        {
            // Capture overlay states from the original GameView before redirecting it
            EditorWindow originalGameView = GetMainGameView();
            bool showStats = GetFieldValue<bool>(originalGameView, ShowStatsField);
            bool showGizmos = GetFieldValue<bool>(originalGameView, ShowGizmosField);

            // Redirect original GameView to unused display to avoid double-rendering
            SetGameViewTargetDisplay(DisplayInactive);

            // Create a fresh GameView instance
            fullscreenInstance = (EditorWindow)ScriptableObject.CreateInstance(GameViewType);

            // Hide the play/pause toolbar inside the GameView
            ShowToolbarProperty?.SetValue(fullscreenInstance, false);

            // Copy overlay states from the original GameView
            SetFieldValue(fullscreenInstance, ShowStatsField, showStats);
            SetFieldValue(fullscreenInstance, ShowGizmosField, showGizmos);

            // Calculate rect in editor points (accounts for DPI / Retina scaling)
            int pixelW = Screen.currentResolution.width;
            int pixelH = Screen.currentResolution.height;
            float scale = EditorGUIUtility.pixelsPerPoint;
            Vector2 resolution = new Vector2(pixelW / scale, pixelH / scale);
            Rect fullscreenRect = new Rect(Vector2.zero, resolution);

            // ShowPopup() creates a borderless window with no Unity chrome
            fullscreenInstance.ShowPopup();
            fullscreenInstance.position = fullscreenRect;
            fullscreenInstance.Focus();

            // Platform-specific: force window over taskbar / dock
            FullscreenPlatform.OnEnterFullscreen(fullscreenInstance, pixelW, pixelH);

            Debug.Log($"[Fullscreen] Entered ({pixelW}x{pixelH}, scale {scale}x). " +
                      $"Press {FullscreenSettings.ToggleKey} or {FullscreenSettings.ExitKey} to exit.");
        }

        /// <summary>
        /// Closes the fullscreen popup, restores platform window state,
        /// and redirects the original GameView back to display 0.
        /// </summary>
        private static void ExitFullscreen()
        {
            FullscreenPlatform.OnExitFullscreen();

            if (fullscreenInstance != null)
            {
                fullscreenInstance.Close();
                fullscreenInstance = null;
            }

            SetGameViewTargetDisplay(DisplayActive);

            Debug.Log("[Fullscreen] Exited fullscreen.");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Finds and returns the main GameView editor window instance.
        /// </summary>
        /// <returns>The GameView EditorWindow, or null if not found.</returns>
        private static EditorWindow GetMainGameView()
        {
            Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
            if (type == null) return null;
            return EditorWindow.GetWindow(type);
        }

        /// <summary>
        /// Redirects the main GameView to render from a specific display index.
        /// Used to avoid double-rendering when the fullscreen popup is active.
        /// </summary>
        /// <param name="displayIndex">The display index to target (0 = default, 1+ = unused).</param>
        private static void SetGameViewTargetDisplay(int displayIndex)
        {
            EditorWindow gameView = GetMainGameView();
            if (gameView == null) return;

            gameView.GetType().InvokeMember(
                "SetTargetDisplay",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod,
                null,
                gameView,
                new object[] { displayIndex }
            );
        }

        /// <summary>
        /// Safely reads a private field value from an EditorWindow via reflection.
        /// Returns the default value if the window or field is null.
        /// </summary>
        /// <typeparam name="T">The field type.</typeparam>
        /// <param name="window">The EditorWindow instance to read from.</param>
        /// <param name="field">The reflected FieldInfo.</param>
        /// <returns>The field value, or default(T) if unavailable.</returns>
        private static T GetFieldValue<T>(EditorWindow window, FieldInfo field)
        {
            if (window == null || field == null) return default;
            return (T)field.GetValue(window);
        }

        /// <summary>
        /// Safely writes a private field value on an EditorWindow via reflection.
        /// No-op if the window or field is null.
        /// </summary>
        /// <param name="window">The EditorWindow instance to write to.</param>
        /// <param name="field">The reflected FieldInfo.</param>
        /// <param name="value">The value to set.</param>
        private static void SetFieldValue(EditorWindow window, FieldInfo field, object value)
        {
            if (window == null || field == null) return;
            field.SetValue(window, value);
        }

        #endregion

        #region Callbacks

        /// <summary>
        /// Registers a callback to handle Play Mode transitions.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void RegisterCallbacks()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Handles Play Mode transitions: injects keybinds, wires delegates,
        /// spawns the key listener on enter, auto-enters fullscreen if enabled,
        /// and exits fullscreen on stop.
        /// </summary>
        /// <param name="state">The current play mode transition state.</param>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                InjectKeybinds();
                SpawnKeyListener();

                if (FullscreenSettings.AutoFullscreen)
                    EditorApplication.delayCall += () => EnterFullscreen();
            }
            else if (state == PlayModeStateChange.ExitingPlayMode && fullscreenInstance != null)
            {
                ExitFullscreen();
            }
        }

        /// <summary>
        /// Copies the current keybind preferences into the shared runtime FullscreenKeybinds
        /// and wires up the delegate callbacks so the runtime listener can invoke editor logic.
        /// </summary>
        private static void InjectKeybinds()
        {
            FullscreenKeybinds.ToggleKey = FullscreenSettings.ToggleKey;
            FullscreenKeybinds.ExitKey = FullscreenSettings.ExitKey;

            FullscreenKeyListener.OnTogglePressed = Toggle;
            FullscreenKeyListener.IsFullscreen = () => IsFullscreen;
        }

        /// <summary>
        /// Creates a hidden, persistent GameObject with the FullscreenKeyListener attached.
        /// This allows the configured keys to work when the Game View has focus during Play Mode.
        /// </summary>
        private static void SpawnKeyListener()
        {
            var go = new GameObject("[FullscreenKeyListener]");
            go.AddComponent<FullscreenKeyListener>();
            go.hideFlags = HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(go);
        }

        #endregion
    }
}
