using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenu : UIBase
{
    public bool isGamePaused = false;
    public GameObject pauseMenuUI;

    public override bool IsEnabled()
    {
        return isGamePaused;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(CrossPlatformInputManager.GetButtonDown("Pause"))
        {
            SwitchPause();
        }
    }

    public void SwitchPause()
    {
        //Make sure there is no other UI open before enabling
        if (!IsEnabled() && !CanEnable()) { return; }
        

        isGamePaused = !isGamePaused;

        DisablePlayer(isGamePaused);

        if (isGamePaused)
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
}
