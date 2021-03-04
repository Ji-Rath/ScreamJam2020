using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JiRath.InventorySystem.EquipSystem;

namespace JiRath.InventorySystem.UI
{
    public class InventoryUI : UIBase
    {
        InventoryManager playerInventory;

        //Get UI references
        public Button nextButton;
        public Button prevButton;
        public TMP_Text textName;
        public TMP_Text textDescription;
        public TMP_Text textAmount;
        public Transform itemPrefabPosition;
        public Color normalColor;
        public Color disabledColor;
        public float scalePreview = 1;
        private GameObject itemPrefab;

        [Tooltip("GameObject to toggle active when using Inventory UI")]
        public GameObject inventoryReference;

        int viewedSlot = 0;

        public override bool IsEnabled()
        {
            return InventoryManager.inventoryVisible;
        }

        void UpdateInventoryUI()
        {
            if (CanEnable())
            {
                DisablePlayer(InventoryManager.inventoryVisible);
                EmptyInventory();

                if (InventoryManager.inventoryVisible)
                {
                    inventoryReference.SetActive(true);
                    CheckButton(nextButton, prevButton);

                    //Only try displaying item if there are any contents
                    if (playerInventory.inventory.itemList.Count > 0)
                    {
                        InventoryItem currentItem = playerInventory.inventory.itemList[viewedSlot];

                        //Update text
                        textName.SetText(currentItem.item.name);
                        textDescription.SetText(currentItem.item.description);

                        //Do not display stack count if the max stack is 1
                        if (currentItem.item.maxStack > 1)
                            textAmount.text = "Amount: " + currentItem.itemAmount + " / " + currentItem.item.maxStack;
                        else
                            textAmount.text = "";

                        //Create the selected inventory prefab and delete the old one
                        GameObject newInventoryPrefab = Instantiate(currentItem.item.itemModel, itemPrefabPosition);
                        Destroy(itemPrefab);
                        newInventoryPrefab.transform.localScale = new Vector3(scalePreview, scalePreview, scalePreview);

                        //Add InventoryPrefab component for spin effect
                        newInventoryPrefab.AddComponent<InventoryPrefab>();
                        newInventoryPrefab.GetComponent<Rigidbody>().isKinematic = true;

                        itemPrefab = newInventoryPrefab;
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

        public void EquipCurrentItem()
        {
            if (playerInventory != null && playerInventory.inventory.itemList.Count > 0)
            {
                EquipManager equipSystem = owningPlayer.GetComponent<EquipManager>();
                equipSystem.EquipItem(playerInventory.inventory.itemList[viewedSlot].item.itemModel);
            }
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
            //MonsterAI.OnMonsterKillPlayer -= DisableInventoryUI;
        }

        public void GetNextItem()
        {
            IncrementSlot(1);
            playerInventory.GetItem(viewedSlot);
            UpdateInventoryUI();
        }
        public void GetPreviousItem()
        {
            IncrementSlot(-1);
            playerInventory.GetItem(viewedSlot);
            UpdateInventoryUI();
        }

        public int IncrementSlot(int amountChange)
        {
            viewedSlot += amountChange;
            int maxSlots = playerInventory.inventory.itemList.Count;

            if (viewedSlot >= maxSlots)
            {
                viewedSlot -= maxSlots;
            }
            else if (viewedSlot < 0)
            {
                viewedSlot = maxSlots - Mathf.Abs(viewedSlot);
            }

            return Mathf.Abs(viewedSlot);
        }

        public override void Bind(GameObject owner)
        {
            base.Bind(owner);

            playerInventory = owner.GetComponent<InventoryManager>();
            playerInventory.UpdateInventoryEvent += UpdateInventoryUI;
            //MonsterAI.OnMonsterKillPlayer += DisableInventoryUI;
        }
    }
}