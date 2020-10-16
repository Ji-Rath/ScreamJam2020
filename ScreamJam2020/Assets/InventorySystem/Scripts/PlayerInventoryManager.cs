using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class PlayerInventoryManager : MonoBehaviour
{
    public InventorySystem playerInventory;
    public bool inventoryVisible = false;

    //Get UI references
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public GameObject inventoryPrefab;

    //Get player reference to manage cursor locking
    public GameObject playerReference;
    private GameObject inventoryReference;
    private FirstPersonController playerController;

    void Start()
    {
        //Get child gameObject to manage visibility of the inventory system
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

            UpdateInventory();
        }
    }

    //Update inventory information with the currently selected item
    void UpdateInventory()
    {
        if (inventoryVisible)
        {
            //Update text
            textName.text = playerInventory.inventory[playerInventory.currentSlot].item.name;
            textDescription.text = playerInventory.inventory[playerInventory.currentSlot].item.description;

            //Create the selected inventory prefab and delete the old one
            GameObject newInventoryPrefab = Instantiate(playerInventory.inventory[playerInventory.currentSlot].item.itemModel, inventoryPrefab.transform.position, Quaternion.identity);
            Destroy(inventoryPrefab);

            //Add InventoryPrefab component for spin effect
            newInventoryPrefab.AddComponent<InventoryPrefab>();

            inventoryPrefab = newInventoryPrefab;
        }
    }

    //Called when the player wants to view the next item in their inventory
    public void NextButton()
    {
        if (playerInventory.inventory.Length - 1 > playerInventory.currentSlot)
            playerInventory.currentSlot++;
        else
            playerInventory.currentSlot = 0;

        UpdateInventory();
    }

    //Called when the player wants to view the previous item in their inventory
    public void PreviousButton()
    {
        if (playerInventory.currentSlot > 0)
            playerInventory.currentSlot--;
        else
            playerInventory.currentSlot = playerInventory.inventory.Length - 1;

        UpdateInventory();
    }
}
