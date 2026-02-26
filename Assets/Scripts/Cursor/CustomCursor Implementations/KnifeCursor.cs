using System;
using DG.Tweening;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace InteractiveKitchen.Cursors
{
    
    public class KnifeCursor : CustomCursor
    {
        [Inject] CustomCursorsSettings settings;
        Sequence currentSlashSequence;
        
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
            DOSlash();
        }

        Sequence DOSlash()
        {
            if(currentSlashSequence != null)
                currentSlashSequence.Complete(true);

            Vector3 initialRotation = transform.localRotation.eulerAngles;
            Vector3 targetRotation = initialRotation + Vector3.forward * settings.knifeSlashRotationDegrees;

            currentSlashSequence = DOTween.Sequence();
            currentSlashSequence.Append(transform.DOLocalRotate(targetRotation, settings.knifeSlashInDuration)
                .SetEase(settings.knifeSlashInEase));

            currentSlashSequence.Append(transform.DOLocalRotate(initialRotation, settings.knifeSlashOutDuration)
                .SetEase(settings.knifeSlashOutEase));

            currentSlashSequence.SetUpdate(true);
            currentSlashSequence.OnComplete(() => currentSlashSequence = null);
            return currentSlashSequence;
        }
    }
}
