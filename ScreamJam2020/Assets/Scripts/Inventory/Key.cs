using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickupable
{
    public override void OnUse()
    {
        GameObject itemInView = GameManager.Get().playerRef.GetComponent<PlayerInteraction>().GetItemInView();
        KeyDoor keyDoor = itemInView.GetComponent<KeyDoor>();
        if (keyDoor)
        {
            if (keyDoor.UnlockKey(item))
            {
                playerInventory.RemoveFromInventory(item, 1);
                Destroy(gameObject);
                Debug.Log("Key used on door!");
            }
            else
            {
                Debug.Log("Could not use key, wrong door?");
            }
        }
    }
}
