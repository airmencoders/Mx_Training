using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Refuel_Sw_Generic : MonoBehaviour
{
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

    

    AudioSource snd_Switched;

    void Start()
    {
        knobWorldPos_Initial = this.transform.position;         // Record original knob position
        snd_Switched = GameObject.Find("Sound - BIT Switch").GetComponent<AudioSource>();
        if (starting_knob_pos == 1 & !threeWay)
        {
            starting_knob_pos = 0;
            Debug.Log("ERROR: Two-way switch set in center position, defaulted to position 0");
        }
        knob_pos = starting_knob_pos;
        knob_old_pos = starting_knob_pos;
    }

    void Update()
    {
        // Check if we are taking no actions, and reset the knob
        if (holdingCoil == false)
        {
            knob_pos = 1;
            offsetPos.y = 0.0f;
        }

        MoveKnob();
        if(!threeWay) Process2Way();
        else Process3Way();


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

    /// <summary>
    ///  Process gesture for 3-way switch
    /// </summary>
    private void Process3Way()
    {
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
    }

    /// <summary>
    /// UI used for debugging
    /// </summary>
    private void OnGUI()
    {
        if (debugMode == false) return;

        debugString = "Name: " + this.gameObject.name;
        debugString += "\nInitial Position: " + initialPos.x + ", " + initialPos.y;
        debugString += "\nCurrent Position: " + currentPos.x + ", " + currentPos.y;
        debugString += "\nOffset Position: " + offsetPos.x + ", " + offsetPos.y;

        debugString += "\n\nTouching Knob: " + isClicked;
        debugString += "\nKnob Position: " + knob_pos;

        
        GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.1f), ScaleX(1.0f), ScaleY(1.0f)), debugString);
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
