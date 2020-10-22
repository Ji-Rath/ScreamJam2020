using UnityEngine;

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

    [Tooltip("Message displayed when item is successfully used"), TextArea]
    public string onUseMessage;

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

    public void OnUse()
    {
        //If the item could be used, play sound
        if(Use())
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
        }
    }

    //Update useMessage with appropriate text related to the item, can probably be put in a static class later on
    void UpdateUseText()
    {
        GameObject itemInView = playerInventory.GetComponent<PlayerInteraction>().GetItemInView();
        onUseMessage = onUseMessage.Replace("{item}", item.name);
        if(itemInView)
            onUseMessage = onUseMessage.Replace("{itemInView}", itemInView.name);
    }

    //Functionality of using item, returns whether the item could be used. Should be overridden
    public virtual bool Use()
    {
        return false;
    }
}
