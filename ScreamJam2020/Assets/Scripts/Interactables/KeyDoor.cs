using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : Door
{
    [Header("Locked Door Config")]
    [Tooltip("A list of items that will unlock the door")]
    public List<ItemBase> keysNeeded = new List<ItemBase>();

    [Tooltip("Message to display when the door is locked"), TextArea]
    public string lockedMessage;

    public bool UnlockKey(ItemBase item)
    {
        bool isCorrectKey = keysNeeded.Contains(item);

        //Remove key if it is correct
        if (isCorrectKey)
            keysNeeded.Remove(item);

        //Unlock door and play message if there are no more keys needed
        if (isLocked && keysNeeded.Count == 0)
        {
            isLocked = false;
        }

        return isCorrectKey;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        //Display locked message
        if(keysNeeded.Count > 0)
        {
            DialogueBox.Get().TriggerText(lockedMessage);
        }
    }
}
