using DG.Tweening;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ForkCursor : CustomCursor
{
    [Header("Settings")]
    [SerializeField] float clickAnimPixelsLength;
    [SerializeField] float clickAnimTime;
    [SerializeField] Ease clickAnimEase;
    
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
        Vector2 targetPos = initialPos + Vector2.one * -clickAnimPixelsLength;
        
        currentImpaleSequence = DOTween.Sequence(); 
        currentImpaleSequence.Append(rectTransform.DOAnchorPos(targetPos, clickAnimTime)
            .SetEase(clickAnimEase));

        currentImpaleSequence.Append(rectTransform.DOAnchorPos(initialPos, clickAnimTime)
            .SetEase(clickAnimEase));

        currentImpaleSequence.SetUpdate(true);
        currentImpaleSequence.OnComplete(() => currentImpaleSequence = null);
        return currentImpaleSequence;
    }
}
