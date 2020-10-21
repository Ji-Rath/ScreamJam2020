using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : InteractableBase
{
    [Tooltip("The item that will be given to the player when interacted")]
    public ItemBase item;
    [Range(0,100)]
    public int enemySpawnChance;
    public AudioClip pickupSound;
    public AudioClip useSound;

    private AudioSource audioSource;
    protected InventoryManager playerInventory;

    void Start()
    {
        //Get player inventory
        playerInventory = GameManager.Get().playerRef.GetComponent<InventoryManager>();
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
            Destroy(gameObject);
        }
            
        else
            Debug.Log("Unable to add to inventory!");
    }

    public virtual void OnUse()
    {
        if (audioSource)
        {
            audioSource.clip = useSound;
            audioSource.Play();
        }
        
        playerInventory.RemoveFromInventory(item, 1);
        Destroy(gameObject);
        Debug.Log("Used Item");
    }
}
