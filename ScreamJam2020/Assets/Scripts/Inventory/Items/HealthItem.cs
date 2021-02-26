using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InventorySystem.Pickupables;

namespace JiRath.InventorySystem.Interactables
{
    public class HealthItem : Pickupable
    {
        public override bool Use()
        {
            Debug.Log("Gave player some health!");
            return true;
        }
    }
}