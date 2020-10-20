﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteraction : MonoBehaviour
{
    [Range(1f, 50f)]
    public float interactRange = 5f;
    [Tooltip("Mask for ray cast on interactable objects")]
    public LayerMask layerMask;
    
    private GameObject itemInView;
    
    private FirstPersonController fpsController;

    public bool enableDebug = false;

    public event Action InteractHover;

    private void Start()
    {
        fpsController = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fpsController.enabled)
        {
            EquipSystem equipSystem = GetComponent<EquipSystem>();
            if (CrossPlatformInputManager.GetButtonDown("Interact") && itemInView != null)
            {
                //Interacting with objects in the world
                itemInView.GetComponent<InteractableBase>().OnInteract();
            }
            else if (equipSystem && equipSystem.currentEquippedItem)
            {
                Pickupable pickupable = equipSystem.currentEquippedItem.GetComponent<Pickupable>();
                //Using item that the player has equipped
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    pickupable.OnUse();
                else if (CrossPlatformInputManager.GetButtonDown("DropItem"))
                    equipSystem.DropItem(pickupable.item);
            }
        }
    }

    void FixedUpdate()
    {
        //Perform ray cast to see if there are any interactables in front of the player
        RaycastHit RayHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RayHit, interactRange, layerMask))
        {
            GameObject HitObject = RayHit.collider.gameObject;
            if (HitObject.GetComponent<InteractableBase>())
            {
                itemInView = HitObject;
                InteractHover?.Invoke();
            }
                
            if (enableDebug)
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);
        }
        else if(!object.ReferenceEquals(itemInView, null)) //Hacky way to make sure that the itemInView is not null regardless of whether it is destroyed
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
