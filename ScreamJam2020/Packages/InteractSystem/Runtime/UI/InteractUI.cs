
using System.Collections;
using UnityEngine;
using TMPro;

namespace JiRath.InteractSystem.UI
{
    [RequireComponent(typeof(Animator))]
    public class InteractUI : UIBase
    {
        public TextMeshProUGUI textInteractable;
        private Animator interactAnimator;

        private bool isVisible = false;

        void Awake()
        {
            //Allow interact UI to overlap with other UI
            enableUIOverlap = true;
        }

        void Start()
        {
            interactAnimator = GetComponent<Animator>();
        }

        void UI_InteractHover()
        {
            //Make sure player is hovering over an item
            Interactable itemInView = owningPlayer.GetComponent<PlayerInteraction>().GetItemInView();
            if (itemInView)
            {
                textInteractable.text = "";

                //Update interact UI text
                Interactable interactable = itemInView.GetComponent<Interactable>();

                if (interactable && GetItemInViewName() != "")
                {
                    string interactName = GetItemInViewName();
                    textInteractable.text = interactName;

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
            Interactable itemInView = owningPlayer.GetComponent<PlayerInteraction>().GetItemInView();
            if (itemInView)
            {
                if (itemInView.interactableName != "") //Display interactable name
                    interactName = itemInView.interactableName;
                else if (itemInView.hoverMessage != "") //Display hover message
                    interactName = itemInView.hoverMessage;
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
            if (owningPlayer)
                owningPlayer.GetComponent<PlayerInteraction>().InteractHover -= UI_InteractHover;
        }

        public override void Bind(GameObject owner)
        {
            base.Bind(owner);
            owner.GetComponent<PlayerInteraction>().InteractHover += UI_InteractHover;
        }

        public override bool IsEnabled()
        {
            return isVisible;
        }
    }
}