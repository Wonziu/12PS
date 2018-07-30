using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Interactable
{
    public string npcName;

    public QuestManager questManager;
    public DialogManager dialogManager;

    public EventDialog defaultEventDialog; // quest id is redundand
    public List<EventDialog> eventDialogs;
    public List<Dialog> dialogs;

    public override void Interact()
    {
        if (questManager.activeQuest != null)
        {
            var dialog = dialogs.Find(x => x.questId == questManager.activeQuest.id);

            if (dialog != null)
            {
                dialogManager.StartDialog(dialog, npcName);
                return;
            }

            var newDialog = eventDialogs.Find(x => x.questId == questManager.activeQuest.id);

            if (newDialog != null)
            {
                dialogManager.StartDialog(newDialog, npcName);
                return;
            }

        }

        dialogManager.StartDialog(defaultEventDialog, npcName);
    }
}