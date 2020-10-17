using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteraction : MonoBehaviour
{
    [Range(1f, 50f)]
    public float interactRange = 10f;
    private GameObject itemInView;

    public Transform DesiredItemLocation;
    private GameObject CurrentEquippedItem;

    

    // Update is called once per frame
    void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Interact"))
        {
            if(itemInView != null)
            {
                itemInView.GetComponent<InteractableBase>().OnInteract();
            }
            else if (CurrentEquippedItem != null)
            {
                CurrentEquippedItem.GetComponent<Pickupable>().OnUse();
            }
        }
    }

    void FixedUpdate()
    {
        RaycastHit RayHit;
        if (Physics.Raycast(transform.position, Camera.main.transform.forward, out RayHit, interactRange))
        {
            GameObject HitObject = RayHit.collider.gameObject;
            if (HitObject.GetComponent<InteractableBase>())
            {
                itemInView = HitObject;
            }
            else if (itemInView != null)
            {
                itemInView = null;
            }
        }
        else if (itemInView != null)
        {
            itemInView = null;
        }
    }

    public void EquipItem(GameObject item)
    {
        if(CurrentEquippedItem != null)
            Destroy(CurrentEquippedItem);

        CurrentEquippedItem = Instantiate(item, DesiredItemLocation);
        CurrentEquippedItem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        CurrentEquippedItem.GetComponent<Collider>().enabled = false;

        GetComponent<ItemSway>().TargetItem = CurrentEquippedItem;
    }

    
}
