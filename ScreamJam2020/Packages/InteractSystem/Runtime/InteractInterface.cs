using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiRath.InteractSystem.Interactables
{
    //[RequireComponent(typeof(Collider))]
    public interface InteractInterface
    {
        /// <summary>
        /// Called whenever something interacts with the interactable
        /// </summary>
        void OnInteract(GameObject Interactor);
    }
}