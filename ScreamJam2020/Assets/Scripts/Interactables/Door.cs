using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : Interactable
{
    public bool disableCollider;
    public bool openOnce;
    [Header("Door Config"), Space]
    public bool isLocked;
    public bool isOpen;
    public bool canInteract = true;

    [Header("No Animation Config")]
    public GameObject openState;
    public GameObject closeState;

    [Header("Sound Config"),Space]
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;
    [HideInInspector]
    protected AudioSource audioSource;

    private Animator animator;

    public delegate void DoorEvent(bool isOpen);
    public event DoorEvent OnInteractDoor;

    // Start is called before the first frame update
    protected void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (animator)
        {
            if (isOpen)
            {
                animator.SetBool("StartOpen", true);
            }
            else
            {
                animator.SetBool("StartOpen", false);
            }
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
                    if(animator)
                    {
                        animator.SetTrigger("Open");
                    }
                    else
                    {
                        openState.SetActive(true);
                        closeState.SetActive(false);
                        if (disableCollider)
                        {
                            GetComponent<BoxCollider>().enabled = false;
                        }

                    }
                    
                    canInteract = false;
                    if (audioSource)
                    {
                        audioSource.clip = openSound;
                        audioSource.Play();
                    }
                    if(openOnce)
                    {
                        enabled = false;
                    }
                }
                else
                {
                    // Close
                    if (animator)
                    {
                        animator.SetTrigger("Close");
                    }
                    else
                    {
                        openState.SetActive(false);
                        closeState.SetActive(true);
                        if (disableCollider)
                        {
                            GetComponent<BoxCollider>().enabled = true;
                        }

                    }
                    
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
