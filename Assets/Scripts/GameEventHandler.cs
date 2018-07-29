using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameEventHandler
{
    public delegate void ItemHandler(Item item);
    public static event ItemHandler OnItemPickedUp;

    public static void ItemPickedUp(Item item)
    {
        if (OnItemPickedUp != null)
            OnItemPickedUp.Invoke(item);
    }
}
