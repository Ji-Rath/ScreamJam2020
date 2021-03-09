
using System;
using UnityEngine;

namespace JiRath.InteractSystem
{
    public class InteractManager : MonoBehaviour
    {
        [Tooltip("Range for interacting with objects in the world")]
        [Range(1f, 50f)]
        public float interactRange = 5f;
        [Tooltip("Mask for ray cast on interactable objects")]
        public LayerMask layerMask;
        public bool enableDebug = false;

        public event Action<Interactable> OnHoverUpdate;
        public event Action OnCancelAction;

        [SerializeField]
        private Camera camComp;
        private Interactable itemInView;

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
                OnCancelAction?.Invoke();
            }
        }

        private void UpdateItemInView()
        {
            if (enableDebug)
                Debug.DrawRay(camComp.transform.position, camComp.transform.forward, Color.green);

            //Perform ray cast to see if there are any interactables in front of the player
            RaycastHit RayHit;
            bool rayHit = Physics.Raycast(camComp.transform.position, camComp.transform.forward, out RayHit, interactRange, layerMask);
            if (rayHit)
            {
                GameObject HitObject = RayHit.collider.gameObject;
                if (HitObject != itemInView)
                {
                    //Update interactable in view if valid and return
                    Interactable interactable = HitObject.GetComponent<Interactable>();
                    if (interactable)
                    {
                        itemInView = interactable;
                    }
                    else if (itemInView != null)
                    {
                        itemInView = null;
                    }
                    OnHoverUpdate?.Invoke(itemInView);
                }
            }
            else if (itemInView)
            {
                //Clear the last item in view
                itemInView = null;
                OnHoverUpdate?.Invoke(itemInView);
            }
        }

        void FixedUpdate()
        {
            UpdateItemInView();

        }

        public Interactable GetItemInView()
        {
            if (itemInView)
                return itemInView;
            return null;
        }
    }
}