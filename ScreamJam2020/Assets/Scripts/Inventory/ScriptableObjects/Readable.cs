using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Readable", menuName = "Readable")]
public class Readable : ItemBase
{
    [Tooltip("The body of text to display"), TextArea]
    public string content;
}
