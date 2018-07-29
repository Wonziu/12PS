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

public class Npc : Interactable
{
    public QuestManager questManager;
    public DialogManager dialogManager;

    public List<Dialog> dialogs; // quest id and corresponding dialog, 0 for default dialog
    public string npcName;

    private void Start()
    {
        Interact();
    }

    public override void Interact()
    {
        if (dialogs.Count == 0)
            return;

        if (questManager.activeQuest != null)
        {
            var dialog = dialogs.Find(x => x.questId == questManager.activeQuest.id);
            
            if (!dialog.Equals(default(Dialog)))
            {
                dialogManager.StartDialog(dialog, npcName);              
                return;
            }
        }

        dialogManager.StartDialog(dialogs[0], npcName);
    }
}