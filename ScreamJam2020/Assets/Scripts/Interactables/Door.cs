using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(AudioSource))]
public class Door : Interactable
{
    [Header("Door Config"), Space]
    public bool isLocked;
    public bool isOpen;
    public bool canInteract = true;

    [Header("Sound Config"),Space]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    private AudioSource audioSource;

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

    public override void OnInteract()
    {
        InteractDoor();
    }

    public void InteractDoor()
    {
        if (!isLocked)
        {
            if(canInteract)
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    // Open
                    animator.SetTrigger("Open");
                    canInteract = false;
                    if (audioSource)
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
        else
        {
            if (audioSource)
            {
                audioSource.clip = lockedSound;
                audioSource.Play();
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
}
