using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleComponent : Interactable
{
    public List<Behaviour> toggleComponent = new List<Behaviour>();

    public override void OnInteract()
    {
        foreach (Behaviour component in toggleComponent)
            component.enabled = !component.enabled;
    }
}
