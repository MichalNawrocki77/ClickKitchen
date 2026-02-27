using InteractiveKitchen.Cursors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

namespace InteractiveKitchen.InteractionSystem
{
    public class MouseDragDetector : MonoBehaviour, 
        IContinousInteractionDetector<PointerEventData, CustomCursor>,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler
    {
        [field: SerializeField] public UnityEvent<PointerEventData, CustomCursor> OnInteractionStart { get; private set; }

        [field: SerializeField] public UnityEvent<PointerEventData, CustomCursor> OnInteractionTick { get; private set; }

        [field: SerializeField] public UnityEvent<PointerEventData, CustomCursor> OnInteractionEnd { get; private set; }

        [Inject] CursorManager cursorManager;

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnInteractionStart?.Invoke(eventData, cursorManager.CurrentCursor);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnInteractionTick?.Invoke(eventData, cursorManager.CurrentCursor);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnInteractionEnd?.Invoke(eventData, cursorManager.CurrentCursor);
        }
    }
}
