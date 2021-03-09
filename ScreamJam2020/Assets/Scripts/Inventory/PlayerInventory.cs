using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JiRath.InventorySystem;
using JiRath.InteractSystem.UI;

public class PlayerInventory : InventoryManager
{
    public GameObject invUI;
    public GameObject readUI;

    // Start is called before the first frame update
    void Start()
    {
        invUI = Instantiate(invUI);
        invUI.GetComponent<UIBase>().Bind(gameObject);

        readUI = Instantiate(readUI);
        readUI.GetComponent<UIBase>().Bind(gameObject);
    }
}
