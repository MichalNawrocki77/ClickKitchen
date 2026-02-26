using UnityEngine;
using UnityEngine.Events;

namespace InteractiveKitchen.InteractionSystem
{
    
    public interface IContinousInteractionDetector
    {
        public UnityEvent OnInteractionStart { get; }
        public UnityEvent OnInteractionTick { get; }
        public UnityEvent OnInteractionEnd { get; }
    }

    public interface IContinousInteractionDetector<T>
    {
        public UnityEvent<T> OnInteractionStart { get; }
        public UnityEvent<T> OnInteractionTick { get; }
        public UnityEvent<T> OnInteractionEnd { get; }
    }

    public interface IContinousInteractionDetector<T1,T2>
    {
        public UnityEvent<T1,T2> OnInteractionStart { get; }
        public UnityEvent<T1,T2> OnInteractionTick { get; }
        public UnityEvent<T1,T2> OnInteractionEnd { get; }
    }

    public interface IContinousInteractionDetector<T1,T2,T3>
    {
        public UnityEvent<T1,T2,T3> OnInteractionStart { get; }
        public UnityEvent<T1,T2,T3> OnInteractionTick { get; }
        public UnityEvent<T1,T2,T3> OnInteractionEnd { get; }
    }

    public interface IContinousInteractionDetector<T1,T2,T3,T4>
    {
        public UnityEvent<T1,T2,T3,T4> OnInteractionStart { get; }
        public UnityEvent<T1,T2,T3,T4> OnInteractionTick { get; }
        public UnityEvent<T1,T2,T3,T4> OnInteractionEnd { get; }

    }
}