using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Pickupable
{
    public override bool Use()
    {
        Debug.Log("Gave player some health!");
        return true;
    }
}
