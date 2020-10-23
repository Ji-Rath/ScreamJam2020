using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : InteractableBase
{
    public string interactableName;


    void Awake()
    {
        if (interactableName != "")
            name = interactableName;
    }

    /// <summary>
    /// Called to change the state of the Interactable (on/off)
    /// </summary>
    /// <param name="activate"></param>
    public virtual void Activate(bool activate)
    {
        OnInteract();
    }
}
