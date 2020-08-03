using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_BackupAudio : MonoBehaviour {
    Transform t;

    public bool isOn = false;
    bool hasChanged = true;

    AudioSource snd_Switched;



    Vector3 initialPos;
    Vector3 currentPos;
    Vector3 offsetPos;

    Vector3 knobWorldPos_Initial;

    string debugString;

    bool isClicked = false;
    public int starting_knob_pos = 1;               // Starting position
    public int knob_pos = 1;                        // Switch active position, two-way uses 0 and 1 only
    int knob_old_pos;

    // Switch Status
    public bool holdingCoil = true;                // Switch will lock into place
    public bool threeWay = true;                    // Two-way or three-way switch

    // Pull toggle
    bool pull_isReady = true;
    bool pull_Toggle = false;


    bool wasPulled = false;
    bool isPushed = false;

    // Public variables
    public bool debugMode = true;

    void Start()
    {
        t = GetComponent<Transform>();
        snd_Switched = GameObject.Find("Sound - Switch Switched").GetComponent<AudioSource>();

        knobWorldPos_Initial = this.transform.position;         // Record original knob position
        if (starting_knob_pos == 1)
        {
            starting_knob_pos = 0;
            Debug.Log("ERROR: Two-way switch set in center position, defaulted to position 0");
        }
        knob_pos = starting_knob_pos;
        knob_old_pos = starting_knob_pos;
    }

    void Update()
    {
        if (hasChanged == false) return;                // No need to poll this, performance tweak

        // TODO: Add sounds

        if (isOn == true) t.localEulerAngles = new Vector3(0.0f, 0.0f, -20.0f);
        else t.localEulerAngles = new Vector3(0.0f, 0.0f, 20.0f);

        // Check if we are taking no actions, and reset the knob
        if (holdingCoil == false)
        {
            knob_pos = 1;
            offsetPos.y = 0.0f;
        }

        MoveKnob();
        Process2Way();

        if (knob_pos != knob_old_pos) snd_Switched.Play();
        knob_old_pos = knob_pos;
    }

    /// <summary>
    ///  Process gesture for 2-way switch
    /// </summary>
    private void Process2Way()
    {
        if (offsetPos.y > 0.03f)
        {
            knob_pos = 0;
        }
        else if (offsetPos.y < -0.03f)
        {
            knob_pos = 2;
        }
    }


    private void OnMouseDown()
    {
        initialPos.x = Input.mousePosition.x;
        initialPos.y = Input.mousePosition.y;

        initialPos.x = initialPos.x / Screen.width;
        initialPos.y = initialPos.y / Screen.width;

        isClicked = true;
    }

    private void OnMouseUp()
    {
        isClicked = false;
    }

    /// <summary>
    /// Determine gesture offset
    /// </summary>
    private void OnMouseDrag()
    {
        currentPos.x = Input.mousePosition.x;
        currentPos.y = Input.mousePosition.y;

        currentPos.x = currentPos.x / Screen.width;
        currentPos.y = currentPos.y / Screen.width;

        offsetPos.x = offsetPos.x / Screen.width;
        offsetPos.y = offsetPos.y / Screen.width;

        offsetPos = currentPos - initialPos;
    }

    /// <summary>
    /// All knob movement and rotation should be handled here
    /// </summary>
    private void MoveKnob()
    {
        switch (knob_pos)
        {
            case 0:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 20.0f);
                isOn = false;
                break;
            case 1:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            default:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -20.0f);
                isOn = true;
                break;
        }
    }

}
