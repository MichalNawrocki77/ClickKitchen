using UnityEngine;
using UnityEngine.Events;

namespace InteractiveKitchen.InteractionSystem
{
    
    public interface IInteractionDetector
    {
        public UnityEvent OnInteractionDetected { get; }
    }

    public interface IInteractionDetector<T>
    {
        public UnityEvent<T> OnInteractionDetected { get; }
    }

    public interface IInteractionDetector<T1,T2>
    {
        public UnityEvent<T1,T2> OnInteractionDetected { get; }
    }

    public interface IInteractionDetector<T1,T2,T3>
    {
        public UnityEvent<T1,T2,T3> OnInteractionDetected { get; }
    }

    public interface IInteractionDetector<T1,T2,T3,T4>
    {
        public UnityEvent<T1,T2,T3,T4> OnInteractionDetected { get; }
    }
}
