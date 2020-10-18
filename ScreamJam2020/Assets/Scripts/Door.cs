using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableBase
{
    public delegate void OnDoorAction(GameObject trigger);
    public static OnDoorAction OnTriggerDisabled;
    
    public bool isLocked;
    public bool isOpen;
    public bool canInteract = true;

    [Header("Hiding Place Config"), Space]
    public bool isHidingPlace;
    public GameObject trigger;
    public GameObject enemyStandPoint;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if (isHidingPlace)
        {
            if (trigger)
            {
                trigger.SetActive(false);
            }
        }

        animator = GetComponent<Animator>();
        if(isOpen)
        {
            animator.SetBool("StartOpen", true);
        }
        else
        {
            animator.SetBool("StartOpen", false);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        //test
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            InteractDoor();
        }
    }*/

    public override void OnInteract()
    {
        InteractDoor();
    }

    public void InteractDoor()
    {
        if(canInteract)
        {
            if (!isLocked)
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    // Open
                    animator.SetTrigger("Open");
                    canInteract = false;
                   
                }
                else
                {
                    // Close
                    animator.SetTrigger("Close");
                    canInteract = false;
                    
                }
            }
        }
        
    }

    public void CanInteract()
    {
        canInteract = true;
    }

    public void CannotInteract()
    {
        canInteract = false;
    }

    public void ActivateTrigger()
    {
        if(isHidingPlace)
        {
            if(trigger)
            {
                trigger.SetActive(true);
            }
        }
    }

    public void DeactivateTrigger()
    {
        if (isHidingPlace)
        {
            if (trigger)
            {
                trigger.SetActive(false);
                if(OnTriggerDisabled != null)
                {
                    OnTriggerDisabled(trigger);
                }
            }
        }
    }
}
