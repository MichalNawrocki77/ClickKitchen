using InteractiveKitchen;
using InteractiveKitchen.Cursors;
using UnityEngine;

public abstract class CustomCursor : MonoBehaviour
{
    protected CursorManager owner;
    protected GlobalGameInput input;
    public void Inject(CursorManager owner, GlobalGameInput input)
    {
        this.owner = owner;
        this.input = input;
    }

    public abstract void OnSelected();
    public abstract void OnDeselected();
}
