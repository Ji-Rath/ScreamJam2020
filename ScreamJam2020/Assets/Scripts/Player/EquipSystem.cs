using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InventorySystem.Pickupables;

namespace JiRath.InventorySystem.EquipSystem
{
    public class EquipSystem : MonoBehaviour
    {
        public delegate void OnPlayerEquipAction(int chance);
        public static OnPlayerEquipAction OnPlayerDropItem;

        [HideInInspector]
        public Pickupable currentEquippedItem = null;
        public Transform itemDisplay;
        public AudioClip dropSound;
        public Vector3 scalePreview;
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
            {
                if (currentEquippedItem.item == item.GetComponent<Pickupable>().item)
                {
                    Destroy(currentEquippedItem.gameObject);
                    return;
                }

                Destroy(currentEquippedItem.gameObject);
            }


            //Create new item

            currentEquippedItem = Instantiate(item, itemDisplay).GetComponent<Pickupable>();
            currentEquippedItem.transform.localScale = scalePreview;
            currentEquippedItem.GetComponent<Collider>().enabled = false;
            currentEquippedItem.GetComponent<Rigidbody>().isKinematic = true;

            //Make sure it follows the player properly
            ItemSway itemSway = GetComponent<ItemSway>();
            if (itemSway)
            {
                itemSway.TargetItem = currentEquippedItem.gameObject;
                itemSway.targetLocation = itemDisplay.transform;
            }
        }

        //Drop the selected item
        public void DropItem(ItemBase item)
        {
            if (!item) { Debug.Log("No Item found to drop!"); return; }

            GetComponent<InventoryManager>().RemoveFromInventory(item, 1);

            if (currentEquippedItem != null)
                Destroy(currentEquippedItem.gameObject);

            if (audioSource)
            {
                audioSource.clip = dropSound;
                audioSource.Play();
            }

            GameObject droppedItem = Instantiate(item.itemModel, gameObject.transform.position, Quaternion.identity);
            Pickupable pickup = droppedItem.GetComponent<Pickupable>();

            //Add velocity and angular velocity to thrown object
            Rigidbody itemBody = droppedItem.GetComponent<Rigidbody>();
            itemBody.velocity = Camera.main.transform.forward * throwForce;
            itemBody.angularVelocity = new Vector3(throwForce, throwForce, throwForce);

            //Cause enemy to appear
            if (pickup)
            {
                OnPlayerDropItem?.Invoke(pickup.enemySpawnChance);
            }



        }
    }
}