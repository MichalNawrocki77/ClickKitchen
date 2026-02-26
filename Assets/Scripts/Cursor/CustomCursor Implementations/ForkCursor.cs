using DG.Tweening;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace InteractiveKitchen.Cursors
{
    
    public class ForkCursor : CustomCursor
    {
        [Inject] CustomCursorsSettings settings;
        
        Sequence currentImpaleSequence;
        RectTransform rectTransform;
        void Awake()
        {
            rectTransform = transform as RectTransform;
        }

        public override void OnSelected()
        {
            input.UI.Click.performed += OnClick;
        }

        public override void OnDeselected()
        {
            input.UI.Click.performed -= OnClick;
        }
        void OnClick(CallbackContext ctx)
        {
            DOImpale();
        }

        Sequence DOImpale()
        {
            if(currentImpaleSequence != null)
                currentImpaleSequence.Complete(true);


            Vector2 initialPos = rectTransform.anchoredPosition;
            //Current position + (-1,-1) * clickAnimPixelsLength;
            Vector2 targetPos = initialPos + Vector2.one * -settings.forkAnimPixelsLength;
            
            currentImpaleSequence = DOTween.Sequence(); 
            currentImpaleSequence.Append(rectTransform.DOAnchorPos(targetPos, settings.forkAnimTime)
                .SetEase(settings.forkAnimEase));

            currentImpaleSequence.Append(rectTransform.DOAnchorPos(initialPos, settings.forkAnimTime)
                .SetEase(settings.forkAnimEase));

            currentImpaleSequence.SetUpdate(true);
            currentImpaleSequence.OnComplete(() => currentImpaleSequence = null);
            return currentImpaleSequence;
        }
    }
}
