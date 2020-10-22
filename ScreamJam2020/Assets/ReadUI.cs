
using TMPro;
using UnityEngine;

public class ReadUI : UIBase
{
    public TextMeshProUGUI textRead;
    private Animator interactAnimator;

    private bool isVisible = false;

    private PlayerInteraction playerInteraction;

    public override bool IsEnabled()
    {
        //Make sure transition animation isnt playing while determining visibility
        return isVisible || interactAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Transition");
    }

    protected override void Start()
    {
        base.Start();

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

            DisablePlayer(true);

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
            DisablePlayer(false);
        }
    }

    void OnDestroy()
    {
        playerInteraction.UseItem -= UI_UseItem;
        playerInteraction.CancelAction -= UI_CancelAction;
    }
}
