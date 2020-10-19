using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private InventorySystem playerInventory;

    //Get UI references
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI textAmount;
    public Transform itemPrefabPosition;
    private GameObject itemPrefab;

    [Tooltip("GameObject to toggle active when using Inventory UI")]
    public GameObject inventoryReference;

    void Start()
    {
        PlayerInventoryManager.UpdateInventoryEvent += UpdateInventoryUI;
        playerInventory = PlayerInventoryManager.instance.playerInventory;

        textName.text = "";
        textDescription.text = "";
        textAmount.text = "";
    }

    void UpdateInventoryUI()
    {
        if (PlayerInventoryManager.instance.inventoryVisible)
        {
            inventoryReference.SetActive(true);

            if(playerInventory.currentSlot < playerInventory.inventory.Count)
            {
                ItemSlot currentItem = playerInventory.inventory[playerInventory.currentSlot];

                if (currentItem.itemAmount != 0)
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
                    newInventoryPrefab.GetComponent<Rigidbody>().isKinematic = true;

                    itemPrefab = newInventoryPrefab;
                }
                else
                {
                    textName.text = "";
                    textDescription.text = "";
                    textAmount.text = "";

                    if (itemPrefab != null)
                        Destroy(itemPrefab);
                }
            }
        }
        else
        {
            inventoryReference.SetActive(false);
        }
    }

    void OnDestroy()
    {
        PlayerInventoryManager.UpdateInventoryEvent -= UpdateInventoryUI;
    }
}
