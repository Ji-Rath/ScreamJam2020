using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;

public class ToggleOpenDoor : Interactable
{
    public List<KeyDoor> toggleDoor = new List<KeyDoor>();

    public override void OnInteract(GameObject Interactor)
    {
        foreach (KeyDoor door in toggleDoor)
        {
            door.isLocked = false;
            door.InteractDoor();
        }
            
    }
}
