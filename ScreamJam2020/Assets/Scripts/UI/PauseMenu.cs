using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;
using JiRath.InteractSystem.UI;

public class PauseMenu : UIBase
{
    public GameObject pauseMenuUI;

    public override bool IsEnabled()
    {
        return pauseMenuUI.activeSelf;
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    public void SwitchPause(bool isPaused)
    {
        //Make sure there is no other UI open before enabling
        if (!CanEnable()) { return; }

        DisablePlayer(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenuUI.SetActive(isPaused);

        // Used to update player comp - primarily used to update variables when the player presses a UI button to switch pause.
        UIController player = owningPlayer.GetComponent<UIController>();
        player.isPaused = isPaused;

    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
        if (owningPlayer)
            owningPlayer.GetComponent<UIController>().OnTogglePause -= SwitchPause;
    }

    public override void Bind(GameObject owner)
    {
        base.Bind(owner);
        owner.GetComponent<UIController>().OnTogglePause += SwitchPause;
    }
}
