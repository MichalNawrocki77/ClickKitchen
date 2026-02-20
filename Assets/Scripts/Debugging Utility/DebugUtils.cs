using UnityEngine;

namespace InteractiveKitchen.Debugging
{
    public static class DebugUtils
    {
        public static void Log(string msg)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log(msg);
            #endif
        }

        public static void LogWarning(string msg)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(msg);
            #endif
        }

        public static void LogError(string msg)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError(msg);
            #endif
        }
    }    
}
