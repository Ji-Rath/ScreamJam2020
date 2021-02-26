using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiRath.InventorySystem.Pickupables
{
    public class Readable : Pickupable
    {
        public override bool Use()
        {
            //ReadUI.instance.UI_UseItem(this);
            return true;
        }
    }
}