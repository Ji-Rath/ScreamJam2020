﻿using System;

///Interface is used for when a Pickupable item can be used on an Interactable
public interface IItemUsable
{
    event Action UseItem;

    /// <summary>
    /// Called when an item (pickupable) is used on self
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Whether the item could be used by self</returns>
    bool OnItemUse(ItemBase item);

    /// <summary>
    /// Determines the solved state of the component
    /// </summary>
    /// <returns>Whether this component is 'correct' or solved</returns>
    bool IsCorrect();
}
