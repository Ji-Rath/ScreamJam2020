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
        protected bool bActivated = false;

        void Awake()
        {
            if (interactableName != "")
                name = interactableName;
        }

        /// <summary>
        /// Called to change the state of the Interactable (on/off)
        /// </summary>
        /// <param name="activate"></param>
        public virtual void Activate(bool bActivate)
        {
            if (bActivate != bActivated)
                OnInteract(gameObject);
        }

        public virtual void OnInteract(GameObject Interactor)
        {
            if (CanInteract(Interactor))
                bActivated = !bActivated;
        }

        public abstract bool CanInteract(GameObject Interactor);
    }
}