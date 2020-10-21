using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : Door
{
    [Tooltip("A list of items that will unlock the door")]
    public List<ItemBase> keysNeeded = new List<ItemBase>();

    public bool UnlockKey(ItemBase item)
    {
        bool isCorrectKey = keysNeeded.Contains(item);

        if (isCorrectKey)
            keysNeeded.Remove(item);

        if (keysNeeded.Count == 0)
            isLocked = false;

        return isCorrectKey;
    }
}
