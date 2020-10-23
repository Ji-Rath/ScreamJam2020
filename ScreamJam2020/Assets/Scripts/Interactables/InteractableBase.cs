using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableBase : MonoBehaviour
{
    public bool notShowButtonPrompt;
    public string interactMessage;
    public abstract void OnInteract();
}
