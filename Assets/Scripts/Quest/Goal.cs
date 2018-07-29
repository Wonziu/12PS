using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : ScriptableObject
{
    public Quest quest;
    public string description;
    public bool isCompleted;
    public int currentAmount;
    public int requiredAmount;

    public virtual void Init()
    {
        isCompleted = false;
    }

    public void Evaluate()
    {
        if (currentAmount >= requiredAmount)
            Complete();
    }

    public void Complete()
    {
        isCompleted = true;
        quest.CheckGoals();
    }
}