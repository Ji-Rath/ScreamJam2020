using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class InventorySystem : ScriptableObject
{
    public ItemSlot[] inventory;
    public int currentSlot;
}

[Serializable]
public struct ItemSlot
{
    public ItemBase item;
    public int itemAmount;
}
