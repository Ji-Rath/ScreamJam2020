using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

using System;

public class InventoryManager : MonoBehaviour
{
    public delegate void OnInventoryAction();
    public static OnInventoryAction OnInventoryFullyEmptySlot;
    public InventorySystem currentInventory;

    [HideInInspector]
    public static bool inventoryVisible = false;

    //Get player reference to manage cursor locking
    private FirstPersonController playerController;

    //Event called to update the inventory UI
    public event Action UpdateInventoryEvent;

    void Start()
    {
        //Get child gameObject to manage visibility of the inventory system
        playerController = GetComponent<FirstPersonController>();
        MonsterAI.OnMonsterKillPlayer += DisableInventory;
    }

    void Update()
    {
        //Check if inventory key was pressed (Should be moved to player controller to prevent random references)
        if (CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            inventoryVisible = !inventoryVisible;
            currentInventory.currentSlot = 0;

            UpdateInventory();
        }
    }

    private void DisableInventory()
    {
        enabled = false;
    }

    //Update inventory information with the currently selected item
    void UpdateInventory()
    {
        UpdateInventoryEvent?.Invoke();
    }

    //Called when the player wants to view the next item in their inventory
    public void ViewNextItem()
    {
        if(currentInventory.currentSlot < currentInventory.inventory.Count - 1)
        {
            if (currentInventory.inventory[currentInventory.currentSlot + 1].item != null)
                currentInventory.currentSlot++;
            else
                currentInventory.currentSlot = 0;

            UpdateInventory();
        }
    }

    //Called when the player wants to view the previous item in their inventory
    public void ViewPreviousItem()
    {
        if(currentInventory.currentSlot != 0)
        {
            if (currentInventory.inventory[currentInventory.currentSlot - 1].item != null)
                currentInventory.currentSlot--;
            else
                currentInventory.currentSlot = currentInventory.inventory.Count - 1;
            UpdateInventory();
        }
    }

    //Button for equipping the item
    public void EquipSelectedItem()
    {
        if(currentInventory.inventory.Count > 0)
        {
            GetComponent<EquipSystem>().EquipItem(currentInventory.inventory[currentInventory.currentSlot].item.itemModel);
        }
        
    }

    //Add specified item to the players inventory if possible
    public bool AddToInventory(ItemBase item, int amount = 1)
    {
        for (int i = 0; i < currentInventory.inventory.Count; i++)
        {
            ItemSlot itemSlot = currentInventory.inventory[i];
            ItemBase itemTest = itemSlot.item;

            //Test whether the player already has the item in their inventory and if there is enough room
            if (itemTest.Equals(item) && (itemSlot.itemAmount + amount) <= itemTest.maxStack)
            {
                AddToStack(i, amount);
                return true;
            }
        }

        //If there is an empty slot available, use that instead
        if (currentInventory.inventory.Count < currentInventory.maxSlots)
        {
            currentInventory.inventory.Add(new ItemSlot(item, amount));
            return true;
        }

        //Unable to add item to inventory
        return false;
    }

    //Remove specified item/amount from inventory
    public bool RemoveFromInventory(ItemBase item, int amount)
    {
        for (int i = 0; i < currentInventory.inventory.Count; i++)
        {
            //When the item is found, remove the set amount
            if(currentInventory.inventory[i].item == item)
            {
                ItemSlot itemSlot = currentInventory.inventory[i];
                itemSlot.itemAmount -= amount;
                currentInventory.inventory[i] = itemSlot;

                if (itemSlot.itemAmount <= 0)
                {
                    currentInventory.inventory.RemoveAt(i);
                    if(OnInventoryFullyEmptySlot != null)
                    {
                        OnInventoryFullyEmptySlot();
                    }
                }

                return true;
            }
        }

        return false;
    }

    //Add specified item to the inventory (assuming it was possible)
    void AddToStack(int slotIndex, int amount)
    {
        ItemSlot itemSlot = currentInventory.inventory[slotIndex];
        itemSlot.itemAmount += amount;

        currentInventory.inventory[slotIndex] = itemSlot;
    }

    public void OnDestroy()
    {
        //Clean Scriptable Object
        currentInventory.inventory.Clear();
        MonsterAI.OnMonsterKillPlayer -= DisableInventory;
    }
}
