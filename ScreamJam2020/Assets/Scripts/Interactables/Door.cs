using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractableBase
{
    public bool isLocked;
    public bool isOpen;
    public bool canInteract = true;

    private Animator animator;

    public delegate void DoorEvent(bool isOpen);
    public event DoorEvent OnInteractDoor;

    // Start is called before the first frame update
    void Start()
    {
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
        if (canInteract && !isLocked)
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

            OnInteractDoor?.Invoke(isOpen);
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
}
