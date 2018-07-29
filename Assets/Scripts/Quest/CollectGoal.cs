using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollectGoal : Goal
{
    public int itemId;

    public override void Init()
    {
        base.Init();
        GameEventHandler.OnItemPickedUp += ItemPickedUp;
    }

    private void ItemPickedUp(Item item)
    {
        if (item.id != itemId)
            return;

        currentAmount++;
        Evaluate();
    }
}

