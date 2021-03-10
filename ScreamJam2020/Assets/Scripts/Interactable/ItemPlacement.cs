using System;
using UnityEngine;
using JiRath.InventorySystem.Usable;
using JiRath.InteractSystem;
using JiRath.InventorySystem;
using JiRath.InventorySystem.EquipSystem;

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

    public bool OnItemUse(GameObject user, ItemBase item)
    {
        bool isSuccess = false;

        //Create the selected inventory prefab and delete the old one
        GameObject newItemPrefab = Instantiate(item.itemModel, itemPosition);
        Destroy(itemPrefab);

        //Add InventoryPrefab component for spin effect
        newItemPrefab.AddComponent<InventoryPrefab>();
        newItemPrefab.GetComponent<Rigidbody>().isKinematic = true;

        itemPrefab = newItemPrefab;

        itemPrefab.GetComponent<Pickupable>().PickupEvent += Item_PickupEvent;
        user.GetComponent<InventoryManager>().RemoveFromInventory(item, 1);

        //Drop or equip item that was on the stand
        if (placedItem != null)
        {
            if (!user.GetComponent<InventoryManager>().AddToInventory(placedItem))
            {
                Instantiate(placedItem, transform);
            }
            else
            {
                user.GetComponent<EquipManager>().EquipItem(placedItem.itemModel);
            }
        }

        placedItem = item;
        UseItem?.Invoke();
        return isSuccess;
    }

    /// <summary>
    /// Called from listener when placed item is picked up
    /// </summary>
    private void Item_PickupEvent(GameObject Interactor)
    {
        itemPrefab = null;
        placedItem = null;
        UseItem?.Invoke();
    }

    public override void OnInteract(GameObject Interactor)
    {
        base.OnInteract(Interactor);
        if (CanInteract(Interactor))
        {
            itemPrefab.GetComponent<Pickupable>().OnInteract(Interactor);
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

    public override bool CanInteract(GameObject Interactor)
    {
        return placedItem && Interactor.GetComponent<InventoryManager>().CanAdd(placedItem);
    }
}
