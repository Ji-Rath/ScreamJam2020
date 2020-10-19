using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

using System;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager instance;

    public InventorySystem playerInventory;

    [HideInInspector]
    public bool inventoryVisible = false;

    //Get player reference to manage cursor locking
    private FirstPersonController playerController;

    //Event called to update the inventory UI
    public static event Action UpdateInventoryEvent;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Get child gameObject to manage visibility of the inventory system
        playerController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        //Check if inventory key was pressed (Should be moved to player controller to prevent random references)
        if (CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            inventoryVisible = !inventoryVisible;
            playerInventory.currentSlot = 0;

            playerController.m_MouseLook.SetCursorLock(!inventoryVisible);
            playerController.enabled = !inventoryVisible;

            UpdateInventory();
        }
    }

    //Update inventory information with the currently selected item
    void UpdateInventory()
    {
        UpdateInventoryEvent?.Invoke();
    }

    //Called when the player wants to view the next item in their inventory
    public void ViewNextItem()
    {
        if(playerInventory.currentSlot < playerInventory.inventory.Count - 1)
        {
            if (playerInventory.inventory[playerInventory.currentSlot + 1].item != null)
                playerInventory.currentSlot++;
            else
                playerInventory.currentSlot = 0;

            UpdateInventory();
        }
    }

    //Called when the player wants to view the previous item in their inventory
    public void ViewPreviousItem()
    {
        if(playerInventory.currentSlot != 0)
        {
            if (playerInventory.inventory[playerInventory.currentSlot - 1].item != null)
                playerInventory.currentSlot--;
            else
                playerInventory.currentSlot = playerInventory.inventory.Count - 1;
            UpdateInventory();
        }
    }

    //Button for equipping the item
    public void EquipSelectedItem()
    {
        if(playerInventory.inventory.Count > 0)
        {
            GetComponent<PlayerInteraction>().EquipItem(playerInventory.inventory[playerInventory.currentSlot].item.itemModel);
        }
        
    }

    //Add specified item to the players inventory if possible
    public bool AddToInventory(ItemBase item, int amount = 1)
    {
        for (int i = 0; i < playerInventory.inventory.Count; i++)
        {
            ItemSlot itemSlot = playerInventory.inventory[i];
            ItemBase itemTest = itemSlot.item;

            //Test whether the player already has the item in their inventory and if there is enough room
            if (itemTest.Equals(item) && (itemSlot.itemAmount + amount) <= itemTest.maxStack)
            {
                AddToStack(i, amount);
                return true;
            }
        }

        //If there is an empty slot available, use that instead
        if (playerInventory.inventory.Count < playerInventory.maxSlots)
        {
            playerInventory.inventory.Add(new ItemSlot(item, amount));
            return true;
        }

        //Unable to add item to inventory
        return false;
    }

    //Remove specified item/amount from inventory
    public bool RemoveFromInventory(ItemBase item, int amount)
    {
        for (int i = 0; i < playerInventory.inventory.Count; i++)
        {
            //When the item is found, remove the set amount
            if(playerInventory.inventory[i].item == item)
            {
                ItemSlot itemSlot = playerInventory.inventory[i];
                itemSlot.itemAmount -= amount;
                playerInventory.inventory[i] = itemSlot;

                if (itemSlot.itemAmount <= 0)
                {
                    playerInventory.inventory.RemoveAt(i);
                }

                return true;
            }
        }

        return false;
    }

    //Add specified item to the inventory (assuming it was possible)
    void AddToStack(int slotIndex, int amount)
    {
        ItemSlot itemSlot = playerInventory.inventory[slotIndex];
        itemSlot.itemAmount += amount;

        playerInventory.inventory[slotIndex] = itemSlot;
    }

    public void OnDestroy()
    {
        //Clean Scriptable Object
        playerInventory.inventory.Clear();
    }
}
