using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//using UnityEngine.SceneManagement;

/// <summary>
/// This handles all mouse clicks for the MCD simulation
/// </summary>
public class MCD_UI : MonoBehaviour {

    GameObject globalObject;
    Globals globalAccess;

    public AvionicsFaultPage avionicsFaultPage;

    MCD_Fail_UI mcd_Fail_UI;

    Load_TitleScrolling loadscreen;

    // Used to manage button animations, not arrayed to keep it readable
    MCD_ButtonMgr b_MSG;
    MCD_ButtonMgr b_STS;

    MCD_ButtonMgr b_L1;
    MCD_ButtonMgr b_L2;
    MCD_ButtonMgr b_L3;
    MCD_ButtonMgr b_L4;
    MCD_ButtonMgr b_L5;
    MCD_ButtonMgr b_L6;

    MCD_ButtonMgr b_M1;
    MCD_ButtonMgr b_M2;
    MCD_ButtonMgr b_M3;
    MCD_ButtonMgr b_M4;
    MCD_ButtonMgr b_M5;
    MCD_ButtonMgr b_M6;

    MCD_ButtonMgr b_R1;
    MCD_ButtonMgr b_R2;
    MCD_ButtonMgr b_R3;
    MCD_ButtonMgr b_R4;
    MCD_ButtonMgr b_R5;
    MCD_ButtonMgr b_R6;

    // UI Alignment
    const float topRow_startPos = 0.325f;
    const float leftCol_startPos = 0.175f;

    float topRow = 0.325f;
    float leftCol = 0.175f;
    float rightCol = 0.730f;

    float xButtonScale = 0.07f;
    float yButtonScale = 0.07f;

    float xSep = 0.09f;
    float ySep = 0.079f;

    // Externs
    MCD_Manager Manager;

    bool directLoad = false;

    // Use this for initialization
    void Start () {

        mcd_Fail_UI = GameObject.Find("!MCD UI").GetComponent<MCD_Fail_UI>();

        try
        {
            globalObject = GameObject.Find("~Globals");
            globalAccess = globalObject.GetComponent<Globals>();
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {
            Debug.Log("Global variables not loaded, UI debug mode engaged");
            globalObject = GameObject.Find("~Globals(Clone)");
            globalAccess = globalObject.GetComponent<Globals>();
        }

        try
        {
            loadscreen = GameObject.Find("Loadscreen Script").GetComponent<Load_TitleScrolling>();
        }
        catch
        {
            Debug.Log("PickleSticks: Couldn't find the load screen, using direct loading");
            directLoad = true;
        }

        // Obtain control of button MCD_Manager.Models for animation
        // You're probably wondering why I did this, well the main controller lost it's public assignments a few times
        // It takes a lot of time to reassign this in the inspector
        b_L1 = GameObject.Find("L1").GetComponent<MCD_ButtonMgr>();
        b_L2 = GameObject.Find("L2").GetComponent<MCD_ButtonMgr>();
        b_L3 = GameObject.Find("L3").GetComponent<MCD_ButtonMgr>();
        b_L4 = GameObject.Find("L4").GetComponent<MCD_ButtonMgr>();
        b_L5 = GameObject.Find("L5").GetComponent<MCD_ButtonMgr>();
        b_L6 = GameObject.Find("L6").GetComponent<MCD_ButtonMgr>();

        b_M1 = GameObject.Find("Lower Button 1M COM").GetComponent<MCD_ButtonMgr>();
        b_M2 = GameObject.Find("Lower Button 2M MSN").GetComponent<MCD_ButtonMgr>();
        b_M3 = GameObject.Find("Lower Button 3M SKE").GetComponent<MCD_ButtonMgr>();
        b_M4 = GameObject.Find("Lower Button 4M Up Arrow").GetComponent<MCD_ButtonMgr>();
        b_M5 = GameObject.Find("Lower Button 5M Down Arrow").GetComponent<MCD_ButtonMgr>();
        b_M6 = GameObject.Find("Lower Button 6M Index").GetComponent<MCD_ButtonMgr>();

        b_R1 = GameObject.Find("R1").GetComponent<MCD_ButtonMgr>();
        b_R2 = GameObject.Find("R2").GetComponent<MCD_ButtonMgr>();
        b_R3 = GameObject.Find("R3").GetComponent<MCD_ButtonMgr>();
        b_R4 = GameObject.Find("R4").GetComponent<MCD_ButtonMgr>();
        b_R5 = GameObject.Find("R5").GetComponent<MCD_ButtonMgr>();
        b_R6 = GameObject.Find("R6").GetComponent<MCD_ButtonMgr>();

        // Obtain control of manager for MCD
        Manager = GameObject.Find("!MCD Manager").GetComponent<MCD_Manager>();
    }

    public void gui_MCD_Off()
    {
        // Disable all controls
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.Off();
        b_M1.Off();
        b_M2.Off();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.Off();
    }
    ////////////// Comm Mode Pages //////////


    public void gui_Comm_Message()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;


        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;          // Reset click timer
                      // Enable not simulated message
            Manager.statusNotSimulatedTimer = 1.0f;     // Set not simulated timer      (TODO: Reduce to single function)
            b_L1.Push();                                // Animate button
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
            b_M6.Push();
            Manager.timeSinceLastClick = 0.0f;
        }

        leftCol += xSep;
    }

    
    // Button handler for main comm page, has access to HF menus
    public void gui_CommMain()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;


        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L3.On();
        b_R3.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        
        if (Manager.timeSinceLastClick > Manager.clickDelay) {
            //L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            //L3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_HF;
                Manager.timeSinceLastClick = 0.0f;
                b_L3.Push();
            }

            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            // L5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);

            //L6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            //R1
            topRow = 0.33f;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);
            
            //R2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);
            
            //R3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_HF;
                Manager.timeSinceLastClick = 0.0f;
                b_R3.Push();
            }
            
            //R4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);
            
            //R5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);
            
            //R6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R6);

            // Bottom Buttons
            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }
            
            //2MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }
            
            //3MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);
            
            //4MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);
            
            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);
            
            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                b_M6.Push();
                Manager.timeSinceLastClick = 0.0f;
            }
        }
    }

    // Button handler for comm HF page, to toggle radios and faults
    public void gui_CommHF()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;


        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L2.On();
        b_R2.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();


        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            //L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                
                Manager.timeSinceLastClick = 0.0f;
                globalAccess.misc_HF1_On = !globalAccess.misc_HF1_On;
                avionicsFaultPage.UpdatePages();
                b_L2.Push();
            }

            //L3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);


            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            // L5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);

            //L6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            //R1
            topRow = 0.33f;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            //R2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.timeSinceLastClick = 0.0f;
                globalAccess.misc_HF2_On = !globalAccess.misc_HF2_On;
                avionicsFaultPage.UpdatePages();
                b_R2.Push();
            }

            //R3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            //R4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            //R5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            //R6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons
            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;

            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            //2MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            //3MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            //4MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                b_M6.Push();
                Manager.timeSinceLastClick = 0.0f;
            }

            leftCol += xSep;
        }
    }
    
    // Button handler for comm index page 1
    public void gui_CommIndex1()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L3.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        // L1
        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            //L3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_HF;
                Manager.timeSinceLastClick = 0.0f;
                b_L3.Push();
            }

            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);
            
            // L5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);

            //L6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);


             // RIGHT BUTTONS
             topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            //R2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);
            
            //R3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);


            //R4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            //R5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            //R6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) 
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_EmCom;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons
            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            //2MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            //3MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);
            
            //4MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M4.Push();
            }

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M5.Push();
            }

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    // Button handler for comm index page 2
    public void gui_CommIndex2()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {

            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_L1.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_L2.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_L3.Push();
            }

            topRow += ySep;
            //L4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_L4.Push();
            }

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_L5.Push();
            }

            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.CommMaint;
                Manager.timeSinceLastClick = 0.0f;
                b_L6.Push();
            }

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R1.Push();
            }

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R2.Push();
            }

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R3.Push();
            }

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R4.Push();
            }

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R5.Push();
            }

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_M3.Push();
            }

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M4.Push();
            }

            leftCol += xSep;
            //5MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M5.Push();
            }

            leftCol += xSep;
            //6MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }

        }
    }

    // Button handler for emissions control page
    public void gui_Comm_EmCom()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_R1.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        // L1
        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                globalAccess.misc_Rad1_On = !globalAccess.misc_Rad1_On;
                avionicsFaultPage.UpdatePages();
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            //L3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            // L5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);

            //L6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);


            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                globalAccess.misc_Rad2_On = !globalAccess.misc_Rad2_On;
                avionicsFaultPage.UpdatePages();
                Manager.timeSinceLastClick = 0.0f;
                b_R1.Push();
            }

            //R2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            //R3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);


            //R4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            //R5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            //R6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons
            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;

            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            //2MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            //3MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            //4MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);
            
            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);
            

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    /// Button handler for comm index page 1
    public void gui_CommMaint()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.On();
        b_L2.On();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.On();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.CommFault;
            Manager.timeSinceLastClick = 0.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.CommFaultHist;
            Manager.timeSinceLastClick = 0.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Status;
            Manager.timeSinceLastClick = 0.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_2;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    /// Button handler for comm index page 1
    public void gui_CommFault()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.CommMaint;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    /// <summary>
    /// Button handler for comm fault history
    /// </summary>
    public void gui_CommFaultHist()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.CommMaint;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    /// <summary>
    /// Button handler for comm status generation
    /// </summary>
    public void gui_CommStatus()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.CommMaint;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    //////////// Mission Mode Pages /////////////

    /// <summary>
    /// Button handler for main mission page
    /// </summary>
    public void gui_MsnMain()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_M1.On();
        b_M2.On();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.timeSinceLastClick = 0.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.timeSinceLastClick = 0.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    // Button handler for main mission page 2
    public void gui_Msn2Main()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_L4.On();
        b_L6.On();
        b_M1.On();
        b_M2.On();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_AvionicsFaults;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_NonAvionicsFaults;
                Manager.timeSinceLastClick = 0.0f;
                b_L2.Push();
            }

            //L3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);
            
            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_EGT_Overtemp;
                Manager.timeSinceLastClick = 0.0f;
                b_L4.Push();
            }

            // L5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);

            //L6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint;
                Manager.timeSinceLastClick = 0.0f;
                b_L6.Push();
            }

            // RIGHT BUTTONS

            //R1
            topRow = 0.33f;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            //R2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            //R3
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);
            
            //R4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);
            
            //R5
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            //R6
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R6);
            
            
            // Bottom Buttons
            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            //2MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }
            
            //3MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            //4MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M4.Push();
            }

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M5.Push();
            }


            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    /// <summary>
    /// Button handler for mission maintenance 
    /// </summary>
    public void gui_Maint()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_L4.On();
        b_L5.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint_MCEDS;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_EFCS_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_L2.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                b_L4.Push();
            }

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_MCEDS_ADPMC2;
                Manager.timeSinceLastClick = 0.0f;
                b_L5.Push();
            }

            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    /// <summary>
    /// Button handler for mceds
    /// </summary>
    public void gui_MCEDS()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.On();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_AvionicsFaults;

            Manager.timeSinceLastClick = 0.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay) ButtonNotSimulated(b_L2);

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay) ButtonNotSimulated(b_L3);

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay) ButtonNotSimulated(b_L4);

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay) ButtonNotSimulated(b_L5);

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay) ButtonNotSimulated(b_L6);
        
        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint;

            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;

            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;

            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;

            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    public void gui_Maint_APDMC2()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L5.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Egt_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_L5.Push();
            }

            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_Faults_Main()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_List;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Propulsion;
                Manager.timeSinceLastClick = 0.0f;
                b_L2.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_Faults_List()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_Faults_Prop()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_L3.On();
        b_L4.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Eng1;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Eng2;
                Manager.timeSinceLastClick = 0.0f;
                b_L2.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Eng3;
                Manager.timeSinceLastClick = 0.0f;
                b_L3.Push();
            }

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Eng4;
                Manager.timeSinceLastClick = 0.0f;
                b_L4.Push();
            }

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_Faults_Eng1()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Faults_Propulsion;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_EgtOvertemp_Main()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_L3.On();
        b_L4.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_EgtHist_Eng1;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_EgtHist_Eng2;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_EgtHist_Eng3;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            //L4
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_EgtHist_Eng4;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_Maint_APDMC2_EgtOvertemp_Eng1()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_APDMS2_Egt_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_EFCS_Main()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_L1.On();
        b_L2.On();
        b_L4.On();
        b_L5.On();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_EFCS_Fault_List;
                Manager.timeSinceLastClick = 0.0f;
                b_L1.Push();
            }


            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_EFCS_Fault_History;
                Manager.timeSinceLastClick = 0.0f;
                b_L2.Push();
            }

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4 - This button left active as a learning opportunity. APDMS 2 only works on MCDs 3 & 4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                mcd_Fail_UI.currentMode = MCD_Fail_UI.mode.Rig;
                Manager.timeSinceLastClick = 0.0f;
                b_L4.Push();
            }
            

            topRow += ySep;
            // L5
                
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                mcd_Fail_UI.currentMode = MCD_Fail_UI.mode.Initialize;
                Manager.timeSinceLastClick = 0.0f;
                b_L5.Push();
            }


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    public void gui_EFCS_FaultList()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            topRow += ySep;
            // L2
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            // L4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);

            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R1);

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R2);

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R3);

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R4);

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_R5);

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none))
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Maint_EFCS_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }
    

    public void gui_Msn_EGTovertemp()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        ResetMCDButtons();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        if (Manager.timeSinceLastClick > Manager.clickDelay)
        {
            // L1
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L1);

            // L2
            topRow += ySep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L2);

            topRow += ySep;
            //L3
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L3);

            topRow += ySep;
            //L4
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L4);

            topRow += ySep;
            // L5
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L5);


            topRow += ySep;
            //L6
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_L6);


            // RIGHT BUTTONS
            topRow = 0.33f;
            //R1
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R1.Push();
            }

            topRow += ySep;
            //R2
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R2.Push();
            }

            topRow += ySep;
            //R3
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R3.Push();
            }

            topRow += ySep;
            //R4
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R4.Push();
            }

            topRow += ySep;
            //R5
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.timeSinceLastClick = 0.0f;
                
                Manager.statusNotSimulatedTimer = 1.0f;
                b_R5.Push();
            }

            topRow += ySep;
            //R6
            if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_R6.Push();
            }

            // Bottom Buttons

            topRow = 0.87f;
            xButtonScale = 0.07f;
            yButtonScale = 0.1f;
            leftCol = 0.265f;
            xSep = 0.0745f;
            //1MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
                Manager.timeSinceLastClick = 0.0f;
                b_M1.Push();
            }

            leftCol += xSep;
            //2MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
                Manager.timeSinceLastClick = 0.0f;
                b_M2.Push();
            }

            leftCol += xSep;
            //3MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M3);

            leftCol += xSep;
            //4MS
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M4);

            //5MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none)) ButtonNotSimulated(b_M5);

            //6MS
            leftCol += xSep;
            if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
            {
                Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
                Manager.timeSinceLastClick = 0.0f;
                b_M6.Push();
            }
        }
    }

    /// <summary>
    /// Avionics fault list
    /// </summary>
    public void gui_AVfaults()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.avionicsFaults_CurrentPage++;
            if (Manager.avionicsFaults_CurrentPage > Manager.avionicsFaults_TotalPages) Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.avionicsFaults_CurrentPage--;
            if (Manager.avionicsFaults_CurrentPage <= 0) Manager.avionicsFaults_CurrentPage = Manager.avionicsFaults_TotalPages;

            Manager.timeSinceLastClick = 0.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }

    /// <summary>
    /// Non-Avionics fault list
    /// </summary>
    public void gui_NonAv()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.On();
        b_M2.On();
        b_M3.Off();
        b_M4.On();
        b_M5.On();
        b_M6.On();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L6.Push();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Comm_Main;
            Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;
            Manager.avionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M3.Push();
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            if (Manager.nonAvionicsFaults_CurrentPage == 1) Manager.nonAvionicsFaults_CurrentPage = 2;
            else Manager.nonAvionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M4.Push();
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            if (Manager.nonAvionicsFaults_CurrentPage == 1) Manager.nonAvionicsFaults_CurrentPage = 2;
            else Manager.nonAvionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M5.Push();
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_2;
            Manager.nonAvionicsFaults_CurrentPage = 1;

            Manager.timeSinceLastClick = 0.0f;
            b_M6.Push();
        }

        leftCol += xSep;
    }


    

    /// <summary>
    /// Quit menus
    /// </summary>
    public void gui_Quit()
    {
        topRow = topRow_startPos;
        leftCol = leftCol_startPos;
        xButtonScale = 0.07f;
        yButtonScale = 0.07f;

        // Alter rendering of buttons to provide visual hints of which buttons are simulated
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.On();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.On();
        b_M1.Off();
        b_M2.Off();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.Off();

        // L1
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "") && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L1.Push();
        }

        topRow += ySep;
        // L2
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L2.Push();
        }

        topRow += ySep;
        //L3
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L3.Push();
        }

        topRow += ySep;
        //L4
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L4.Push();
        }

        topRow += ySep;
        // L5
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_L5.Push();
        }

        topRow += ySep;
        //L6
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Maint;

            Manager.timeSinceLastClick = 0.0f;
            b_L6.Push();

            //if (directLoad) SceneManager.LoadScene("10_Main Menu");
            //else loadscreen.LoadScreen("10_Main Menu");
            //Application.Quit();
        }

        // RIGHT BUTTONS
        topRow = 0.33f;
        //R1
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R1.Push();
        }

        topRow += ySep;
        //R2
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R2.Push();
        }

        topRow += ySep;
        //R3
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R3.Push();
        }

        topRow += ySep;
        //R4
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R4.Push();
        }

        topRow += ySep;
        //R5
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            
            Manager.statusNotSimulatedTimer = 1.0f;
            b_R5.Push();
        }

        topRow += ySep;
        //R6
        if (GUI.Button(new Rect(ScaleX(rightCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.mcdCurrentMode = MCD_Manager.Mode.Msn_Index_1;

            Manager.timeSinceLastClick = 0.0f;
            b_R6.Push();
        }

        // Bottom Buttons

        topRow = 0.87f;
        xButtonScale = 0.07f;
        yButtonScale = 0.1f;
        leftCol = 0.265f;
        xSep = 0.0745f;
        //1MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M1.Push();
        }

        leftCol += xSep;
        //2MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            Manager.timeSinceLastClick = 0.0f;
            Manager.statusNotSimulatedTimer = 1.0f;
            b_M2.Push();
        }

        leftCol += xSep;
        //3MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            ButtonNotSimulated(b_M3);
        }

        leftCol += xSep;
        //4MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            ButtonNotSimulated(b_M4);
        }

        leftCol += xSep;
        //5MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            ButtonNotSimulated(b_M5);
        }

        leftCol += xSep;
        //6MS
        if (GUI.Button(new Rect(ScaleX(leftCol), ScaleY(topRow), ScaleX(xButtonScale), ScaleY(yButtonScale)), "", GUIStyle.none) && Manager.timeSinceLastClick > Manager.clickDelay)
        {
            ButtonNotSimulated(b_M6);
        }

        leftCol += xSep;
    }


    /////////// HIGH USE FUNCTIONS ///////////
    /// <summary>
    /// Resets sim timers
    /// </summary>
    void ButtonNotSimulated(MCD_ButtonMgr btn)
    {
        Manager.timeSinceLastClick = 0.0f;          // Reset click timer
                  // Enable not simulated message
        Manager.statusNotSimulatedTimer = 1.0f;     // Set not simulated timer      (TODO: Reduce to single function)
        btn.Push();
        /*switch (btn)
        {
            case ("L1"):
                b_L1.Push();
                break;
            case ("L2"):
                b_L2.Push();
                break;
            case ("L3"):
                b_L3.Push();
                break;
            case ("L4"):
                b_L4.Push();
                break;
            case ("L5"):
                b_L5.Push();
                break;
            case ("L6"):
                b_L6.Push();
                break;
            case ("R1"):
                b_R1.Push();
                break;
            case ("R2"):
                b_R2.Push();
                break;
            case ("R3"):
                b_R3.Push();
                break;
            case ("R4"):
                b_R4.Push();
                break;
            case ("R5"):
                b_R5.Push();
                break;
            case ("R6"):
                b_R6.Push();
                break;
            case ("M1"):
                b_M1.Push();
                break;
            case ("M2"):
                b_M2.Push();
                break;
            case ("M3"):
                b_M3.Push();
                break;
            case ("M4"):
                b_M4.Push();
                break;
            case ("M5"):
                b_M5.Push();
                break;
            case ("M6"):
                b_M6.Push();
                break;
            default:
                Debug.Log("ERROR: Invalid button press");
                break;
        }*/
    }
        
    /// <summary>
    /// Turns all buttons off, before setting them
    /// </summary>
    void ResetMCDButtons()
    {
        b_L1.Off();
        b_L2.Off();
        b_L3.Off();
        b_L4.Off();
        b_L5.Off();
        b_L6.Off();
        b_R1.Off();
        b_R2.Off();
        b_R3.Off();
        b_R4.Off();
        b_R5.Off();
        b_R6.Off();
        b_M1.Off();
        b_M2.Off();
        b_M3.Off();
        b_M4.Off();
        b_M5.Off();
        b_M6.Off();
    }

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
