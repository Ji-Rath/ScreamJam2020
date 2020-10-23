using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyDoor : Door, IItemUsable
{
    [Header("Locked Door Config")]
    [Tooltip("A list of items that will unlock the door")]
    public List<ItemBase> keysNeeded = new List<ItemBase>();

    [Tooltip("Message to display when the door is locked"), TextArea]
    public string lockedMessage;

    public event Action UseItem;

    public bool OnItemUse(ItemBase item)
    {
        bool isCorrectKey = keysNeeded.Contains(item);

        //Remove key if it is correct
        if (isCorrectKey)
            keysNeeded.Remove(item);

        //Unlock door and play message if there are no more keys needed
        if (isLocked && IsCorrect())
            isLocked = false;

        UseItem?.Invoke();
        return isCorrectKey;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        //Display locked message
        if(!IsCorrect())
        {
            DialogueBox.Get().TriggerText(lockedMessage);
        }
    }

    public bool IsCorrect()
    {
        return keysNeeded.Count == 0;
    }
}
