using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleComponent : InteractableBase
{
    public List<Behaviour> toggleComponent = new List<Behaviour>();

    public override void OnInteract()
    {
        foreach (Behaviour component in toggleComponent)
            component.enabled = !component.enabled;
    }
}
