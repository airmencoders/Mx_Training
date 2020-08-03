using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Highly based on KnobController, there is likely a lot of un-needed code here. :(
/// </summary>
public class SW_Hot_Xmit : MonoBehaviour {


    Vector3 initialPos;
    Vector3 currentPos;
    Vector3 offsetPos;

    Vector3 knobWorldPos_Initial;


    string debugString;
    string GUIstr;

    bool isClicked = false;
    public int knob_pos = 1;

    int knob_old_pos;
    int radioSelection = 0;

    // DEBUGGING
    int radio = 0;
    int volume = 5;
    bool listening = false;


    // Pull toggle
    bool pull_isReady = true;
    bool pull_Toggle = false;

    // Public variables
    public bool debugMode = true;

    public bool wasPulled = false;
    public bool isPushed = false;

    AudioSource snd_Switched;

    void Start()
    {
        knobWorldPos_Initial = this.transform.position;         // Record original knob position
        snd_Switched = GameObject.Find("Sound - Switch Switched").GetComponent<AudioSource>();
        knob_old_pos = knob_pos;
    }

    void Update()
    {
        // Check if we are taking no actions, and reset the knob
        if (isClicked == false && knob_pos == 2)
        {
            knob_pos = 1;
        }

        if (debugMode == true)       // Used for testing, enabling this will likely kill it's function
        {
            //ReadMovement();
        }


        MoveKnob();

        if (offsetPos.y > 0.03f)
        {
            knob_pos = 0;
        }
        else if (offsetPos.y < -0.03f)
        {
            knob_pos = 2;
        }
        else
        {
            knob_pos = 1;
        }

        //CheckRotation();

        if (knob_pos == 0)
        {
            isPushed = true;
        }
        else if (knob_pos == 1)
        {
            isPushed = false;
        }

        // Knob pull request processing
        if (knob_pos == 2)
        {
            if (pull_isReady == true)
            {
                pull_Toggle = true;
                wasPulled = true;
            }
            pull_isReady = false;
        }
        else
        {
            pull_isReady = true;
        }

        if (knob_pos != knob_old_pos) snd_Switched.Play();
        knob_old_pos = knob_pos;
    }

    private void OnGUI()
    {
        if (debugMode == false) return;


        debugString = "Initial Position: " + initialPos.x + ", " + initialPos.y;
        debugString += "\nCurrent Position: " + currentPos.x + ", " + currentPos.y;
        debugString += "\nOffset Position: " + offsetPos.x + ", " + offsetPos.y;

        debugString += "\n\nTouching Knob: " + isClicked;
        debugString += "\nKnob Position: " + knob_pos;
        debugString += "\nPull Requested: " + pull_Toggle;

        debugString += "\n\nRadio Position: " + radio;
        debugString += "\nVolume Position: " + volume;
        debugString += "\n Listening: " + listening;


        GUIstr = debugString;

        GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.1f), ScaleX(1.0f), ScaleY(1.0f)), GUIstr);
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

    private void OnMouseDrag()
    {
        currentPos.x = Input.mousePosition.x;
        currentPos.y = Input.mousePosition.y;

        currentPos.x = currentPos.x / Screen.width;
        currentPos.y = currentPos.y / Screen.width;

        offsetPos.x = offsetPos.x / Screen.width;
        offsetPos.y = offsetPos.y / Screen.width;

        offsetPos = currentPos - initialPos;



        //Debug.Log(debugString);
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
                break;
            case 1:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                break;
            default:
                this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -20.0f);
                break;
        }
    }

    /////////// HIGH USE FUNCTIONS ///////////


    /// <summary>
    /// sX(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">X position from left to right</param>
    /// <returns></returns>    
    public float ScaleX(float i)
    {
        i = (float)Screen.width * i;
        return i;
    }


    /// <summary>
    /// sY(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">Y position from top to bottom</param>
    /// <returns></returns>    
    public float ScaleY(float i)
    {
        i = (float)Screen.height * i;
        return i;
    }
}
