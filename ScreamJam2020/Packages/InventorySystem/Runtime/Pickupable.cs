using System;
using UnityEngine;
using JiRath.InteractSystem;
using JiRath.InventorySystem.Usable;


namespace JiRath.InventorySystem
{
    /// <summary>
    /// Class that handles objects that can be picked up by the player and optionally used
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Pickupable : Interactable
    {
        [Tooltip("The item that will be given to the player when interacted")]
        public ItemBase item;
        public bool destroyOnUse = true;
        [Range(0, 100)]
        public int enemySpawnChance;

        [Header("Sound Config"), Space]
        public AudioClip pickupSound;
        public AudioClip useSound;

        private AudioSource audioSource;
        protected InventoryManager playerInventory;
        protected PlayerInteraction playerInteract;

        [Tooltip("Message displayed when item is successfully used"), TextArea]
        public string onUseMessage;

        public event Action PickupEvent;

        public override void OnInteract(GameObject Interactor)
        {
            GameObject playerRef = Interactor;
            playerInventory = playerRef.GetComponent<InventoryManager>();
            playerInteract = playerRef.GetComponent<PlayerInteraction>();
            audioSource = playerRef.GetComponent<AudioSource>();

            //If the item is pickupable, add it to inventory
            if (playerInventory.AddToInventory(item))
            {
                if (audioSource)
                {
                    audioSource.clip = pickupSound;
                    audioSource.Play();
                }

                PickupEvent?.Invoke();
                Destroy(gameObject);
            }
            else
                Debug.Log("Unable to add to inventory!");
        }

        /// <summary>
        /// Called when the item will be used
        /// </summary>
        public bool OnUse(GameObject user, Interactable itemInView)
        {
            playerInventory = user.GetComponent<InventoryManager>();
            playerInteract = user.GetComponent<PlayerInteraction>();

            //If the item could be used, play sound
            if (Use(user, itemInView))
            {
                //Play use sound
                audioSource?.PlayOneShot(useSound);

                //Destroy item if specified
                if (destroyOnUse)
                {
                    playerInventory.RemoveFromInventory(item, 1);
                    Destroy(gameObject);
                }

                //Show used message
                UpdateUseText();
                //DialogueBox.Get().TriggerText(onUseMessage);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Update useMessage with appropriate text related to the item, can probably be put in a static class later on
        /// </summary>
        void UpdateUseText()
        {
            Interactable itemInView = playerInteract.GetItemInView();
            onUseMessage = onUseMessage.Replace("{item}", item.name);
            if (itemInView)
                onUseMessage = onUseMessage.Replace("{itemInView}", itemInView.name);
        }

        /// <summary>
        /// Functionality of using an item on a IItemUsable component
        /// </summary>
        /// <returns>Whether the item could be used.</returns>
        public virtual bool Use(GameObject user, Interactable itemInView)
        {
            if (itemInView)
            {
                IItemUsable itemUsable = itemInView.GetComponent<IItemUsable>();
                if (itemUsable != null)
                    return itemUsable.OnItemUse(item);
            }
            return false;
        }
    }
}
