using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SW_Mode : MonoBehaviour {

    Vector3 initialPos;
    Vector3 currentPos;
    Vector3 offsetPos;

    Vector3 knobWorldPos_Initial;


    string debugString;
    string GUIstr;

    bool isClicked = false;
    public int knob_pos = 1;
    int radioSelection = 0;

    // DEBUGGING
    int radio = 0;
    public int mode = 0;
    bool listening = false;


    // Pull toggle
    bool pull_isReady = true;
    bool pull_Toggle = false;

    // Public variables
    public bool debugMode = true;

    public bool turnedCCW = false;                  // I don't like polling variables, but this will have to do.
    public bool turnedCW = false;
    public bool wasPulled = false;
    public bool isPushed = false;

    AudioSource snd_KnobTurned;

    void Start()
    {
        knobWorldPos_Initial = this.transform.position;         // Record original knob position
        snd_KnobTurned = GameObject.Find("Sound - KnobTurned").GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if we are taking no actions, and reset the knob
        if (isClicked == false)
        {
            knob_pos = 1;
        }

        
            ReadMovement();
        

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

        CheckRotation();

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
        debugString += "\nmode Position: " + mode;
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
    /// Poll knob movement, for debugging
    /// </summary>
    private void ReadMovement()
    {
        if (isPushed == true)            // Change radio
        {
            if (turnedCW)
            {
                radio++;
                turnedCW = false;
            }
            else if (turnedCCW)
            {
                radio--;
                turnedCCW = false;
            }
        }
        else                            // Change mode
        {
            if (turnedCW)
            {
                if(mode < 3)mode++;
                turnedCW = false;
            }
            else if (turnedCCW)
            {
                if (mode > 0) mode--;
                turnedCCW = false;
            }
        }

        if (wasPulled == true)
        {
            listening = !listening;
            wasPulled = false;
        }
    }

    /// <summary>
    /// All knob movement and rotation should be handled here
    /// </summary>
    private void MoveKnob()
    {
        switch (knob_pos)
        {
            case 0:
                this.transform.position = new Vector3(knobWorldPos_Initial.x, knobWorldPos_Initial.y, knobWorldPos_Initial.z + 0.1f);
                break;
            case 1:
                this.transform.position = knobWorldPos_Initial;
                break;
            default:
                this.transform.position = new Vector3(knobWorldPos_Initial.x, knobWorldPos_Initial.y, knobWorldPos_Initial.z - 0.1f);
                break;
        }

        switch (mode)
        {
            case 0:
                this.transform.localEulerAngles = new Vector3(140.0f, 90.0f, 270.0f);
                break;
            case 1:
                this.transform.localEulerAngles = new Vector3(180.0f, 90.0f, 270.0f);
                break;
            case 2:
                this.transform.localEulerAngles = new Vector3(220.0f, 90.0f, 270.0f);
                break;
            default:
                this.transform.localEulerAngles = new Vector3(260.0f, 90.0f, 270.0f);
                break;
        }
    }

    /// <summary>
    /// Check for X-axis movement
    /// </summary>
    private void CheckRotation()
    {
        if (offsetPos.x > 0.05f)
        {
            turnedCW = true;
            snd_KnobTurned.Play();
            initialPos.x = currentPos.x;
        }
        else if (offsetPos.x < -0.05f)
        {
            turnedCCW = true;
            snd_KnobTurned.Play();
            initialPos.x = currentPos.x;
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

