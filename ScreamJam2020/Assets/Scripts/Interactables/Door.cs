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

    [Header("Sound Config"),Space]
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private Animator animator;

    public delegate void DoorEvent(bool isOpen);
    public event DoorEvent OnInteractDoor;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                if(audioSource)
                {
                    audioSource.clip = openSound;
                    audioSource.Play();
                }
                
            }
            else
            {
                // Close
                animator.SetTrigger("Close");
                canInteract = false;
                if (audioSource)
                {
                    audioSource.clip = closeSound;
                    audioSource.Play();
                }
                
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
