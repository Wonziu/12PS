using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : Npc
{
    public List<QuestInformation> questDialogs;
    private Queue<QuestInformation> questDialogsQueue;

    private bool _givenQuest;

    private void Awake()
    {
        questDialogsQueue = new Queue<QuestInformation>();
    }

    private void Start()
    {
        foreach (var questDialog in questDialogs)
        {
            questDialogsQueue.Enqueue(questDialog);
        }

        questDialogs.Clear();
    }

    public override void Interact()
    {
        if (!_givenQuest && questDialogsQueue.Count > 0) 
        {
            questManager.ActivateQuest(questDialogsQueue.Peek().quest);
            dialogManager.StartDialog(questDialogsQueue.Peek().StartEventDialog, npcName);
            
            _givenQuest = true;
        }
        else if (!QuestCompleted())      
            base.Interact();

        else
        {
            questManager.DeactivateQuest();
            dialogManager.StartDialog(questDialogsQueue.Peek().CompleteEventDialog, npcName);

            _givenQuest = false;
            questDialogsQueue.Dequeue();
        }

    }

    private bool QuestCompleted()
    {
        return questManager.activeQuest != null && questManager.activeQuest.completed;
    }
}