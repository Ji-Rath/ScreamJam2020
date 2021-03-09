using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InteractSystem;
using JiRath.InteractSystem.UI;

public class PlayerInteraction : InteractManager
{
    public GameObject interactUI;

    // Start is called before the first frame update
    void Start()
    {
        interactUI = Instantiate(interactUI);
        interactUI.GetComponent<UIBase>().Bind(gameObject);
    }
}
