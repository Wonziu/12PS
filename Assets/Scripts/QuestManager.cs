using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest activeQuest;

    public void Start()
    {
        activeQuest.Init();
    }

    public void ActivateQuest(Quest quest)
    {        
        activeQuest = quest;
    }
}   
