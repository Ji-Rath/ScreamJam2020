using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObject : Interactable
{
    public List<GameObject> toggleGameObject = new List<GameObject>();

    public override void OnInteract()
    {
        foreach (GameObject newGameObject in toggleGameObject)
            newGameObject.SetActive(!newGameObject.activeSelf);
    }
}
