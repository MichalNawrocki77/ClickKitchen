using UnityEngine;

namespace InteractiveKitchen.InteractionSystem
{
    public interface IContinouslyInteractableObject
    {
        IContinousInteractionDetector detector { get; }   
    }    

    public interface IContinouslyInteractableObject<T>
    {
        IContinousInteractionDetector<T> detector { get; }   
    }

    public interface IContinouslyInteractableObject<T1,T2>
    {
        IContinousInteractionDetector<T1,T2> detector { get; }   
    }

    public interface IContinouslyInteractableObject<T1,T2,T3>
    {
        IContinousInteractionDetector<T1,T2,T3> detector { get; }   
    }

    public interface IContinouslyInteractableObject<T1,T2,T3,T4>
    {
        IContinousInteractionDetector<T1,T2,T3,T4> detector { get; }   
    }    
}
