using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;

public class ToggleOpenDoor : Interactable
{
    public List<KeyDoor> toggleDoor = new List<KeyDoor>();

    public override void OnInteract(GameObject Interactor)
    {
        base.OnInteract(Interactor);
        foreach (KeyDoor door in toggleDoor)
        {
            door.isLocked = false;
            door.OnInteract(Interactor);
        }
            
    }

    public override bool CanInteract(GameObject Interactor)
    {
        return true;
    }
}
