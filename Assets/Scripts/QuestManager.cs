using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Quest activeQuest;

    public void Start()
    {

    }

    public void ActivateQuest(Quest quest)
    {        
        activeQuest = quest;
        activeQuest.Init();
    }

    public void DeactivateQuest()
    {
        activeQuest = null;
    }

    public void OpenDoor()
    {
        Debug.Log("OPENED");
    }
}   
