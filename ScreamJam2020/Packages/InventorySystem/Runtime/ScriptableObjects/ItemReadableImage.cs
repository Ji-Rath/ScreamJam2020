using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemReadableImage", menuName = "ItemReadableImage")]
public class ItemReadableImage : ItemBase
{
    [Tooltip("The image to display")]
    public Sprite image;
}
