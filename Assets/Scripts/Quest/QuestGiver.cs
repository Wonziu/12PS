using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct QuestConfig
{
    public Quest quest;
    public DialogFile dialogOnQuestEnd;
}

public class QuestGiver : Npc
{
    public QuestManager questManager;
    public List<Quest> quests;

    private bool givenQuest;

    public override void Interact()
    {
        if (!QuestCompleted())
        {
            base.Interact();
        }
        else
        {

        }
    }

    public bool QuestCompleted()
    {
        return questManager.activeQuest != null && questManager.activeQuest.completed;
    }
}