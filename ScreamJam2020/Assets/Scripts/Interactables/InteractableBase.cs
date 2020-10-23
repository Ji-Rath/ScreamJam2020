using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableBase : MonoBehaviour
{
    
    
    /// <summary>
    /// Called whenever something interacts with the interactable
    /// </summary>
    public abstract void OnInteract();
}
