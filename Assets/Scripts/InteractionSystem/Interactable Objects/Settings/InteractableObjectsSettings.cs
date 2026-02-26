using DG.Tweening;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ClickKitchen/Settings/Interactable Objects Settings")]
public class InteractableObjectsSettings : ScriptableObject
{
    [Header("Global Settings")]
    public int InteractableLayer;

    [Header("Fridge")]
    public Vector3 FridgeAnimTargetScale;
    public float FridgeScaleUpTime;
    public float FridgeScaleAnimInterval;
    public Ease FridgeScaleUpEase;
    public float FridgeScaleDownTime;
    public Ease FridgeScaleDownEase;
    public float FridgeDoorOpenRotation;
    public float FridgeDoorOpenTime;
    public Ease FridgeDoorOpenEase;
    public float FridgeDoorCloseTime;
    public Ease FridgeDoorCloseEase;
}
