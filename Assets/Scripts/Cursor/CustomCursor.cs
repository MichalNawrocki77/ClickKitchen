using InteractiveKitchen;
using InteractiveKitchen.Cursors;
using UnityEngine;
using Zenject;

public abstract class CustomCursor : MonoBehaviour
{
    [Inject] protected CursorManager owner;
    [Inject] protected GlobalGameInput input;

    public abstract void OnSelected();
    public abstract void OnDeselected();
}
