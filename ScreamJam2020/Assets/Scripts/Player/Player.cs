using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem.UI;
using JiRath.InventorySystem;
using UnityStandardAssets.Characters.FirstPerson;
using System;


public class Player : MonoBehaviour
{
    public bool isHiding;
    public GameObject currentHidingPlace;

    public GameObject PauseUI;
    public event Action<bool> OnTogglePause;
    public bool isPaused;
    

    // Start is called before the first frame update
    void Start()
    {
        PauseUI = Instantiate(PauseUI);
        PauseUI.GetComponent<UIBase>().Bind(gameObject);

        var inventory = GetComponent<InventoryManager>();
        if (inventory)
            inventory.OnToggleInventory += DisablePlayerMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "HidingPlace")
        {
            currentHidingPlace = other.gameObject;
            isHiding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HidingPlace")
        {
            isHiding = false;
        }
    }

    void OnDestroy()
    {
        var inventory = GetComponent<InventoryManager>();
        if (inventory)
            inventory.OnToggleInventory -= DisablePlayerMovement;

        OnTogglePause -= DisablePlayerMovement;
    }
    
    //Update hiding status of player
    public void UpdateHideStatus(GameObject trigger)
    {
        if(trigger == currentHidingPlace)
        {
            //Update hide status and current spot depending on whether the player is actually 'hidden'
            isHiding = trigger.activeSelf;
            currentHidingPlace = isHiding ? null : currentHidingPlace;
        }
    }

    public void DisablePlayerMovement(bool disabled)
    {
        GetComponent<FirstPersonController>().enabled = !disabled;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            OnTogglePause?.Invoke(isPaused);
        }
    }
}
