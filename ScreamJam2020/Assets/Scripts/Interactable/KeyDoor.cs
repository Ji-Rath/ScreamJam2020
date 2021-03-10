using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InventorySystem.Usable;
using JiRath.InventorySystem;
using JiRath.InventorySystem.EquipSystem;

[Serializable]
public class KeyDoor : Door, IItemUsable
{
    public delegate void OnKeyDoorAction();
    public OnKeyDoorAction OnKeyDoorUnlocked;


    [Header("Locked Door Config")]
    [Tooltip("A list of items that will unlock the door")]
    public List<ItemBase> keysNeeded = new List<ItemBase>();
    public bool allKeysInInventory;

    [Tooltip("Message to display when the door is locked"), TextArea]
    public string lockedMessage;

    public event Action UseItem;

    public bool OnItemUse(GameObject user, ItemBase item)
    {
        if (!allKeysInInventory)
        {
            bool isCorrectKey = keysNeeded.Contains(item);

            //Remove key if it is correct
            if (isCorrectKey)
            {
                user.GetComponent<InventoryManager>().RemoveFromInventory(item, 1);
                keysNeeded.Remove(item);
            }

            //Unlock door and play message if there are no more keys needed
            if (isLocked && keysNeeded.Count == 0)
            {
                isLocked = false;
                OnKeyDoorUnlocked?.Invoke();
                OnInteract(gameObject);
            }

            UseItem?.Invoke();
            return isCorrectKey;
        }
        else
        {
            List<int> correctKeysIndex = new List<int>();
            InventoryManager inventoryManager = GameManager.Get().playerRef.GetComponent<InventoryManager>();
            for (int i = 0; i < inventoryManager.inventory.maxSlots; i++)
            {
                bool isCorrectKey = keysNeeded.Contains(inventoryManager.inventory.itemList[i].item);
                if (isCorrectKey)
                {
                    correctKeysIndex.Add(i);
                }

            }

            if (correctKeysIndex.Count >= keysNeeded.Count)
            {
                List<ItemBase> itemsToDelete = new List<ItemBase>();

                for (int i = 0; i < correctKeysIndex.Count; i++)
                {
                    itemsToDelete.Add(inventoryManager.inventory.itemList[correctKeysIndex[i]].item);
                }

                for (int i = 0; i < itemsToDelete.Count; i++)
                {
                    inventoryManager.RemoveFromInventory(itemsToDelete[i], 1);
                }
                itemsToDelete.Clear();
                EquipManager equipSystem = GameManager.Get().playerRef.GetComponent<EquipManager>();
                Destroy(equipSystem.currentEquippedItem.gameObject);
                correctKeysIndex.Clear();

                isLocked = false;

                if (OnKeyDoorUnlocked != null)
                {
                    OnKeyDoorUnlocked();
                }

                return true;
            }
            else
            {
                correctKeysIndex.Clear();
                return false;
            }
        }

    }

    public override void OnInteract(GameObject Interactor)
    {
        base.OnInteract(Interactor);

        //Display locked message
        if (keysNeeded.Count > 0 && isLocked)
        {
            DialogueBox.Get().TriggerText(lockedMessage);
        }
    }

    public bool IsCorrect()
    {
        return keysNeeded.Count == 0;
    }
}