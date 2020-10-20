using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreen : MonobehaviourSingleton<UILoadingScreen>
{
    public TextMeshProUGUI loadingText;
    public Slider loadingBar;
    public GameObject nextLevelButton;

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        LoaderManager.OnFullyLoadLevel += ShowUI;
        if (nextLevelButton)
        {
            nextLevelButton.SetActive(false);
        }
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        int loadingVal = (int)(LoaderManager.Get().loadingProgress * 100) + 1;

        if (loadingVal >= 100)
        {
            loadingVal = 100;
        }

        loadingText.text = "LOADING LEVEL " + loadingVal + "%";
        loadingBar.value = loadingVal;

        if (LoaderManager.Get().loadingProgress >= 1 && LoaderManager.Get().acceptLoad)
        {
            LoaderManager.Get().loadingProgress = 0;
            LoaderManager.Get().timeLoading = 0;
            SetVisible(false);
        }
            
    }

    public void ShowUI()
    {
        if(nextLevelButton)
        {
            nextLevelButton.SetActive(true);
        }
        
    }

    private void OnDestroy()
    {
        LoaderManager.OnFullyLoadLevel -= ShowUI;
    }
}
