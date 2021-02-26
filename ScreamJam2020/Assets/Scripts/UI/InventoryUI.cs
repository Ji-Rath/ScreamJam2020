using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace JiRath.InventorySystem.UI
{
    public class InventoryUI : UIBase
    {
        public InventoryManager playerInventory;

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

            playerInventory.UpdateInventoryEvent += UpdateInventoryUI;
            MonsterAI.OnMonsterKillPlayer += DisableInventoryUI;

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
                    if (playerInventory.currentSlot < playerInventory.inventory.itemList.Count)
                    {
                        InventoryItem currentItem = playerInventory.inventory.itemList[playerInventory.currentSlot];

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
                    // Toggle visibility of Inventory UI when it should not be visible
                    inventoryReference.SetActive(false);
                }
            }

        }

        public void CheckButton(Button nextButton, Button prevButton)
        {
            if (playerInventory.currentSlot >= playerInventory.inventory.itemList.Count - 1)
            {
                nextButton.image.color = disabledColor;
            }
            else
            {
                nextButton.image.color = normalColor;
            }

            if (playerInventory.currentSlot <= 0)
            {
                prevButton.image.color = disabledColor;
            }
            else
            {
                prevButton.image.color = normalColor;
            }

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
            playerInventory.UpdateInventoryEvent -= UpdateInventoryUI;
            MonsterAI.OnMonsterKillPlayer -= DisableInventoryUI;
        }
    }
}
