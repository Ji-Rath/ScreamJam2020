using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickupable
{
    public bool instantlyOpen;

    public override bool Use()
    {
        GameObject itemInView = GameManager.Get().playerRef.GetComponent<PlayerInteraction>().GetItemInView();
        if(itemInView)
        {
            KeyDoor keyDoor = itemInView.GetComponent<KeyDoor>();
            if (keyDoor)
            {
                bool couldUnlock = keyDoor.OnItemUse(item);
                if(instantlyOpen)
                {
                    keyDoor.InteractDoor();
                }
                return couldUnlock;
            }
        }
        return false;
    }
}

