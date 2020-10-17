using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : InteractableBase
{
    [Tooltip("The item that will be given to the player when interacted")]
    public ItemBase item;

    public override void OnInteract()
    {
        //If the item is pickupable, add it to inventory
        if (PlayerInventoryManager.instance.AddToInventory(item))
            Destroy(gameObject);
        else
            Debug.Log("Unable to add to inventory!");
    }

    public virtual void OnUse()
    {
        PlayerInventoryManager.instance.RemoveFromInventory(item, 1);
        Destroy(gameObject);
    }
}
