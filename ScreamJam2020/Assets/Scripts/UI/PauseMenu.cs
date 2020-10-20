using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    private FirstPersonController playerController;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuUI.SetActive(false);
        playerController = GameManager.Get().playerRef.GetComponent<FirstPersonController>();
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
        isGamePaused = !isGamePaused;

        if(playerController)
        {
            playerController.m_MouseLook.SetCursorLock(!isGamePaused);
            playerController.enabled = !isGamePaused;
        }
        
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
