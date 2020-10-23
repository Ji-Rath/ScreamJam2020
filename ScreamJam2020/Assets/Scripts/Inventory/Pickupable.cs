using System;
using UnityEngine;

/// <summary>
/// Class that handles objects that can be picked up by the player and optionally used
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Pickupable : InteractableBase
{
    [Tooltip("The item that will be given to the player when interacted")]
    public ItemBase item;
    public bool destroyOnUse = true;
    [Range(0,100)]
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

    void Start()
    {
        //Get player inventory
        GameObject playerRef = GameManager.Get().playerRef;
        playerInventory = playerRef.GetComponent<InventoryManager>();
        playerInteract = playerRef.GetComponent<PlayerInteraction>();

        audioSource = GameManager.Get().playerRef.GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
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
    public bool OnUse()
    {
        //If the item could be used, play sound
        if(!UseOnInteractactable() && Use())
        {
            //Play use sound
            if (audioSource)
            {
                audioSource.clip = useSound;
                audioSource.Play();
            }

            //Destroy item if specified
            if(destroyOnUse)
            {
                playerInventory.RemoveFromInventory(item, 1);
                Destroy(gameObject);
            }

            //Show used message
            UpdateUseText();
            DialogueBox.Get().TriggerText(onUseMessage);

            return true;
        }
        return false;
    }

    /// <summary>
    /// Update useMessage with appropriate text related to the item, can probably be put in a static class later on
    /// </summary>
    void UpdateUseText()
    {
        GameObject itemInView = playerInteract.GetItemInView();
        onUseMessage = onUseMessage.Replace("{item}", item.name);
        if(itemInView)
            onUseMessage = onUseMessage.Replace("{itemInView}", itemInView.name);
    }

    /// <summary>
    /// Called to see if the current pickupable can be used on whats being hovered by the player
    /// </summary>
    /// <returns></returns>
    private bool UseOnInteractactable()
    {
        //Check if the player is hovering over an ItemPlacement object and determine whether the object is able to be used by it
        GameObject itemInView = playerInteract.GetItemInView();
        if (itemInView)
        {
            ItemPlacement itemPlacement = itemInView.GetComponent<ItemPlacement>();
            if (itemPlacement && itemPlacement.OnItemUse(item))
            {
                playerInventory.RemoveFromInventory(item, 1);
                Destroy(gameObject);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Functionality of using the item
    /// </summary>
    /// <returns>Whether the item could be used.</returns>
    public virtual bool Use()
    {
        return false;
    }
}
