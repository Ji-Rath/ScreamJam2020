using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public Camera playerCamera;
    public Camera pixelCamera;
    public bool isHiding;
    public GameObject currentHidingPlace;
    

    // Start is called before the first frame update
    void Start()
    {
        //pixelCamera.enabled = false;
        //playerCamera.enabled = false;
        //pixelCamera.enabled = true;
        //playerCamera.enabled = true;
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
