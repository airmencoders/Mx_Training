using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles loading screen, and all associated commands with changing scene.
/// As much as I loved the animations, 
/// Get it Loading Master, haha so punny, so tired.
/// </summary>
public class LoadingMaster : MonoBehaviour 
{
    public bool DebugMode = false;
    public bool DirectLoading = false;



    bool calledLoad = true;

    public Globals g;       // Used to get UI locking status
    public Camera c;        // Used to enable/disable screen
    public LoadingQuotes quotes;    // Used to alter quotes
    
    public string DesiredScene;


    // Start is called before the first frame update
    void Start()
    {
        g = GameObject.Find("~Globals").GetComponent<Globals>();          // Get access to global variables

        // Check if we are in presentation mode
        if (g.demo_mode == true)
        {
            LoadScreen("90_Intro");         // We are skipping the load sequence, and going right to the intro slides
        }
        else
        {
            LoadScreen("10_Main Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetSceneByName(DesiredScene).isLoaded) HideLoadingScreen();
    }

    public void LoadScreen(string scene)
    {
        DesiredScene = scene;
        ShowLoadingScreen();
        quotes.RandomQuote();
        
        SceneManager.LoadSceneAsync(DesiredScene);
    }

    void ShowLoadingScreen()
    {
        g.lockUI = true;
        c.enabled = true;
    }
    void HideLoadingScreen()
    {
        c.enabled = false;
        g.lockUI = false;
    }
}
