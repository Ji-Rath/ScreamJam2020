using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOpenDoor : Interactable
{
    public List<KeyDoor> toggleDoor = new List<KeyDoor>();

    public override void OnInteract()
    {
        foreach (KeyDoor door in toggleDoor)
        {
            door.isLocked = false;
            door.InteractDoor();
        }
            
    }
}
