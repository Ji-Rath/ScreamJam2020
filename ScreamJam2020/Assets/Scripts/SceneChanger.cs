using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public string sceneToChange;
    public GameObject menuCanvas;
    public GameObject creditCanvas;
    public GameObject controlsCanvas;
    public bool hasLoadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.Get().PlaySound("Menu");
       /* if (PlayerState.Get().hasPlayerWon)
        {
            GoToCredits();
            PlayerState.Get().hasPlayerWon = false;
        }*/
    }

    public void ChangeScene()
    {
        if(hasLoadingScreen)
        {
            LoaderManager.Get().LoadScene(sceneToChange);
            UILoadingScreen.Get().SetVisible(true);

            if (GameManager.Get())
            {
                Destroy(GameManager.Get().gameObject);
            }
        }
        else
        {
            if (GameManager.Get())
            {
                Destroy(GameManager.Get().gameObject);
            }

            SceneManager.LoadScene(sceneToChange);
        }
        
    }

    public void ChangeScene(string newScene)
    {
        if (hasLoadingScreen)
        {
            LoaderManager.Get().LoadScene(newScene);
            UILoadingScreen.Get().SetVisible(true);

            if(GameManager.Get().gameObject)
            {
                Destroy(GameManager.Get().gameObject);
            }
        }
        else
        {
            if (GameManager.Get().gameObject)
            {
                Destroy(GameManager.Get().gameObject);
            }


            SceneManager.LoadScene(newScene);  
        }
        
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void PlayButtonSound()
    {
        //SoundManager.Get().PlaySound("Boton");
    }

    public void GoToCredits()
    {
        menuCanvas.SetActive(false);
        creditCanvas.SetActive(true);
        controlsCanvas.SetActive(false);
    }

    public void GoToMenu()
    {
        creditCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        controlsCanvas.SetActive(false);
    }

    public void GoToControls()
    {
        creditCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        controlsCanvas.SetActive(true);
    }
}
