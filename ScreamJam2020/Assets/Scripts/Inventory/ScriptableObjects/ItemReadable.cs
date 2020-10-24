using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemReadable", menuName = "ItemReadable")]
public class ItemReadable : ItemBase
{
    [Tooltip("The body of text to display"), TextArea]
    public string content;
}
