using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    private static List<UIBase> UIList = new List<UIBase>();
    [HideInInspector]
    public GameObject owningPlayer;
    public bool enableUIOverlap = false;

    void OnEnable()
    {
        if (!enableUIOverlap)
            UIList.Add(this);
    }

    void OnDisable()
    {
        if (!enableUIOverlap)
            UIList.Remove(this);
    }

    /// <summary>
    /// Bind the UI to the inputted GameObject
    /// </summary>
    /// <param name="owner">The GameObject to bind to</param>
    /// <returns></returns>
    public virtual void Bind(GameObject owner)
    {
        owningPlayer = owner;
    }

    public abstract bool IsEnabled();

    public bool CanEnable()
    {
        foreach (UIBase UI in UIList)
        {
            if (UI != this && UI.IsEnabled())
                return false;
        }
        return true;
    }

    public virtual void DisablePlayer(bool disablePlayer)
    {
        if (disablePlayer)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
