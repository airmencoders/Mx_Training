using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used to manage animation state
/// </summary>
public enum Status
{
    ready,
    start,
    hold,
    end,
    stopped
}



public class Load_TitleScrolling : MonoBehaviour {

    Globals globals;

    public ScrollingObj[] AnimatedImg = new ScrollingObj[5];

    Status overallStatus = Status.ready;        // Status we want
    Status currentStatus = Status.ready;        // Where we really are

    float timer = 0.0f;

    bool calledLoad = false;            // Load is buffered
    bool isLoading = false;             // Load is started
    bool hasLoaded = false;             // Load is complete

    string desiredScene = "Intro";      // This is our starting point

    public MotivationalText motivate;   // Used to access motivational text

    ////// Chute dropping stuff //////
    public GameObject ChootPrefab;      // Parachute object

    float dropTimer = 0.0f;             // How long since last drop
    float dropDelay = 0.2f;             // Delay between drops

    // Use this for initialization
    void Start()
    {
        globals = GameObject.Find("~Globals").GetComponent<Globals>();          // Get access to global variables. Sure this is bad deal with it.

        // Ensure all timers match default times, left public for testing
        for (int i = 0; i < AnimatedImg.Length; i++)
        {
            AnimatedImg[i].timer = AnimatedImg[i].startTimer;
        }

        // Check if we are in presentation mode
        if (globals.demo_mode == true)
        {
            SceneManager.LoadSceneAsync("90_Intro");         // We are skipping the load sequence, and going right to the intro slides
        }
        else
        {
            LoadScreen("10_Main Menu");
            //SceneManager.LoadSceneAsync("10_Main Menu");         // We are skipping the load sequence, and going right to the intro slides
        }
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        // Poll for status
        if (currentStatus == Status.start)
        {
            bool ready = true;
            for (int i = 0; i < AnimatedImg.Length; i++)
            {
                

                if (AnimatedImg[i].currentState == Status.start)
                {
                    ready = false;
                }
            }
            if (ready == true) currentStatus = Status.hold;
        }

        else if (currentStatus == Status.hold)
        {
            if (isLoading == false)
            {
                Debug.Log("Attempting to load scene");
                SceneManager.LoadSceneAsync(desiredScene);
                isLoading = true;
            }
            if (SceneManager.GetSceneByName(desiredScene).isLoaded)
            {
                hasLoaded = true;
                EndScreen();
            }
            DropChute();
        }
        else if (currentStatus == Status.end)
        {
            bool ready = true;
            for (int i = 0; i < AnimatedImg.Length; i++)
            {
               if (AnimatedImg[i].currentState == Status.end)
                {
                    ready = false;
                }
            }
            if (ready == true) currentStatus = Status.stopped;
        }
        else if(currentStatus == Status.stopped)
        {
            ResetScreen();
            globals.lockUI = false;
        }

        for (int i = 0; i < AnimatedImg.Length; i++)
        {
            // Object is in start position and ready to go
            if (AnimatedImg[i].currentState == Status.ready)
            {
                AnimatedImg[i].timer = AnimatedImg[i].startTimer;                       //Reset Timer
                AnimatedImg[i].currentPos.position = AnimatedImg[i].StartPos;
            }

            // Start moving object in, automatic hold transisition
            if (AnimatedImg[i].currentState == Status.start)
            {
                AnimatedImg[i].timer += Time.deltaTime;
                AnimatedImg[i].currentPos.position = Vector3.Lerp(AnimatedImg[i].StartPos, AnimatedImg[i].holdPos, AnimatedImg[i].timer);
                if (AnimatedImg[i].timer > 1.0f)
                {
                    //AnimatedImg[i].timer = 0.0f;
                    AnimatedImg[i].currentState = Status.hold;
                    string dbgStr = "i = " + AnimatedImg[i].currentState.ToString();
                    Debug.Log(dbgStr);
                }
            }

            else if (AnimatedImg[i].currentState == Status.hold)
            {
                AnimatedImg[i].timer += Time.deltaTime;
                AnimatedImg[i].currentPos.position = AnimatedImg[i].holdPos;
            }

            else if (AnimatedImg[i].currentState == Status.end)
            {
                AnimatedImg[i].timer += Time.deltaTime;
                AnimatedImg[i].currentPos.position = Vector3.Lerp(AnimatedImg[i].holdPos, AnimatedImg[i].endPos, AnimatedImg[i].timer);
                if (AnimatedImg[i].timer > 1.0f)
                {
                    AnimatedImg[i].currentState = Status.stopped;
                }
            }
            else if (AnimatedImg[i].currentState == Status.stopped)
            {
                AnimatedImg[i].currentPos.position = AnimatedImg[i].endPos;
                AnimatedImg[i].timer += Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Scroll screen in and start loading process
    /// </summary>
    /// <param name="scene"></param>
    public void LoadScreen(string scene)
    {
        calledLoad = true;
        globals.lockUI = true;
        motivate.RandomQuote();
        overallStatus = Status.start;           // Debugging, start animation after 5 second delay
        currentStatus = Status.start;           // Debugging, this is our real status

        desiredScene = scene;

        for (int i = 0; i < AnimatedImg.Length; i++)
        {
            AnimatedImg[i].currentState = overallStatus;
        }
    }

    /// <summary>
    /// Scroll screen out
    /// </summary>
    public void EndScreen()
    {
        overallStatus = Status.end;           // Debugging, start animation after 5 second delay
        currentStatus = Status.end;           // Debugging, this is our real status

        for (int i = 0; i < AnimatedImg.Length; i++)
        {
            AnimatedImg[i].currentState = overallStatus;
            AnimatedImg[i].timer = (AnimatedImg[i].startTimer * -1.0f) - 2.0f;
        }
    }

    void ResetScreen()
    {
        for (int i = 0; i < AnimatedImg.Length; i++)
        {
            AnimatedImg[i].currentState = Status.ready;
            AnimatedImg[i].timer = AnimatedImg[i].startTimer;
        }
        calledLoad = false;
        isLoading = false;
        hasLoaded = false;
    }

    void DropChute()
    {
        dropTimer += Time.deltaTime;
        // Chute drop
        if (dropTimer > dropDelay)
        {
            dropTimer = 0.0f;
            Instantiate(ChootPrefab);
            Debug.Log("CHOOT SHOOT");
        }
    }
}

[System.Serializable]
public class ScrollingObj
{
    public Transform currentPos;
    public Vector3 StartPos;
    public Vector3 holdPos;
    public Vector3 endPos;
    public float startTimer = -3.0f;
    public float timer = -3.0f;
    public Status currentState = Status.ready;

    
}