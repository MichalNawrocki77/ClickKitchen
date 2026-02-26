using DG.Tweening;
using InteractiveKitchen.Cursors;
using InteractiveKitchen.Debugging;
using InteractiveKitchen.InteractionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class InteractableFridge : MonoBehaviour, IInteractableObject<PointerEventData, CustomCursor>
{
    IInteractionDetector<PointerEventData, CustomCursor> IInteractableObject<PointerEventData, CustomCursor>.Detector => detector;
    IInteractionDetector<PointerEventData, CustomCursor> detector;


    [SerializeField] Transform Door;

    [Inject] InteractableObjectsSettings settings;
    public bool IsClosed { get; private set; }
    Vector3 initialDoorRotation;
    Vector3 initialFridgeScale;
    Sequence currentOpenFridgeAnimSequence;
    Sequence currentCloseFridgeAnimSequence;
    void Awake()
    {
        detector = GetComponent<IInteractionDetector<PointerEventData, CustomCursor>>();
        
        if(Door == null)
            DebugUtils.LogError("Fridge does not have door reference!");

    }
    void Start()
    {
        initialFridgeScale = transform.localScale;

        if(Door != null)
            initialDoorRotation = Door.localEulerAngles;

        IsClosed = true;
    }

    void OnEnable()
    {
        detector.OnInteractionDetected.AddListener(HandleClick);
    }
    void OnDisable()
    {
        detector.OnInteractionDetected.RemoveListener(HandleClick);
    }

    void HandleClick(PointerEventData eventData, CustomCursor cursor)
    {
        if(currentOpenFridgeAnimSequence != null)
        {
            currentOpenFridgeAnimSequence.Complete(true);
            CloseFridge();
            return;
        }
        
        if(currentCloseFridgeAnimSequence != null)
        {
            currentCloseFridgeAnimSequence.Complete(true);
            OpenFridge();
            return;
        }

        DebugUtils.Log($"Door: {Door.transform.localEulerAngles}; Initial: {initialDoorRotation}; IsClosed: {IsClosed}");
        if(IsClosed)
            OpenFridge();
        else
            CloseFridge();
    }

    Sequence OpenFridge()
    {
        currentOpenFridgeAnimSequence = DOTween.Sequence();
        IsClosed = false;

        currentOpenFridgeAnimSequence.Append(
            transform.DOScale(settings.FridgeAnimTargetScale, settings.FridgeScaleUpTime))
            .SetEase(settings.FridgeScaleUpEase);
        
        currentOpenFridgeAnimSequence.AppendInterval(settings.FridgeScaleAnimInterval);

        currentOpenFridgeAnimSequence.Append(
            transform.DOScale(initialFridgeScale, settings.FridgeScaleDownTime)
            .SetEase(settings.FridgeScaleDownEase));
        
        currentOpenFridgeAnimSequence.Insert(0,
            Door.DOLocalRotate(Vector3.up * settings.FridgeDoorOpenRotation, settings.FridgeDoorOpenTime)
                .SetEase(settings.FridgeDoorOpenEase));

        currentOpenFridgeAnimSequence.OnComplete(() => currentOpenFridgeAnimSequence = null);
        return currentOpenFridgeAnimSequence;
    }
    Sequence CloseFridge()
    {
        currentCloseFridgeAnimSequence = DOTween.Sequence();
        
        currentCloseFridgeAnimSequence.Append(
            transform.DOScale(settings.FridgeAnimTargetScale, settings.FridgeScaleUpTime))
            .SetEase(settings.FridgeScaleUpEase);
        
        currentCloseFridgeAnimSequence.AppendInterval(settings.FridgeScaleAnimInterval);

        currentCloseFridgeAnimSequence.Append(
            transform.DOScale(initialFridgeScale, settings.FridgeScaleDownTime)
            .SetEase(settings.FridgeScaleDownEase));

        currentCloseFridgeAnimSequence.Insert(0,
            Door.DOLocalRotate(initialDoorRotation, settings.FridgeDoorCloseTime)
            .SetEase(settings.FridgeDoorCloseEase));
        
        currentCloseFridgeAnimSequence.OnComplete(() => 
        {
            currentCloseFridgeAnimSequence = null;
            IsClosed = true;
        });
        return currentCloseFridgeAnimSequence;
    }
}
