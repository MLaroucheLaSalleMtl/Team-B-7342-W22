using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Loading : MonoBehaviour
{
    /// <summary>
    /// 1) Preloader, don't put anything in the variable 
    /// 2) With progressbar or percentage: Assign progressbar and/or txtPercent
    /// 3) WaitForUserInput: also in use when using a cutscene, but don't use the delay
    /// 4) Delay: When you want to display hints (but don't use waitForUserInput)
    /// X) If I don't use loadSceneByName or sceneToLoadByIndex = load next scene.
    /// X1) If I specify a name in loadSceneByName, it will use the scnee name
    /// X2) If sceneToLoadByIndex = -1: Load next scene, any other number = build index
    /// </summary>

    private AsyncOperation async;
    [SerializeField] private Image progressBar;
    [SerializeField] private Text txtPercent;
    [Header("Use Wait for user input for cutscene or title screen")]
    [SerializeField] private bool waitForUserInput = false;
    private bool ready = false;
    [SerializeField] private GameObject waitForUserInputTxt;
    [Header("Use delay only if not using Wait for user input")]
    [SerializeField] private float delay = 0f;
    [Header("Only in used if Load Scene by Name is empty")]
    [Tooltip("-1: Load next scene, otherwise use the build index number to load a specific scene")]
    [SerializeField] private int sceneToLoadByIndex = -1;

    private bool anyKey = false;

    // Start is called before the first frame update
    void Start()
    {
        InitValue();
        if (sceneToLoadByIndex < 0) //No valid build index
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(currentScene);
            async = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
        }
        else //valid build index
        {
            async = SceneManager.LoadSceneAsync(sceneToLoadByIndex);
        }

        async.allowSceneActivation = false; //To not allow the scene to show until splash screen is done

        if (!waitForUserInput)
        {
            Invoke("Activate", delay);
        }
        if (waitForUserInputTxt)
            waitForUserInputTxt.SetActive(false);
    }

    public void OnAnyKey(InputAction.CallbackContext context)
    {
        anyKey = context.performed;
    }

    public void Activate()
    {
        ready = true;
    }

    void InitValue()
    {
        Time.timeScale = 1.0f;
        Input.ResetInputAxes();
        System.GC.Collect();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar)
            progressBar.fillAmount = async.progress + 0.1f;

        if (txtPercent)
            txtPercent.text = ((async.progress + 0.1f) * 100).ToString("F2") + " %";

        if (waitForUserInput && waitForUserInputTxt & async.progress > 0.89f)
        {
            waitForUserInputTxt.SetActive(true);
        }

        if (waitForUserInput && anyKey)
        {
            if (async.progress > 0.89f && SplashScreen.isFinished)
            {
                ready = true;
            }
        }

        if (async.progress > 0.89f && SplashScreen.isFinished && ready)
        {
            async.allowSceneActivation = true;
        }
    }
}
