using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    public TextMeshProUGUI textInteractable;
    private Animator interactAnimator;

    private PlayerInteraction playerInteract;

    private bool isVisible = false;

    void Start()
    {
        //Listen for InteractHover event
        playerInteract = GameManager.Get().playerRef.GetComponent<PlayerInteraction>();
        playerInteract.InteractHover += UI_InteractHover;

        interactAnimator = GetComponent<Animator>();
    }

    void UI_InteractHover()
    {
        //Make sure player is hovering over an item
        GameObject itemInView = playerInteract.GetItemInView();
        if (itemInView)
        {
            //Update interact UI text
            InteractableBase interactable = itemInView.GetComponent<InteractableBase>();
            Pickupable pickupable = itemInView.GetComponent<Pickupable>();

            if (pickupable)
                textInteractable.text = pickupable.interactMessage;
            else if (interactable)
                textInteractable.text = interactable.interactMessage;

            //Make text visible if it is not already
            if (!isVisible)
            {
                isVisible = true;
                interactAnimator.SetBool("isVisible", isVisible);
            }
        }
        else if (isVisible)
        {
            //Delay fade out to make sure text does not blink while quickly switching targets
            isVisible = false;
            StartCoroutine(DelayedFadeOut());
        }
    }

    IEnumerator DelayedFadeOut()
    {
        yield return new WaitForSeconds(0.25f);

        if (!playerInteract.GetItemInView())
            interactAnimator.SetBool("isVisible", isVisible);
        else
            isVisible = true;
    }

    void OnDestroy()
    {
        playerInteract.InteractHover -= UI_InteractHover;
    }
}
