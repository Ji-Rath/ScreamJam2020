using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableBase
{
    public override void OnInteract()
    {
        Debug.Log("Pressed Button!");
    }
}
