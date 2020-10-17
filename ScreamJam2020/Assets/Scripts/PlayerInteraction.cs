using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerInteraction : MonoBehaviour
{
    [Range(1f, 50f)]
    public float interactRange = 10f;
    public LayerMask layerMask;
    public GameObject itemInView;

    public Transform DesiredItemLocation;
    private GameObject CurrentEquippedItem;
    private FirstPersonController fpsController;

    private void Start()
    {
        fpsController = gameObject.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fpsController.enabled)
        {
            if(CrossPlatformInputManager.GetButtonDown("Interact"))
            {
                if (itemInView != null)
                {
                    itemInView.GetComponent<InteractableBase>().OnInteract();
                }
            }
            else if(CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                if (CurrentEquippedItem != null)
                {
                    CurrentEquippedItem.GetComponent<Pickupable>().OnUse();
                }
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
            {
                itemInView = HitObject;
            }
            //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.green);
        }
        else if (itemInView != null)
        {
            itemInView = null;
            //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white);
        }
        else
        {
            //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.white);
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
        GetComponent<ItemSway>().targetLocation = DesiredItemLocation;
    }

    
}
