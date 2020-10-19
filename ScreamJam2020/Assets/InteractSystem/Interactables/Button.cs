using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableBase
{
    public GameObject light;
    public override void OnInteract()
    {
        Debug.Log("Pressed Button!");
        light.SetActive(true);
    }
}
