using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MCD_Fail_UI : MonoBehaviour {

    public GUIStyle big;
    public GUIStyle normal;
    public GUIStyle background;
    
    public enum mode
    {
        Off,
        Rig,
        Initialize,
        ICS,
        Chop
    };

    public mode currentMode = mode.Off;

    Vector3 MCD_Pos = new Vector3(-5.0f, 15.0f, -4.0f);
    Vector3 ICS_Pos = new Vector3(-7.0f, 15.0f, -4.0f);
    Vector3 set_Pos;

    public GameObject Karate_Chop;

    bool rig = false;
    bool initialize = false;
    public bool ics = false;




    // Use this for initialization
    void Start () {
        set_Pos = MCD_Pos;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentMode == mode.Off) return;
        if (currentMode == mode.Chop)
        {
            Instantiate(Karate_Chop, set_Pos, transform.rotation);
            currentMode = mode.Off;
        }
        big.fontSize = (int)(Screen.width * 0.085f);
        normal.fontSize = (int)(Screen.width * 0.03f);
        
        
    }

    private void OnGUI()
    {
        if (currentMode == mode.Rig)
        {
            if(rig == true)
            {
                set_Pos = MCD_Pos;
                currentMode = mode.Chop;
                return;
            }
            GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", background);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(0.2f)), "Don't go here!!!\n", big);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.2f), ScaleX(1.0f), ScaleY(1.0f)),
                "This menu contains destructive power that will make the flightline\n" +
                "fall in love with you. Seriously though don't go in this menu, only bad\n" +
                "things can result from you going here.\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Lesson learned:\n" +
                "With knowledge comes power, the power to stay out of the 'Rig' menu!\n" +
                "", normal);

            if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", GUIStyle.none))
            {
                rig = true;
                currentMode = mode.Off;
            }
        }

        if (currentMode == mode.Initialize)
        {
            if (initialize == true)
            {
                set_Pos = MCD_Pos;
                currentMode = mode.Chop;
                return;
            }
            GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", background);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(0.2f)), "Don't go here!!!\n", big);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.2f), ScaleX(1.0f), ScaleY(1.0f)),
                "This menu contains really destructive power will cause a disturbance\n" +
                "in the Flightline, as if all AFIN specialists cried out in terror and were\n" +
                "suddenly silenced.\n" +
                "\n" +
                "Only something terrible can result from you going here.\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Lesson learned:\n" +
                "Use the force, stay out of the 'Initialization' menus!\n" +
                "", normal);

            if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", GUIStyle.none))
            {
                initialize = true;
                currentMode = mode.Off;
            }
        }

        if(currentMode == mode.ICS)
        {
            if (ics == true)
            {
                set_Pos = ICS_Pos;
                currentMode = mode.Chop;
                return;
            }
            GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", background);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(0.2f)), "Congratulations!\n", big);
            GUI.Label(new Rect(ScaleX(0.015f), ScaleY(0.2f), ScaleX(1.0f), ScaleY(1.0f)),
                "You found the 'GROUND OPS POWER' switch. This shuts off power to all\n" +
                "avionics systems! Looks like you'll need to re-apply power. If this plane\n" +
                "was getting ready to take off, the pilots will love you.\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "\n" +
                "Lesson learned:\n" +
                "Don't touch a switch if you don't know what it does!", normal);

            if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", GUIStyle.none))
            {
                ics = true;
                currentMode = mode.Off;
            }
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
