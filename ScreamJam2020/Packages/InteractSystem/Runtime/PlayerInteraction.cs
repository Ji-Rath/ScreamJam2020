
using System;
using UnityEngine;

namespace JiRath.InteractSystem
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Range(1f, 50f)]
        public float interactRange = 5f;
        [Tooltip("Mask for ray cast on interactable objects")]
        public LayerMask layerMask;
        private GameObject itemInView;
        public bool enableDebug = false;

        public event Action InteractHover;
        //public event Action<Pickupable> UseItem;
        public event Action CancelAction;

        // Update is called once per frame
        void Update()
        {
            //EquipSystem equipSystem = GetComponent<EquipSystem>();
            if (Input.GetButtonDown("Interact") && itemInView != null)
            {
                //Interacting with objects in the world
                itemInView.GetComponent<Interactable>().OnInteract(gameObject);
            }

            if (Input.GetButtonDown("Cancel"))
            {
                CancelAction?.Invoke();
            }
        }

        void FixedUpdate()
        {
            //Perform ray cast to see if there are any interactables in front of the player
            RaycastHit RayHit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RayHit, interactRange, layerMask))
            {
                GameObject HitObject = RayHit.collider.gameObject;
                if (HitObject.GetComponent<Interactable>())
                {
                    itemInView = HitObject;
                    InteractHover?.Invoke();
                }
                else
                {
                    itemInView = null;
                    InteractHover?.Invoke();
                }

                if (enableDebug)
                    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);
            }
            else if (!object.ReferenceEquals(itemInView, null)) //Hacky way to make sure that the itemInView is not null regardless of whether it is destroyed
            {
                itemInView = null;
                InteractHover?.Invoke();

                if (enableDebug)
                    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white);
            }
        }

        public Interactable GetItemInView()
        {
            if (itemInView)
                return itemInView.GetComponent<Interactable>();
            return null;
        }
    }
}