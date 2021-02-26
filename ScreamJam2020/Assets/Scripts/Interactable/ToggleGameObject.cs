using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;

public class ToggleGameObject : Interactable
{
    public List<GameObject> toggleGameObject = new List<GameObject>();

    public override void OnInteract(GameObject Interactor)
    {
        foreach (GameObject newGameObject in toggleGameObject)
            newGameObject.SetActive(!newGameObject.activeSelf);
    }
}
