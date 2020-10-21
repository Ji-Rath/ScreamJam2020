using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public class HidingSpot : MonoBehaviour
{
    [Header("Hiding Place Config"), Space]
    public GameObject trigger;
    public GameObject enemyStandPoint;
    
    void Start()
    {
        //Listen for DoorInteraction
        GetComponent<Door>().OnInteractDoor += HideSpot_InteractDoor;

        //Disable trigger on start
        if (trigger)
            trigger.SetActive(false);
    }

    //Toggle trigger when the specified door is interacted with
    void HideSpot_InteractDoor(bool isOpen)
    {
        trigger.SetActive(!isOpen);

        GameManager.Get().playerRef.GetComponent<Player>().UpdateHideStatus(trigger);
    }

    void OnDestroy()
    {
        //Stop listening for event
        GetComponent<Door>().OnInteractDoor -= HideSpot_InteractDoor;
    }
}
