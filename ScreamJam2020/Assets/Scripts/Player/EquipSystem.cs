using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSystem : MonoBehaviour
{
    public delegate void OnPlayerEquipAction();
    public static OnPlayerEquipAction OnPlayerDropItem;

    [HideInInspector]
    public Pickupable currentEquippedItem = null;
    public Transform itemDisplay;
    public AudioClip dropSound;
    private AudioSource audioSource;

    public float throwForce = 10f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void EquipItem(GameObject item)
    {
        //Destroy currently equipped item
        if (currentEquippedItem != null)
            Destroy(currentEquippedItem.gameObject);
        //Create new item
        
        currentEquippedItem = Instantiate(item, itemDisplay).GetComponent<Pickupable>();
        currentEquippedItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        currentEquippedItem.GetComponent<Collider>().enabled = false;
        currentEquippedItem.GetComponent<Rigidbody>().isKinematic = true;

        //Make sure it follows the player properly
        ItemSway itemSway = GetComponent<ItemSway>();
        if(itemSway)
        {
            itemSway.TargetItem = currentEquippedItem.gameObject;
            itemSway.targetLocation = itemDisplay.transform;
        }
    }

    //Drop the selected item
    public void DropItem(ItemBase item)
    {
        GetComponent<InventoryManager>().RemoveFromInventory(item, 1);

        if (currentEquippedItem != null)
            Destroy(currentEquippedItem.gameObject);

        if(audioSource)
        {
            audioSource.clip = dropSound;
            audioSource.Play();
        }
        

        GameObject droppedItem = Instantiate(item.itemModel, gameObject.transform.position, Quaternion.identity);

        //Add velocity and angular velocity to thrown object
        Rigidbody itemBody = droppedItem.GetComponent<Rigidbody>();
        itemBody.velocity = Camera.main.transform.forward * throwForce;
        itemBody.angularVelocity = new Vector3(throwForce, throwForce, throwForce);

        //Cause enemy to appear
        if(OnPlayerDropItem != null)
        {
            OnPlayerDropItem();
        }

        DialogueBox.Get().SetText("I think the enemy heard that..");
        DialogueBox.Get().TriggerText(2);
    }
}
