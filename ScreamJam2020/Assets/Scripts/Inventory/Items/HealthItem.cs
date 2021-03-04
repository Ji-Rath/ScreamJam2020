using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;

namespace JiRath.InventorySystem.Interactables
{
    public class HealthItem : Pickupable
    {
        public override bool Use(GameObject user, Interactable itemInView)
        {
            Debug.Log("Gave player some health!");
            return true;
        }
    }
}