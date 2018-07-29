using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Quest : ScriptableObject
{
    public int id;
    public string description;
    public bool completed;

    public List<Goal> goals = new List<Goal>();

    public void Init()
    {   
        foreach (var goal in goals)
        {
            goal.quest = this;            
            goal.Init();
        }
    }

    public void CheckGoals()
    {
        completed = goals.TrueForAll(g => g.isCompleted);
    }
}