using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : InteractableBase
{
    public string interactableName;
    void Awake()
    {
        if(interactableName != "")
            name = interactableName;
    }
}
