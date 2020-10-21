using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadUI : MonoBehaviour
{
    public TextMeshProUGUI textRead;
    private Animator interactAnimator;

    private bool isVisible = false;

    private PlayerInteraction playerInteraction;

    void Start()
    {
        interactAnimator = GetComponent<Animator>();

        playerInteraction = GameManager.Get().playerRef.GetComponent<PlayerInteraction>();
        if(playerInteraction)
        {
            playerInteraction.UseItem += UI_UseItem;
            playerInteraction.CancelAction += UI_CancelAction;
        }
    }

    void UI_UseItem(Pickupable pickupable)
    {
        if (!isVisible)
        {
            //If item is not valid, return
            if (!pickupable) { Debug.LogError("Unable to access pickupable!"); return; }

            //Check if item used is readable
            Readable readable = (Readable)pickupable.item;

            //If item is not valid, return
            if (!readable) { Debug.LogError("Unable to access readable item!"); return; }

            //Display text and make UI visible
            textRead.text = readable.content;
        
            isVisible = true;
            interactAnimator.SetBool("isVisible", isVisible);
        }
    }

    void UI_CancelAction()
    {
        if (isVisible)
        {
            isVisible = false;
            interactAnimator.SetBool("isVisible", isVisible);
        }
    }

    void OnDestroy()
    {
        playerInteraction.UseItem -= UI_UseItem;
        playerInteraction.CancelAction -= UI_CancelAction;
    }
}
