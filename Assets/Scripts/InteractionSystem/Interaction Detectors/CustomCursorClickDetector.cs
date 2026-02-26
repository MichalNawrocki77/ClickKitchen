using UnityEngine;
using UnityEngine.EventSystems;
using InteractiveKitchen.Cursors;
using UnityEngine.Events;
using Zenject;
using System;

namespace InteractiveKitchen.InteractionSystem
{
    public class CustomCursorClickDetector : MonoBehaviour,
                                             IInteractionDetector<PointerEventData, CustomCursor>, 
                                             IPointerClickHandler
    {
        [Inject] CursorManager cursorManager;

        [TypeFilter(typeof(CustomCursor))]
        [SerializeField] SerializableType allowedCursor;
        [field: SerializeField] public UnityEvent<PointerEventData, CustomCursor> OnInteractionDetected { get; private set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(cursorManager?.CurrentCursor.GetType() != allowedCursor.Type) return;

            OnInteractionDetected?.Invoke(eventData, cursorManager?.CurrentCursor);    
        }
    }
}
