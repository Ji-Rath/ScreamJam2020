using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

using System;

public class InventoryManager : MonoBehaviour
{

    // Event called when there is an update to the inventory
    public event Action UpdateInventoryEvent;

    // ScriptableObject that contains inventory items/info
    public InventorySystem inventory;

    [HideInInspector]
    public static bool inventoryVisible = false;

    [HideInInspector]
    public int currentSlot = 0;

    void Update()
    {
        //Check if inventory key was pressed
        if (CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            ToggleInventory();
        }
    }

    // Toggle visiblity of inventory
    private void ToggleInventory()
    {
        inventoryVisible = !inventoryVisible;
        currentSlot = 0;

        UpdateInventoryEvent?.Invoke();
    }

    //Called when the player wants to view the next item in their inventory
    public void ViewNextItem()
    {
        if(currentSlot < inventory.itemList.Count - 1)
        {
            if (inventory.itemList[currentSlot+1].item != null)
            {
                currentSlot++;
            }
            else
            {
                currentSlot = 0;
            }
            UpdateInventoryEvent?.Invoke();
        }
    }

    //Called when the player wants to view the previous item in their inventory
    public void ViewPreviousItem()
    {
        if(currentSlot != 0)
        {
            if (inventory.itemList[currentSlot - 1].item != null)
            {
                currentSlot--;
            }
            else
            {
                currentSlot = inventory.itemList.Count - 1;
            }
            UpdateInventoryEvent?.Invoke();
        }
    }

    //Button for equipping the item
    public void EquipSelectedItem()
    {
        if(inventory.itemList.Count > 0)
        {
            GetComponent<EquipSystem>().EquipItem(inventory.itemList[currentSlot].item.itemModel);
        }
    }

    //Add specified item to the players inventory if possible
    public bool AddToInventory(ItemBase item, int amount = 1)
    {
        // Cycle through all inventory slots
        for (int i = 0; i < inventory.itemList.Count; i++)
        {
            InventoryItem itemSlot = inventory.itemList[i];
            ItemBase itemTest = itemSlot.item;

            // If the player has the item already in their inventory, attempt to add to stack
            if (itemTest.Equals(item) && (itemSlot.itemAmount + amount) <= itemTest.maxStack)
            {
                AddToStack(i, amount);
                return true;
            }
        }

        //If there is an empty slot available, use that instead
        if (inventory.itemList.Count < inventory.maxSlots)
        {
            inventory.itemList.Add(new InventoryItem(item, amount));
            return true;
        }

        //Unable to add item to inventory
        return false;
    }

    //Remove specified item/amount from inventory
    public bool RemoveFromInventory(ItemBase item, int amount)
    {
        for (int i = 0; i < inventory.itemList.Count; i++)
        {
            //When the item is found, remove the set amount
            if(inventory.itemList[i].item == item)
            {
                InventoryItem itemSlot = inventory.itemList[i];
                itemSlot.itemAmount -= amount;
                inventory.itemList[i] = itemSlot;

                if (itemSlot.itemAmount <= 0)
                {
                    inventory.itemList.RemoveAt(i);
                }

                return true;
            }
        }

        Debug.Log("Unable to remove item!");
        return false;
    }

    //Add specified item to the inventory (assuming it was possible)
    public void AddToStack(int slotIndex, int amount)
    {
        InventoryItem itemSlot = inventory.itemList[slotIndex];
        itemSlot.itemAmount += amount;

        inventory.itemList[slotIndex] = itemSlot;
    }

    public void OnDestroy()
    {
        //Clean Scriptable Object
        inventory.itemList.Clear();
    }
}
