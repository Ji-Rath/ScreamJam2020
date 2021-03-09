using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem.UI;
using JiRath.InventorySystem;
using System;
using JiRath.InventorySystem.EquipSystem;


public class Player : MonoBehaviour
{
    public bool isHiding;
    public GameObject currentHidingPlace;

    public GameObject PauseUI;

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
}
