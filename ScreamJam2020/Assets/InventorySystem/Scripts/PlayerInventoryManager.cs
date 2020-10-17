using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;
using System.Linq;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager instance;

    public InventorySystem playerInventory;
    public bool inventoryVisible = false;

    //Get UI references
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textAmount;
    public Transform itemPrefabPosition;
    private GameObject itemPrefab;

    //Get player reference to manage cursor locking
    private GameObject playerReference;
    private GameObject inventoryReference;
    private FirstPersonController playerController;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Get child gameObject to manage visibility of the inventory system
        playerReference = GameManager.Get().playerRef;
        inventoryReference = transform.GetChild(0).gameObject;
        playerController = playerReference.GetComponent<FirstPersonController>();
    }

    void Update()
    {
        //Check if inventory key was pressed (Should be moved to player controller to prevent random references)
        if (CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            inventoryVisible = !inventoryVisible;
            inventoryReference.SetActive(inventoryVisible);
            playerController.m_MouseLook.SetCursorLock(!inventoryVisible);
            playerInventory.currentSlot = 0;
            playerController.enabled = !inventoryVisible;

            UpdateInventory();
        }
    }

    //Update inventory information with the currently selected item
    void UpdateInventory()
    {
        if(playerInventory.currentSlot < playerInventory.inventory.Count)
        {
            ItemSlot currentItem = playerInventory.inventory[playerInventory.currentSlot];

            if (currentItem.itemAmount != 0 && inventoryVisible)
            {
                //Update text
                textName.text = currentItem.item.name;
                textDescription.text = currentItem.item.description;
                textAmount.text = currentItem.itemAmount + " / " + currentItem.item.maxStack;

                //Create the selected inventory prefab and delete the old one
                GameObject newInventoryPrefab = Instantiate(currentItem.item.itemModel, itemPrefabPosition);
                Destroy(itemPrefab);

                //Add InventoryPrefab component for spin effect
                newInventoryPrefab.AddComponent<InventoryPrefab>();

                itemPrefab = newInventoryPrefab;
            }
            else
            {
                textName.text = "";
                textDescription.text = "";
                textAmount.text = "";
                Destroy(itemPrefab);
            }
        }
        else
        {
            //You can either enable the controller or unpause the game
            playerController.enabled = true;
        }
    }

    //Called when the player wants to view the next item in their inventory
    public void NextButton()
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
    public void PreviousButton()
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
    public void EquipButton()
    {
        playerReference.GetComponent<PlayerInteraction>().EquipItem(playerInventory.inventory[playerInventory.currentSlot].item.itemModel);
    }

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
}
