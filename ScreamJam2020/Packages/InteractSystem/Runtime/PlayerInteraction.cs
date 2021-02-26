
using System;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;
//using UnityStandardAssets.CrossPlatformInput;

namespace JiRath.InteractSystem.PlayerInteract
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
            /*
            else if (equipSystem && equipSystem.currentEquippedItem)
            {
                Pickupable pickupable = equipSystem.currentEquippedItem.GetComponent<Pickupable>();
                if (pickupable)
                {
                    //Using item that the player has equipped
                    if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    {
                        pickupable.OnUse();
                        UseItem?.Invoke(pickupable);
                    }
                    else if (CrossPlatformInputManager.GetButtonDown("DropItem"))
                        equipSystem.DropItem(pickupable.item);
                }
            }
            */

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

        public GameObject GetItemInView()
        {
            return itemInView;
        }
    }
}