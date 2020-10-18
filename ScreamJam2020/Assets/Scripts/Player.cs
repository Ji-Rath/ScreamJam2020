using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    
    public bool isHiding;
    public GameObject currentHidingPlace;
    

    // Start is called before the first frame update
    void Start()
    {
        Door.OnTriggerDisabled += DeactivateHiding;
        
    }

     // Update is called once per frame
     void Update()
     {
        
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

    

    private void DeactivateHiding(GameObject trigger)
    {
        if(isHiding)
        {
            if(trigger == currentHidingPlace)
            {
                isHiding = false;
                currentHidingPlace = null;
            }
        }
    }

    private void OnDestroy()
    {
        Door.OnTriggerDisabled -= DeactivateHiding;
    }
}
