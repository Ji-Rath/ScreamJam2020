using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiRath.InteractSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        public string interactableName;
        public string hoverMessage;


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
            OnInteract(gameObject);
        }

        public abstract void OnInteract(GameObject Interactor);
    }
}