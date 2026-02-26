using UnityEngine;

namespace InteractiveKitchen.InteractionSystem
{
    public interface IInteractableObject
    {
        IInteractionDetector Detector { get; }
    }
    
    public interface IInteractableObject<T>
    {
        IInteractionDetector<T> Detector { get; }
    }

    public interface IInteractableObject<T1,T2>
    {
        IInteractionDetector<T1,T2> Detector { get; }
    }

    public interface IInteractableObject<T1,T2,T3>
    {
        IInteractionDetector<T1,T2,T3> Detector { get; }
    }

    public interface IInteractableObject<T1,T2,T3,T4>
    {
        IInteractionDetector<T1,T2,T3,T4> Detector { get; }
    }
}
