using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class InventorySystem : ScriptableObject
{
    public List<ItemSlot> inventory;
    public int currentSlot = 0;
    public int maxSlots = 3;
}

[Serializable]
public struct ItemSlot
{
    public ItemBase item;
    public int itemAmount;

    public ItemSlot(ItemBase item, int itemAmount)
    {
        this.item = item;
        this.itemAmount = itemAmount;
    }
}
