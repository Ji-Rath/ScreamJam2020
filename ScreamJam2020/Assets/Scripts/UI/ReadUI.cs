
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ReadUI : UIBase
{
    public static ReadUI instance;

    public TextMeshProUGUI textRead;
    public Image imageRead;
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

        instance = this;

        interactAnimator = GetComponent<Animator>();

        playerInteraction = GameManager.Get().playerRef.GetComponent<PlayerInteraction>();
        if(playerInteraction)
        {
            playerInteraction.CancelAction += UI_CancelAction;
        }
    }

    public void UI_UseItem(Pickupable pickupable)
    {
        if (!isVisible)
        {
            //If item is not valid, return
            if (!pickupable) { Debug.LogError("Unable to access pickupable!"); return; }

            //If item is not readable, return
            ItemReadable readable = pickupable.item as ItemReadable;
            ItemReadableImage readableImage = pickupable.item as ItemReadableImage;
            if (readable)
            {
                DisablePlayer(true);
                textRead.enabled = true;
                imageRead.enabled = false;

                //Display text and make UI visible
                textRead.text = readable.content;

                isVisible = true;
                interactAnimator.SetBool("isVisible", isVisible);
            }
            else if (readableImage)
            {
                DisablePlayer(true);
                textRead.enabled = false;
                imageRead.enabled = true;

                //Display text and make UI visible
                imageRead.sprite = readableImage.image;

                isVisible = true;
                interactAnimator.SetBool("isVisible", isVisible);
            }
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
        playerInteraction.CancelAction -= UI_CancelAction;
    }
}
