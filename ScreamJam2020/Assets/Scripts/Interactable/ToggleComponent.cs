﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;

public class ToggleComponent : Interactable
{
    public List<Behaviour> toggleComponent = new List<Behaviour>();

    public override void OnInteract(GameObject Interactor)
    {
        foreach (Behaviour component in toggleComponent)
            component.enabled = !component.enabled;
    }
}