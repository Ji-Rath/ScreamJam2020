using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteraction : MonoBehaviour
{
    [Range(1f, 50f)]
    public float interactRange = 5f;
    public float throwForce = 10f;
    public LayerMask layerMask;
    [HideInInspector]
    public GameObject itemInView;

    public Transform DesiredItemLocation;
    private GameObject currentEquippedItem;
    private FirstPersonController fpsController;

    public bool enableDebug = false;

    private void Start()
    {
        fpsController = gameObject.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fpsController.enabled)
        {
            if (CrossPlatformInputManager.GetButtonDown("Interact") && itemInView != null)
            {
                //Interacting with objects in the world
                itemInView.GetComponent<InteractableBase>().OnInteract();
            }
            else if (currentEquippedItem != null)
            {
                Pickupable pickupable = currentEquippedItem.GetComponent<Pickupable>();
                //Using item that the player has equipped
                if (CrossPlatformInputManager.GetButtonDown("Fire1"))
                    pickupable.OnUse();
                else if (CrossPlatformInputManager.GetButtonDown("DropItem"))
                    DropItem(pickupable.item, true);
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit RayHit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RayHit, interactRange, layerMask))
        {
            GameObject HitObject = RayHit.collider.gameObject;
            if (HitObject.GetComponent<InteractableBase>())
                itemInView = HitObject;

            if (enableDebug)
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);
        }
        else if (itemInView != null)
        {
            itemInView = null;

            if (enableDebug)
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white);
        }
        else
        {
            if (enableDebug)
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white);
        }
    }

    public void EquipItem(GameObject item)
    {
        //Destroy currently equipped item
        if(currentEquippedItem != null)
            Destroy(currentEquippedItem);

        //Create new item
        currentEquippedItem = Instantiate(item, DesiredItemLocation);
        currentEquippedItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        currentEquippedItem.GetComponent<Collider>().enabled = false;
        currentEquippedItem.GetComponent<Rigidbody>().isKinematic = true;

        //Make sure it follows the player properly
        GetComponent<ItemSway>().TargetItem = currentEquippedItem;
        GetComponent<ItemSway>().targetLocation = DesiredItemLocation;

        EventSystem.instance.TriggerNearbyEvent();
    }

    //Unequip the currently equipped item
    public void UnequipItem()
    {
        if (currentEquippedItem != null)
            Destroy(currentEquippedItem);
    }

    //Drop the selected item
    void DropItem(ItemBase item, bool currentlyEquipped = false)
    {
        PlayerInventoryManager.instance.RemoveFromInventory(item, 1);
        if (currentlyEquipped)
            UnequipItem();
            
        GameObject droppedItem = Instantiate(item.itemModel, gameObject.transform.position, Quaternion.identity);

        //Add velocity and angular velocity to thrown object
        Rigidbody itemBody = droppedItem.GetComponent<Rigidbody>();
        itemBody.velocity = Camera.main.transform.forward * throwForce;
        itemBody.angularVelocity = new Vector3(throwForce, throwForce, throwForce);

        GameManager.Get().enemyRef.GetComponent<MonsterAI>().CreateStimulus();
    }
}
