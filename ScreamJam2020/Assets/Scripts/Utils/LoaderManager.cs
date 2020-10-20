using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonobehaviourSingleton<LoaderManager>
{
    public delegate void OnLoaderAction();
    public static OnLoaderAction OnFullyLoadLevel;

    public float loadingProgress;
    public bool fakeLoad;
    public bool acceptLoad;
    public float timeLoading;
    public float minTimeToLoad = 2;
    private bool doOnce;
    private bool instantChange = false;
    private AsyncOperation ao = null;

    public void LoadScene(string sceneName)
    {
        doOnce = false;
        acceptLoad = false;
        instantChange = false;
        loadingProgress = 0;
        timeLoading = 0;
        StartCoroutine(AsynchronousLoad(sceneName));
    }

    public void LoadSceneAuto(string sceneName)
    {
        doOnce = false;
        acceptLoad = true;
        instantChange = false;
        loadingProgress = 0;
        timeLoading = 0;
        StartCoroutine(AsynchronousLoad(sceneName));
    }

    public void EnableLoad()
    {
        acceptLoad = true;
        instantChange = true;
    }

    IEnumerator AsynchronousLoad(string scene)
    {
        if (loadingProgress == 0)
        {
            ao = SceneManager.LoadSceneAsync(scene);
            ao.allowSceneActivation = false;
        }
        

        while (!ao.isDone)
        {

            timeLoading += Time.deltaTime;
            loadingProgress = ao.progress + 1.0f;

            if(fakeLoad && !instantChange)
            {
                loadingProgress = loadingProgress * timeLoading / minTimeToLoad;
            }


            // Loading completed
            if (loadingProgress >= 1)
            {
                loadingProgress = 1;

                if (!doOnce)
                {
                    if(OnFullyLoadLevel != null)
                    {
                        OnFullyLoadLevel();
                    }

                    doOnce = true;
                }

                if(acceptLoad)
                {
                    ao.allowSceneActivation = true;
                }
                
            }

            yield return null;
        }
    }
}
