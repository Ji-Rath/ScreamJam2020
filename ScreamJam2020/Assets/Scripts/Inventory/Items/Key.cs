using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickupable
{
    public override bool Use()
    {
        GameObject itemInView = GameManager.Get().playerRef.GetComponent<PlayerInteraction>().GetItemInView();
        if(itemInView)
        {
            KeyDoor keyDoor = itemInView.GetComponent<KeyDoor>();
            if (keyDoor)
            {
                return keyDoor.UnlockKey(item);
            }
        }
        return false;
    }
}

