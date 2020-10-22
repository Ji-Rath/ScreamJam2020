using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public abstract class UIBase : MonoBehaviour
{
    private static List<UIBase> UIList = new List<UIBase>();
    protected FirstPersonController playerController;

    void Awake()
    {
        UIList.Add(this);
    }

    protected virtual void Start()
    {
        playerController = GameManager.Get().playerRef.GetComponent<FirstPersonController>();
    }

    void OnDestroy()
    {
        UIList.Remove(this);
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

    public void DisablePlayer(bool disablePlayer)
    {
        if(playerController && playerController.enabled == disablePlayer)
        {
            playerController.m_MouseLook.SetCursorLock(!disablePlayer);
            playerController.enabled = !disablePlayer;
        }   
    }
}
