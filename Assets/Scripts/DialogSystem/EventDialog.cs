using UnityEngine.Events;

[System.Serializable]
public class EventDialog
{
    public UnityEvent onDialogEnd;
    public DialogFile dialog;
    public bool invokeEventOnce;
    public bool wasEventInvoked;
    public int questId;
}