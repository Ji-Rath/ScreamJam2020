using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractUI : MonoBehaviour
{
    public TextMeshProUGUI textInteractable;
    private Animator interactAnimator;

    private PlayerInteraction playerInteract;

    void Start()
    {
        playerInteract = GameManager.Get().playerRef.GetComponent<PlayerInteraction>();
        playerInteract.InteractHover += UI_InteractHover;

        interactAnimator = GetComponent<Animator>();
    }

    void UI_InteractHover()
    {
        if (playerInteract.itemInView)
        {
            InteractableBase item = playerInteract.itemInView.GetComponent<InteractableBase>();

            interactAnimator.SetTrigger("FadeIn");

            if (item)
                textInteractable.text = item.name;
        }
        else
        {
            interactAnimator.SetTrigger("FadeOut");
            Debug.Log("FadeOut");
        }

        
    }

    void OnDestroy()
    {
        playerInteract.InteractHover -= UI_InteractHover;
    }
}
