using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
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
            textInteractable.text = "";

            //Update interact UI text
            Interactable interactable = itemInView.GetComponent<Interactable>();
            Pickupable pickupable = itemInView.GetComponent<Pickupable>();

            if (pickupable)
                textInteractable.text = "Press E to pickup "+pickupable.item.name;
            else if (interactable && GetItemInViewName() != "")
            {
                string interactName = GetItemInViewName();

                Pickupable currentlyEquipped = playerInteract.GetComponent<EquipSystem>().currentEquippedItem;
                IItemUsable itemUsable = interactable as IItemUsable;
                if ((itemUsable != null && !itemUsable.IsCorrect()) && currentlyEquipped) //Display 'Use [item] on [itemhover]'
                    textInteractable.text = "Use " + currentlyEquipped.item.name + " on " + interactName;
            }
            else
            {
                isVisible = false;
                StartCoroutine(DelayedFadeOut());
            }
                

            //Make text visible if it is not already
            if (!isVisible && textInteractable.text != "")
            {
                //Make text visible if it is not already
                if (!isVisible)
                {
                    isVisible = true;
                    interactAnimator.SetBool("isVisible", isVisible);
                }
            }
            
        }
        else if (isVisible)
        {
            //Delay fade out to make sure text does not blink while quickly switching targets
            isVisible = false;
            StartCoroutine(DelayedFadeOut());
        }
    }

    string GetItemInViewName()
    {
        string interactName = "";
        GameObject itemInView = playerInteract.GetItemInView();
        if(itemInView)
        {
            Interactable interactable = itemInView.GetComponent<Interactable>();
            if(interactable)
            {
                if (interactable.interactableName != "") //Display interactable name
                    interactName = interactable.interactableName;
                else if (interactable.hoverMessage != "") //Display hover message
                    interactName = interactable.hoverMessage;
            }
        }
        return interactName;
    }

    IEnumerator DelayedFadeOut()
    {
        yield return new WaitForSeconds(0.25f);

        if (GetItemInViewName() == "")
            interactAnimator.SetBool("isVisible", isVisible);
        else
            isVisible = true;
    }

    void OnDestroy()
    {
        playerInteract.InteractHover -= UI_InteractHover;
    }
}
