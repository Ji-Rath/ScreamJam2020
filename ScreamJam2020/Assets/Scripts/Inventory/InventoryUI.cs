using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryUI : UIBase
{
    private InventorySystem playerInventory;
    public InventoryManager inventoryManager;

    //Get UI references
    public Button nextButton;
    public Button prevButton;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textAmount;
    public Transform itemPrefabPosition;
    public Color normalColor;
    public Color disabledColor;
    public Vector3 scalePreview;
    private GameObject itemPrefab;

    [Tooltip("GameObject to toggle active when using Inventory UI")]
    public GameObject inventoryReference;

    public override bool IsEnabled()
    {
        return InventoryManager.inventoryVisible;
    }

    new void Start()
    {
        base.Start();

        inventoryManager.UpdateInventoryEvent += UpdateInventoryUI;
        InventoryManager.OnInventoryFullyEmptySlot += DeleteItem;
        MonsterAI.OnMonsterKillPlayer += DisableInventoryUI;

        playerInventory = inventoryManager.currentInventory;
        EmptyInventory();
    }

    void UpdateInventoryUI()
    {
        if (CanEnable())
        {
            DisablePlayer(InventoryManager.inventoryVisible);

            if (InventoryManager.inventoryVisible)
            {
                inventoryReference.SetActive(true);
                CheckButton(nextButton, prevButton);

                //Ensure the current slot is a valid slot
                if (playerInventory.currentSlot < playerInventory.inventory.Count)
                {
                    ItemSlot currentItem = playerInventory.inventory[playerInventory.currentSlot];

                    //Make sure the amount is not 0 for some reason
                    if (currentItem.itemAmount != 0)
                    {
                        //Update text
                        textName.text = currentItem.item.name;
                        textDescription.text = currentItem.item.description;

                        //Do not display stack count if the max stack is 1
                        if (currentItem.item.maxStack > 1)
                            textAmount.text = "Amount: " + currentItem.itemAmount + " / " + currentItem.item.maxStack;
                        else
                            textAmount.text = "";

                        //Create the selected inventory prefab and delete the old one
                        GameObject newInventoryPrefab = Instantiate(currentItem.item.itemModel, itemPrefabPosition);
                        Destroy(itemPrefab);
						newInventoryPrefab.transform.localScale = scalePreview;

                        //Add InventoryPrefab component for spin effect
                        newInventoryPrefab.AddComponent<InventoryPrefab>();
                        newInventoryPrefab.GetComponent<Rigidbody>().isKinematic = true;

                        itemPrefab = newInventoryPrefab;
                    }
                    else
                    {
                        EmptyInventory();
                    }
                }
                else
                {
                    EmptyInventory();
                }
            }
            else if (inventoryReference.activeSelf)
            {
                inventoryReference.SetActive(false);
            }
        }
        
    }

    public void CheckButton(Button nextButton,Button prevButton)
    {
        if(inventoryManager.currentInventory.currentSlot >= playerInventory.inventory.Count-1)
        {
            nextButton.image.color = disabledColor;
        }
        else
        {
            nextButton.image.color = normalColor;
        }

        if (inventoryManager.currentInventory.currentSlot <= 0)
        {
            prevButton.image.color = disabledColor;
        }
        else
        {
            prevButton.image.color = normalColor;
        }

    }

    public void DeleteItem()
    {
        textName.text = "";
        textDescription.text = "";
        textAmount.text = "";

        if (itemPrefab != null)
            Destroy(itemPrefab);
    }

    private void DisableInventoryUI()
    {
        gameObject.SetActive(false);
    }

    //Clear all item text from the inventory
    void EmptyInventory()
    {
        textName.text = "";
        textDescription.text = "";
        textAmount.text = "";

        if (itemPrefab != null)
            Destroy(itemPrefab);
    }

    void OnDestroy()
    {
        inventoryManager.UpdateInventoryEvent -= UpdateInventoryUI;
        InventoryManager.OnInventoryFullyEmptySlot -= DeleteItem;
        MonsterAI.OnMonsterKillPlayer -= DisableInventoryUI;
    }
}
