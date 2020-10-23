using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacement : Interactable, IItemUsable
{
    [Tooltip("Correct item to be placed")]
    public ItemBase itemNeeded;
    private ItemBase placedItem = null;
    public Transform itemPosition;

    private GameObject itemPrefab;

    public event Action UseItem;

    public override void Activate(bool activate)
    {
        if (activate)
            placedItem = itemNeeded;
        else
            placedItem = null;
    }

    public bool OnItemUse(ItemBase item)
    {
        bool isSuccess = false;
        if(placedItem)
        {
            if (GameManager.Get().playerRef.GetComponent<InventoryManager>().AddToInventory(placedItem))
            {
                placedItem = item;
                isSuccess = true;
            }
        }
        else
        {
            placedItem = item;
            isSuccess = true;
        }

        //Create the selected inventory prefab and delete the old one
        GameObject newItemPrefab = Instantiate(placedItem.itemModel, itemPosition);
        Destroy(itemPrefab);

        //Add InventoryPrefab component for spin effect
        newItemPrefab.AddComponent<InventoryPrefab>();
        newItemPrefab.GetComponent<Rigidbody>().isKinematic = true;

        itemPrefab = newItemPrefab;

        itemPrefab.GetComponent<Pickupable>().PickupEvent += Item_PickupEvent;

        UseItem?.Invoke();
        return isSuccess;
    }

    /// <summary>
    /// Called from listener when placed item is picked up
    /// </summary>
    private void Item_PickupEvent()
    {
        placedItem = null;
        UseItem?.Invoke();
    }

    public override void OnInteract()
    {
        if (placedItem && GameManager.Get().playerRef.GetComponent<InventoryManager>().AddToInventory(placedItem))
        {
            Item_PickupEvent();
            UseItem?.Invoke();
        }
    }

    public bool IsCorrect()
    {
        return itemNeeded == placedItem;
    }

    void OnDestroy()
    {
        if (itemPrefab && itemPrefab.GetComponent<Pickupable>())
            itemPrefab.GetComponent<Pickupable>().PickupEvent -= Item_PickupEvent;
    }
}
