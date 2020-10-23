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
            //Update interact UI text
            Interactable interactable = itemInView.GetComponent<Interactable>();
            Pickupable pickupable = itemInView.GetComponent<Pickupable>();

            if (pickupable)
                textInteractable.text = "Press E to pickup "+pickupable.item.name;
            else if (interactable)
            {
                string interactName = interactable.name;
                
                if (interactable.interactableName != "") //Display interactable name
                    interactName = interactable.interactableName;
                else if (interactable.hoverMessage != "") //Display hover message
                    interactName = interactable.hoverMessage;

                textInteractable.text = interactName;

                if (interactable as IItemUsable != null && playerInteract.GetComponent<EquipSystem>().currentEquippedItem) //Display 'Use [item] on [itemhover]'
                    textInteractable.text = "Use " + playerInteract.GetComponent<EquipSystem>().currentEquippedItem.item.name + " on " + interactName;
            }
                

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
