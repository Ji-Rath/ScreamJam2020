using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using JiRath.InteractSystem.UI;
using JiRath.InventorySystem.UI;

public class UIController : MonoBehaviour
{
    public List<GameObject> UIList;

    public event Action<bool> OnTogglePause;
    public bool isPaused;

    void Start()
    {
        for(var i=0;i<UIList.Count;i++)
        {
            UIList[i] = Instantiate(UIList[i]);
            var ui = UIList[i].GetComponent<UIBase>();
            ui.OnDisablePlayer += DisablePlayerMovement;
            ui.Bind(gameObject);
        }
    }

    public void DisablePlayerMovement(bool disabled)
    {
        GetComponent<FirstPersonController>().enabled = !disabled;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !isPaused;
            OnTogglePause?.Invoke(isPaused);
        }
    }
}
