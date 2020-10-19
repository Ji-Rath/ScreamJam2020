using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : InteractableBase
{
    [Tooltip("The item that will be given to the player when interacted")]
    public ItemBase item;

    public override void OnInteract()
    {
        //If the item is pickupable, add it to inventory
        if (InventoryManager.instance.AddToInventory(item))
            Destroy(gameObject);
        else
            Debug.Log("Unable to add to inventory!");
    }

    public virtual void OnUse()
    {
        InventoryManager.instance.RemoveFromInventory(item, 1);
        Destroy(gameObject);
        Debug.Log("Used Item");
    }
}
