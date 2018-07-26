using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Dialog 
{
    public UnityEvent onDialogEnd;
    public DialogFile dialog;
    public bool invokeEventOnce;
    public bool wasEventInvoked;
    public int questId;    
}

public class Npc : MonoBehaviour
{   
    public DialogManager dialogManager;
    public List<Dialog> dialogs;

    private void Start()
    {        
        dialogManager.StartDialog(dialogs[0], "Grzegorz");
    }
}
