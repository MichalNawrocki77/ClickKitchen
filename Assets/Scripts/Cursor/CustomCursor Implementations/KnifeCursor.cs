using System;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class KnifeCursor : CustomCursor
{
    [Header("Settings")]
    [SerializeField] float slashRotationDegrees;
    [SerializeField] float slashInDuration;
    [SerializeField] Ease slashInEase;
    [SerializeField] float slashOutDuration;
    [SerializeField] Ease slashOutEase;

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
        Vector3 targetRotation = initialRotation + Vector3.forward * slashRotationDegrees;

        currentSlashSequence = DOTween.Sequence();
        currentSlashSequence.Append(transform.DOLocalRotate(targetRotation, slashInDuration)
            .SetEase(slashInEase));

        currentSlashSequence.Append(transform.DOLocalRotate(initialRotation, slashOutDuration)
            .SetEase(slashOutEase));

        currentSlashSequence.SetUpdate(true);
        currentSlashSequence.OnComplete(() => currentSlashSequence = null);
        return currentSlashSequence;
    }
}
