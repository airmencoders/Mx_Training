using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Local class for managing faults

/// <summary>
/// This monster drives the UI, and display rendering for the MCD. Might seprate this later should it be approved for further development.
///
/// TODO: Divorce MCD text rendering from this file (Unable to do within time available)
/// </summary>
public class MCD_Manager : MonoBehaviour {
    // Allows passing of variables from main menu, display, and possible briefing window later. 
    
    bool debugMode = false;
    public bool MCD_Is_Ready = false;
    public GameObject defaultGlobals;                   // NOTE: defaultGlobals is used for debugging only

    public MCD_Static_Pages stringStorage;

    GameObject globalObject;
    Globals globalAccess;

    GameObject helpUI_Object;
    MCD_Help helpUI_Access;

    MCD_UI mcd_UI;
    

    public AvionicsFaultPage avionicsFaultPage;

    

    // MCD Screen Modes
    public enum Mode
    {
        Comm_Main,
        Comm_Message,
        Comm_HF,
        Comm_Index_1,         
        Comm_Index_2, 
        Comm_EmCom,
        CommMaint,
        CommFault,
        CommFaultHist,
        Comm_Status,

        Msn_Index_1,               
        Msn_Index_2,
        Msn_EGT_Overtemp,
        Msn_AvionicsFaults,
        Msn_NonAvionicsFaults,
        // TODO: THESE WERE DISABLED IN THE UI DUE TO TIME CONSTRAINTS. INSTRUCTOR FIRST, PROGRAMMING SECOND.
        Msn_Maint,
        Msn_Maint_MCEDS,
        Msn_MCEDS_Faults,
        Msn_MCEDS_History,
        Msn_MCEDS_SCEFS,
        Msn_MCEDS_ADPMC2,

        Maint_APDMS2,
        Maint_APDMS2_Faults_Main,
        Maint_APDMS2_Faults_List,
        Maint_APDMS2_Faults_Propulsion,
        Maint_APDMS2_Faults_Eng1,
        Maint_APDMS2_Faults_Eng2,
        Maint_APDMS2_Faults_Eng3,
        Maint_APDMS2_Faults_Eng4,
        Maint_APDMS2_Egt_Main,
        Maint_APDMS2_EgtHist_Eng1,
        Maint_APDMS2_EgtHist_Eng2,
        Maint_APDMS2_EgtHist_Eng3,
        Maint_APDMS2_EgtHist_Eng4,

        Maint_EFCS_Main,
        Maint_EFCS_Fault_List,
        Maint_EFCS_Fault_History,
        
        Quit,
        NotSim
    }
    
    public Mode mcdCurrentMode = Mode.Comm_Main;       

    
    // Help UI Misc
    public float timeSinceLastClick = 0.0f;
    public float clickDelay = 0.1f;

    // Random button
    public Renderer sprite_RandomOn;
    public Renderer sprite_RandomOff;
    float randomButtonTimer = 0.0f;

    public Renderer sprite_HelpOn;
    public Renderer sprite_HelpOff;
    public Renderer sprite_HelpPage1;

    float helpButtonTimer = 0.0f;
    bool helpDisplayEnabled = false;

    // Quit button
    public Renderer sprite_QuitOn;
    public Renderer sprite_QuitOff;
    float quitButtonTimer = 0.0f;

    
    // Sounds
    public AudioSource sndOK;
    public AudioSource sndFAIL;
    public AudioSource sndHFchime;

    bool HF1_LastStatus = false;
    bool HF2_LastStatus = false;

    // Brightness Knob
    bool displayTurnedOn = false;
    float displayBrightnessTimer = 2.0f;

    public Transform brtKnob;
    Vector3[] knobPos = new Vector3[2]
    {
        new Vector3(0.0f, 0.0f, -45.0f),        // Knob off position
        new Vector3(0.0f, 0.0f, -310.0f)        // Knob on position
    };

    
    // Fault counters
    // int totalFaults = 0;
    public int avionicsFaults_CurrentPage = 1;
    public int avionicsFaults_TotalPages = 1;

    public int nonAvionicsFaults_CurrentPage = 1;

    // Display is 24x14
    string currentDisp;
    string currentDispRev;
    public GameObject mcdDisp;
    public GameObject mcdDispRev;
    public GameObject mcdDispGlow;
    public GameObject mcdDispRevGlow;

    bool Msg_On = true;

    
    // Buttons Access used to manage animation of buttons, and toggling of msg button light
    public Renderer msg_btn_on;
    public Renderer msg_btn_off;

    public MCD_ButtonMgr b_L1;
    public MCD_ButtonMgr b_L2;
    public MCD_ButtonMgr b_L3;
    public MCD_ButtonMgr b_L4;
    public MCD_ButtonMgr b_L5;
    public MCD_ButtonMgr b_L6;
    public MCD_ButtonMgr b_M1;
    public MCD_ButtonMgr b_M2;
    public MCD_ButtonMgr b_M3;
    public MCD_ButtonMgr b_M4;
    public MCD_ButtonMgr b_M5;
    public MCD_ButtonMgr b_M6;
    public MCD_ButtonMgr b_R1;
    public MCD_ButtonMgr b_R2;
    public MCD_ButtonMgr b_R3;
    public MCD_ButtonMgr b_R4;
    public MCD_ButtonMgr b_R5;
    public MCD_ButtonMgr b_R6;

    //////////////////// Manage status messages ///////////////////////
    
    public float statusNotSimulatedTimer = 0.0f;

    /// <summary>
    /// Runs automatically on scene load
    /// </summary>
    void Start() {
        

        mcd_UI = GameObject.Find("!MCD UI").GetComponent<MCD_UI>();
        

        // These lines address a rendering flaw in Text Mesh, and allows for it to be drawn behind other objects. Removing this will break fading, and help pages
        mcdDisp.GetComponent<MeshRenderer>().sortingLayerName = "Default";
        mcdDispRev.GetComponent<MeshRenderer>().sortingLayerName = "Default";
        mcdDisp.GetComponent<MeshRenderer>().sortingOrder = 0;
        mcdDispRev.GetComponent<MeshRenderer>().sortingOrder = 0;

        // This will detect if this scene was directly loaded, and if so enable debug mode using default settings
        try
        {
            globalObject = GameObject.Find("~Globals");
            globalAccess = globalObject.GetComponent<Globals>();
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {
            Debug.Log("Global variables not loaded, debug mode engaged");
            Instantiate(defaultGlobals);
            globalObject = GameObject.Find("~Globals(Clone)");
            globalAccess = globalObject.GetComponent<Globals>();
            debugMode = true;
        }
        

        // Help UI
        helpUI_Object = GameObject.Find("!Help_UI");
        helpUI_Access = helpUI_Object.GetComponent<MCD_Help>();
        
        // Sets HF status or purpose of playing sounds
        HF1_LastStatus = globalAccess.misc_HF1_On;
        HF2_LastStatus = globalAccess.misc_HF2_On;

        // MY MCD IS READY!!!!111111ONE!11
        MCD_Is_Ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        randomButtonTimer += Time.deltaTime;

        if (quitButtonTimer > 0.10f)
        {
            sprite_RandomOn.enabled = false;
            sprite_RandomOff.enabled = true;
        }

        if (helpButtonTimer > 0.10f)
        {
            sprite_HelpOn.enabled = false;
            sprite_HelpOff.enabled = true;
        }
        
        quitButtonTimer += Time.deltaTime;
        if (quitButtonTimer > 0.10f)
        {
            sprite_QuitOn.enabled = false;
            sprite_QuitOff.enabled = true;
        }


        SetStatusLight();

        TextMesh t = mcdDisp.GetComponent<TextMesh>();
        TextMesh tg = mcdDispGlow.GetComponent<TextMesh>();
        TextMesh t2 = mcdDispRev.GetComponent<TextMesh>();
        TextMesh tg2 = mcdDispRevGlow.GetComponent<TextMesh>();

        displayBrightnessTimer += Time.deltaTime;

        if (displayTurnedOn == true)
        {
            helpUI_Access.wasOn = true;
            helpUI_Access.hideHint = true;
            brtKnob.localEulerAngles = Vector3.Lerp(knobPos[0], knobPos[1], displayBrightnessTimer);
            t.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.0f, 1.0f, displayBrightnessTimer));
            tg.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.0f, 0.5f, displayBrightnessTimer));
            t2.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.0f, 1.0f, displayBrightnessTimer));
            tg2.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.0f, 0.1f, displayBrightnessTimer));
        }
        else
        {
            brtKnob.localEulerAngles = Vector3.Lerp(knobPos[1], knobPos[0], displayBrightnessTimer);
            t.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, displayBrightnessTimer));
            tg.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.5f, 0.0f, displayBrightnessTimer));
            t2.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, displayBrightnessTimer));
            tg2.color = new Vector4(0.0f, 1.0f, 0.0f, Mathf.Lerp(0.1f, 0.0f, displayBrightnessTimer));
        }


        // Time since last command, smart boards seem to have a odd delay that is firing multiple clicks at once
        timeSinceLastClick += Time.deltaTime;

        // Screen mode
        if (mcdCurrentMode == Mode.Comm_Message) disp_Comm_Message();
        else if (mcdCurrentMode == Mode.Comm_Main) disp_CommMain();
        else if (mcdCurrentMode == Mode.Comm_HF) disp_CommHF();
        else if (mcdCurrentMode == Mode.Comm_Index_1) disp_CommIndex1();
        else if (mcdCurrentMode == Mode.Comm_Index_2) disp_CommIndex2();
        else if (mcdCurrentMode == Mode.Comm_EmCom) disp_Comm_EmCom();
        else if (mcdCurrentMode == Mode.CommMaint) disp_CommMaint();
        else if (mcdCurrentMode == Mode.CommFault) disp_CommFault();
        else if (mcdCurrentMode == Mode.CommFaultHist) disp_CommFaultHist();
        else if (mcdCurrentMode == Mode.Comm_Status) disp_CommStatus();
        else if (mcdCurrentMode == Mode.Msn_Index_1) MsnIndex1();
        else if (mcdCurrentMode == Mode.Msn_Index_2) MsnIndex2();
        else if (mcdCurrentMode == Mode.Msn_EGT_Overtemp) disp_Msn_EGTOvertemp();// disp_Msn_EGTOverTemp();
        else if (mcdCurrentMode == Mode.Msn_Maint) disp_Maint();
        else if (mcdCurrentMode == Mode.Msn_Maint_MCEDS) disp_MCEDS();
        else if (mcdCurrentMode == Mode.Msn_MCEDS_ADPMC2) disp_APDMC2_Maint();
        else if (mcdCurrentMode == Mode.Msn_AvionicsFaults) genAvFaults(avionicsFaults_CurrentPage);
        else if (mcdCurrentMode == Mode.Msn_NonAvionicsFaults) disp_NonAv();

        else if (mcdCurrentMode == Mode.Maint_APDMS2) disp_APDMC2_Maint();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Main) disp_APDMC2_Fault_Main();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_List) disp_APDMC2_Fault_List();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Propulsion) disp_APDMC2_Fault_Propulsion();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng1) disp_APDMC2_Fault_Propulsion_Eng1();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng2) disp_APDMC2_Fault_Propulsion_Eng2();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng3) disp_APDMC2_Fault_Propulsion_Eng3();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng4) disp_APDMC2_Fault_Propulsion_Eng4();

        else if (mcdCurrentMode == Mode.Maint_APDMS2_Egt_Main) disp_APDMC2_EgtHistory_Main();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng1) disp_APDMC2_EgtHistory_Eng1();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng2) disp_APDMC2_EgtHistory_Eng2();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng3) disp_APDMC2_EgtHistory_Eng3();
        else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng4) disp_APDMC2_EgtHistory_Eng4();

        // EFCS - Flight controls faults
        else if (mcdCurrentMode == Mode.Maint_EFCS_Main) disp_EFCS_Main();
        else if (mcdCurrentMode == Mode.Maint_EFCS_Fault_List) disp_EFCS_FaultList();
        else if (mcdCurrentMode == Mode.Maint_EFCS_Fault_History) disp_EFCS_FaultHistory();

        else if (mcdCurrentMode == Mode.Quit) disp_Quit();


        t.text = currentDisp;
        tg.text = currentDisp;

        t2.text = currentDispRev;
        tg2.text = currentDispRev;

        if (statusNotSimulatedTimer > 0.0f)
        {
            globalAccess.stat_NotSim = true;
            statusNotSimulatedTimer -= Time.deltaTime;
        }
        else
        {
            globalAccess.stat_NotSim = false;
        }

        CheckHFStatus();

        randomButtonTimer += Time.deltaTime;
        helpButtonTimer += Time.deltaTime;
        quitButtonTimer += Time.deltaTime;


    }

    private void OnGUI()
    {
        float topRow = 0.1f;

        float xScale = 0.07f;
        float yScale = 0.13f;

        if (debugMode == true) GUI.Label(new Rect(sX(0.0f), sY(0.0f), sX(0.8f), sY(0.14f)), "Standalone Mode: ENABLED");

        // Help buttons
        if (helpDisplayEnabled)
        {
            if (GUI.Button(new Rect(sX(0.0f), sY(0.0f), sX(1.0f), sY(1.0f)), "", GUIStyle.none) && timeSinceLastClick > clickDelay)
            {
                helpButtonTimer = 0.0f;
                sprite_HelpOn.enabled = true;
                sprite_HelpOff.enabled = false;
                helpUI_Access.hideHint = true;
                helpUI_Access.wasOn = true;
                helpDisplayEnabled = !helpDisplayEnabled;

                if (helpDisplayEnabled == true)
                {
                    sprite_HelpOn.enabled = true;
                    sprite_HelpOff.enabled = false;
                    sprite_HelpPage1.enabled = true;
                }
                else
                {
                    sprite_HelpOn.enabled = false;
                    sprite_HelpOff.enabled = true;
                    sprite_HelpPage1.enabled = false;
                }

                if (sndOK.isPlaying == false) sndOK.Play();
                timeSinceLastClick = 0.0f;
                return;
            }
        }
        else
        {
            if (GUI.Button(new Rect(sX(0.88f), sY(0.66f), sX(0.08f), sY(0.15f)), "", GUIStyle.none) && timeSinceLastClick > clickDelay)
            {
                helpButtonTimer = 0.0f;
                sprite_HelpOn.enabled = true;
                sprite_HelpOff.enabled = false;
                helpUI_Access.hideHint = true;
                helpUI_Access.wasOn = true;
                helpDisplayEnabled = !helpDisplayEnabled;

                if (helpDisplayEnabled == true)
                {
                    sprite_HelpOn.enabled = true;
                    sprite_HelpOff.enabled = false;
                    sprite_HelpPage1.enabled = true;
                }
                else
                {
                    sprite_HelpOn.enabled = false;
                    sprite_HelpOff.enabled = true;
                    sprite_HelpPage1.enabled = false;
                }

                if (sndOK.isPlaying == false) sndOK.Play();
                timeSinceLastClick = 0.0f;
            }
        }

        // Random Button
        if (GUI.Button(new Rect(sX(0.875f), sY(0.01f), sX(0.09f), sY(0.15f)), "", GUIStyle.none) && timeSinceLastClick > clickDelay)//clickDelay)//, GUIStyle.none))
        {
            randomButtonTimer = 0.0f;
            sprite_RandomOn.enabled = true;
            sprite_RandomOff.enabled = false;
            avionicsFaultPage.SetRandomFaults();
            if (sndOK.isPlaying == false) sndOK.Play();
            timeSinceLastClick = 0.0f;
        }

        
        
        // Quit Button
        if (GUI.Button(new Rect(sX(0.88f), sY(0.84f), sX(0.08f), sY(0.15f)), "", GUIStyle.none) && timeSinceLastClick > clickDelay)//clickDelay)//, GUIStyle.none))
        {
            quitButtonTimer = 0.0f;
            sprite_QuitOn.enabled = true;
            sprite_QuitOff.enabled = false;
            if (sndOK.isPlaying == false) sndOK.Play();
            timeSinceLastClick = 0.0f;
            SceneManager.LoadScene("10_Main Menu");
        }

        // MSG BUTTON
        if (GUI.Button(new Rect(sX(0.35f), sY(topRow), sX(xScale), sY(yScale)), "", GUIStyle.none))
        {
            if (Msg_On == true)
            {
                ClearNextStatus();
                if (sndOK.isPlaying == false) sndOK.Play();
                timeSinceLastClick = 0.0f;
            }

            if (Msg_On == false)
            {
                if (sndOK.isPlaying == false) sndOK.Play();
                // ADD CODE TO SWITCH TO MESSAGE PAGE
            }
            /*if (statusLastMessage < 4)
                {

                    statusLastMessage++;
                    if (sndOK.isPlaying == false) sndOK.Play();
                    if (statusLastMessage == 4) Msg_On = false;
                }
                else
                {

                    statusCurrentMessage = 0;
                    statusNotSimulatedTimer = 1.0f;
                    if (sndFAIL.isPlaying == false) sndFAIL.Play();
                }
             */
        }


            // STS Button
            if (GUI.Button(new Rect(sX(0.555f), sY(topRow), sX(xScale), sY(yScale)), "", GUIStyle.none))
            {
                statusNotSimulatedTimer = 1.0f;
                if (sndFAIL.isPlaying == false) sndFAIL.Play();
                timeSinceLastClick = 0.0f;
            }

            if (GUI.Button(new Rect(sX(0.63f), sY(0.07f), sX(xScale * 1.3f), sY(yScale * 1.5f)), "", GUIStyle.none))
            {
                timeSinceLastClick = 0.0f;
                displayTurnedOn = !displayTurnedOn;
                displayBrightnessTimer = 0.0f;
            }


        

        // Menu processing
        {
            if (helpDisplayEnabled == true) return;                                     // Disable controls if help page is up
            if (mcdCurrentMode == Mode.Quit)
            {
                if (displayTurnedOn == false)
                {
                    timeSinceLastClick = 0.0f;
                    displayTurnedOn = !displayTurnedOn;
                    displayBrightnessTimer = 0.0f;
                }
                mcd_UI.gui_Quit();
            }
            if (displayTurnedOn == false)
            {
                mcd_UI.gui_MCD_Off();
                return;                         // Visually disable controls if display is turned off
            }

            // TODO: Change to a switch later on
            if (mcdCurrentMode == Mode.Comm_Message) mcd_UI.gui_Comm_Message();                     // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_Main) mcd_UI.gui_CommMain();        // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_HF) mcd_UI.gui_CommHF();        // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_Index_1) mcd_UI.gui_CommIndex1();        // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_Index_2) mcd_UI.gui_CommIndex2();        // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_EmCom) mcd_UI.gui_Comm_EmCom();        // GOOD V3
            else if (mcdCurrentMode == Mode.CommMaint) mcd_UI.gui_CommMaint();          // GOOD V3  
            else if (mcdCurrentMode == Mode.CommFault) mcd_UI.gui_CommFault();          // GOOD V3  
            else if (mcdCurrentMode == Mode.CommFaultHist) mcd_UI.gui_CommFaultHist();  // GOOD V3
            else if (mcdCurrentMode == Mode.Comm_Status) mcd_UI.gui_CommStatus();        // GOOD V3

            else if (mcdCurrentMode == Mode.Msn_Index_1) mcd_UI.gui_MsnMain();                 // GOOD V3
            else if (mcdCurrentMode == Mode.Msn_Index_2) mcd_UI.gui_Msn2Main();               // GOOD V3
            else if (mcdCurrentMode == Mode.Msn_EGT_Overtemp) mcd_UI.gui_Msn_EGTovertemp();               // GOOD V3
            //TODO: THERE IS A BUG WHERE OCCASIONALLY YOU GET AN EXTRA PAGE THAT IS BLANK WITH INCORRECT FORMATTING
            //  RECOMPILING MID SIMULATION ALSO BREAKS THIS PAGE
            else if (mcdCurrentMode == Mode.Msn_AvionicsFaults) mcd_UI.gui_AVfaults();                  // GOOD V3
            else if (mcdCurrentMode == Mode.Msn_NonAvionicsFaults) mcd_UI.gui_NonAv();
            //TODO: DISABLED, PAGE NOT COMPLETE
            else if (mcdCurrentMode == Mode.Msn_Maint) mcd_UI.gui_Maint();
            else if (mcdCurrentMode == Mode.Msn_Maint_MCEDS) mcd_UI.gui_MCEDS();
            else if (mcdCurrentMode == Mode.Msn_MCEDS_ADPMC2) mcd_UI.gui_Maint_APDMC2();

            else if (mcdCurrentMode == Mode.Maint_APDMS2) mcd_UI.gui_Maint_APDMC2();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Main) mcd_UI.gui_Maint_APDMC2_Faults_Main();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_List) mcd_UI.gui_Maint_APDMC2_Faults_List();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Propulsion) mcd_UI.gui_Maint_APDMC2_Faults_Prop();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng1) mcd_UI.gui_Maint_APDMC2_Faults_Eng1();        // These menus are functionally identical, use the same function
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng2) mcd_UI.gui_Maint_APDMC2_Faults_Eng1();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng3) mcd_UI.gui_Maint_APDMC2_Faults_Eng1();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Faults_Eng4) mcd_UI.gui_Maint_APDMC2_Faults_Eng1();

            // APDMC2 - EGT Overtemp History
            else if (mcdCurrentMode == Mode.Maint_APDMS2_Egt_Main) mcd_UI.gui_Maint_APDMC2_EgtOvertemp_Main();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng1) mcd_UI.gui_Maint_APDMC2_EgtOvertemp_Eng1();  // These menus are functionally identical, use the same function
            else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng2) mcd_UI.gui_Maint_APDMC2_EgtOvertemp_Eng1();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng3) mcd_UI.gui_Maint_APDMC2_EgtOvertemp_Eng1();
            else if (mcdCurrentMode == Mode.Maint_APDMS2_EgtHist_Eng4) mcd_UI.gui_Maint_APDMC2_EgtOvertemp_Eng1();

            // EFCS - Flight controls faults
            else if (mcdCurrentMode == Mode.Maint_EFCS_Main) mcd_UI.gui_EFCS_Main();
            else if (mcdCurrentMode == Mode.Maint_EFCS_Fault_List) mcd_UI.gui_EFCS_FaultList();     // Both are functionally identical for now
            else if (mcdCurrentMode == Mode.Maint_EFCS_Fault_History) mcd_UI.gui_EFCS_FaultList();


        }
    }

    ///////////////////////// START DISPLAY CODE /////////////////////
    void disp_Comm_Message()        // Display empty comm message page
    {
        currentDisp = stringStorage.comm_Msg_Summary.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_CommMain()            // Display comm main menus, varies based on HF power
    {
        if (!globalAccess.misc_HF1_On && !globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_Main_HF_Off.ToString();
        else if (globalAccess.misc_HF1_On && !globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_Main_HF_1_On.ToString();
        else if (!globalAccess.misc_HF1_On && globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_Main_HF_2_On.ToString();
        else currentDisp = stringStorage.comm_Main_HF_On.ToString();
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_CommHF()
    {
        if (!globalAccess.misc_HF1_On && !globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_HF_Off.ToString();
        else if (globalAccess.misc_HF1_On && !globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_HF_1_On.ToString();
        else if (!globalAccess.misc_HF1_On && globalAccess.misc_HF2_On) currentDisp = stringStorage.comm_HF_2_On.ToString();
        else currentDisp = stringStorage.comm_HF_On.ToString();

        
        currentDisp += GenerateLastLine();
        currentDisp += "1/2";

        currentDispRev = "";
    }

    void disp_CommIndex1()          // Display comm index page 1
    {
        currentDisp = stringStorage.comm_Index1.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();
        currentDisp += "1/2";
        
        currentDispRev = "";
    }

    void disp_CommIndex2()          // Display comm index page 2  
    {
        currentDisp = stringStorage.comm_Index2.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();
        currentDisp += "2/2";

        currentDispRev = "";
    }

    void disp_Comm_EmCom()
    {
        currentDisp =
            " NAV EMITTER CONTROL 1  " + "\n" +
            " RAD ALT 1     RAD ALT 2" + "\n";
        if (!globalAccess.misc_Rad1_On && !globalAccess.misc_Rad2_On) currentDisp += ">STANDBY        STANDBY<" + "\n";
        else if (globalAccess.misc_Rad1_On && !globalAccess.misc_Rad2_On) currentDisp += ">ON             STANDBY<" + "\n";
        else if (!globalAccess.misc_Rad1_On && globalAccess.misc_Rad2_On) currentDisp += ">STANDBY             ON<" + "\n";
        else currentDisp += ">ON                  ON<" + "\n";

        currentDisp +=
            " DME1              DME2 " + "\n" +
            ">STANDBY        STANDBY<" + "\n" +
            " WX RADAR           SKE " + "\n" +
            ">OFF            STANDBY<" + "\n" +
            "TACAN                   " + "\n" +
            ">STANDBY                " + "\n" +
            "AERO-I                  " + "\n" +
            ">STANDBY                " + "\n" +
            "                        " + "\n" +
            "                 RETURN>";
        currentDisp += GenerateLastLine();
        currentDisp += "1/2";

    }

    void disp_CommMaint()           // Display comm maintenance page
    {
        currentDisp = stringStorage.comm_Maint.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_CommFault()           // Display comm faults based on CCU failure scenario
    {
            currentDisp =
        "     comm fault list    " + "\n" +
        "                        " + "\n";

        if (globalAccess.fault_CCU1 == true) currentDisp += 
        "CCU 1                   " + "\n";

        else currentDisp += 
        "                        " + "\n";

        currentDisp += 
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";

        currentDisp += GenerateLastLine();
        
        currentDispRev = "";
    }

    void disp_CommFaultHist()       // Display fault history, based on CCU status
    {
        currentDisp =
            "   comm fault history   " + "\n" +
            " lru      gmt/date  flt " + "\n";

        if (globalAccess.fault_CCU1 == true) currentDisp += 
                "ccu 1     1342/060419 1 " + "\n";
        else currentDisp +=
                "                        " + "\n";

        currentDisp +=
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "*erase all?      return>";

        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }


    void disp_CommStatus()          // Comm status page, based on CCU status
    {
        currentDisp =
            "   COMM SYSTEM STATUS   " + "\n" +
            "PROG             MMP WOW" + "\n" +
            " 8.3             ON  ON " + "\n" +
            "BKUP: AUDIO    PLT  CPLT" + "\n" +
            "      OFF      OFF  OFF " + "\n" +
            "I/PHONE: AR  MAINT   C/M" + "\n" +
            "         OFF OFF     OFF" + "\n" +
            "BUS CTRL: BUS1 BUS2 MCD " + "\n";

        if (globalAccess.fault_CCU1 == false) currentDisp += "          CCU1 CCU2 CCU1" + "\n";
        else currentDisp += "          CCU2 CCU2 CCU2" + "\n";

        currentDisp +=
            "NORM/ISO                " + "\n" +
            "ISO                     " + "\n" +
            "IFF ZERO                " + "\n" +
            "ON               RETURN>";

        currentDisp += GenerateLastLine();
        
        currentDispRev = "";
    }

    void MsnIndex1()
    {
        if (globalAccess.misc_IRUaligned == true)
        {
            currentDisp =
            "       msn index 1      " + "\n" +
            "                        " + "\n" +
            "<MSN INIT               " + "\n" +
            "                        " + "\n" +
            "<NAV SENSOR    APPROACH>" + "\n" +
            "                        " + "\n" +
            "               A/D MENU>" + "\n" +
            "                        " + "\n" +
            "               R/Z MENU>" + "\n" +
            "                        " + "\n" +
            "<FUEL PLAN     AOC CONV>" + "\n" +
            "                        " + "\n" +
            "               ATS MENU>";

            currentDisp += GenerateLastLine();
            currentDisp += "1/2";



            currentDispRev =
            "                        " + "\n" +
            "                        " + "\n" +
            "              TOLD MENU>" + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<ROUTE DATA             " + "\n" +
            "                        " + "\n" +
            "<WT-CG                  " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<GPWS/TAWS              " + "\n" +
            "                        ";
        }       // MSN Index 1 IRUs aligned
        else                             
        {
            currentDisp =
            "       msn index 1      " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "               APPROACH>" + "\n" +
            "                        " + "\n" +
            "               A/D MENU>" + "\n" +
            "                        " + "\n" +
            "               R/Z MENU>" + "\n" +
            "                        " + "\n" +
            "<FUEL PLAN     AOC CONV>" + "\n" +
            "                        " + "\n" +
            "               ATS MENU>";
            currentDisp += GenerateLastLine();
            currentDisp += "1/2";

            
            currentDispRev =
            "                        " + "\n" +
            "                        " + "\n" +
            "<MSN INIT     TOLD MENU>" + "\n" +
            "                        " + "\n" +
            "<NAV SENSOR             " + "\n" +
            "                        " + "\n" +
            "<ROUTE DATA             " + "\n" +
            "                        " + "\n" +
            "<WT-CG                  " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "<GPWS/TAWS              " + "\n" +
            "                        ";   
        }
    }

    void MsnIndex2()
    {
        currentDisp =
        "      msn index 2       " + "\n" +
        " avionic                " + "\n" +
        "<FAULT LIST   PERF FACT>" + "\n" +
        " non-avionic            " + "\n" +
        "<FAULT LIST    MC ERASE>" + "\n" +
        " printer      permanent " + "\n" +
        "<SELECT        DATABASE>" + "\n" +
        "                 custom " + "\n" +
        "<EGT OVERTEMP  DATABASE>" + "\n" +
        " database       pos/alt " + "\n" +
        "<LOADING    CONVERSIONS>" + "\n" +
        "                        " + "\n" +
        "<MAINTENANCE      CLEAR>";

        currentDisp += GenerateLastLine();
        currentDisp += "2/2";

        currentDispRev = "";

    }                   // MSN Index 2 block 21

    void disp_Maint()               // Msn maintenance page
    {
        currentDisp = stringStorage.msn_maint.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
        /*string txtString =
        "    maintenance menu    " + "\n" +
        "                        " + "\n" +
        "<MC/EDS             CMU>" + "\n" +
        "                        " + "\n" +
        "<EFCS        SW VERSION>" + "\n" +
        "                        " + "\n" +
        "<SC/EFCS                " + "\n" +
        "                        " + "\n" +
        " A/PDMS 1               " + "\n" +
        "                        " + "\n" +
        "<A/PDMS 2               " + "\n" +
        "                        " + "\n" +
        "<DEF SYSTEM      return>" + "\n" +
        ">" + statusMessages[statusCurrentMessage];

        for (int i = 0; i < 20 - statusMessages[statusCurrentMessage].Length; i++)
        {
            txtString += " ";
        }
        txtString += "   ";

        currentDisp = txtString;

        txtString = "";
        currentDispRev = txtString;*/
    }

    void disp_Msn_EGTOvertemp()     // EGT Overtemp
    {
        currentDisp = stringStorage.Msn_EgtOvertemp.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_MCEDS()
    {
        currentDisp =
        "   mc/eds maint menu    " + "\n" +
        "                        " + "\n" +
        "<FAULT LIST             " + "\n" +
        "                        " + "\n" +
        "<FAULT HISTORY          " + "\n" +
        "                        " + "\n" +
        "<INITIATED BIT          " + "\n" +
        "              operating " + "\n" +
        "<SFO           3678.3hrs" + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";

        currentDisp += GenerateLastLine();
        
        currentDispRev = "";
    }           // Display MC/EDS maintenance menu

    void disp_MCEDS_Fault()
    {
        currentDisp =
        "   ms/eds fault list    " + "\n" +
        "                        " + "\n" +
        "(fault 1)               " + "\n" +
        "                        " + "\n" +
        "(fault 2)               " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";

        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }       
    
    // Display MC/EDS fault listing

    /// <summary>
    /// Display MC/EDS fault history
    /// </summary>
    void disp_MCEDS_FaultHist()
    {
        currentDisp =
        "  mc/eds fault history  " + "\n" +
        "lru       gmt/date  flt " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "*erase all       return>";

        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display EFCS maintenance menu
    /// </summary>
    void disp_EFCS_Maint()
    {
        currentDisp =
        "    efcs maint menu     " + "\n" +
        "                        " + "\n" +
        "<FAULT LIST             " + "\n" +
        "                        " + "\n" +
        "<FAULT HISTORY          " + "\n" +
        "                        " + "\n" +
        "<MAINTENANCE BIT        " + "\n" +
        "                        " + "\n" +
        "<RIG                    " + "\n" +
        "                        " + "\n" +
        "<LRU INITIALIZATION     " + "\n" +
        "                        " + "\n" +
        "<MEMORY INSPECT   return>";

        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display EFCS fault listing
    /// </summary>
    void disp_EFCS_Fault()
    {
        currentDisp =
        "   efcs fault list      " + "\n" +
        "                        " + "\n" +
        "(SHOULD HAVE ECO FAULTS)" + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";

        currentDisp += GenerateLastLine();
        currentDisp += "1/1";

        currentDispRev = "";
    }

    /// <summary>
    /// Display EFCS fault history
    /// </summary>
    void disp_EFCS_FaultHist()
    {
        currentDisp =
        "  efcs fault history    " + "\n" +
        "lru       gmt/date  flt " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "*erase page      return>"; currentDisp += GenerateLastLine();
        currentDisp += "1/1";

        currentDispRev = "";
    }

    /// <summary>
    /// Display SCEFC maintenance menu
    /// </summary>
    void disp_SCEFC_Maint()
    {
        currentDisp =
        "    efcs maint menu     " + "\n" +
        "                        " + "\n" +
        "<FAULT LIST             " + "\n" +
        "                        " + "\n" +
        "<FAULT HISTORY          " + "\n" +
        "                        " + "\n" +
        "<MAINTENANCE BIT        " + "\n" +
        "                        " + "\n" +
        "<RIG                    " + "\n" +
        "                        " + "\n" +
        "<LRU INITIALIZATION     " + "\n" +
        "                        " + "\n" +
        "<MEMORY INSPECT   return>";
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display SCEFC fault listing
    /// </summary>
    void disp_SCEFC_Fault()
    {
        currentDisp =
        "   scefc fault list      " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                 return>";
        currentDisp += GenerateLastLine();
        currentDisp += "1/1";

        currentDispRev = "";
    }

    /// <summary>
    /// Display SCEFC fault history
    /// </summary>
    void disp_SCEFC_FaultHist()
    {
        currentDisp =
        "  scefc fault history   " + "\n" +
        "lru       gmt/date  flt " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "*erase page      return>";
        currentDisp += GenerateLastLine();
        currentDisp += "1/1";

        currentDispRev = "";
    }

    /// <summary>
    /// Display APDMC2 maintenance menu
    /// </summary>
    void disp_APDMC2_Maint()
    {
        currentDisp = stringStorage.msn_maint_apdms2.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display APDMC2 fault listing
    /// </summary>
    void disp_APDMC2_Fault_Main()
    {
        currentDisp = stringStorage.msn_maint_apdms2_fault_main.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_List()
    {
        currentDisp = stringStorage.msn_maint_apdms2_fault_list.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_Propulsion()
    {
        currentDisp = stringStorage.msn_maint_apdms2_faults_prop2.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_Propulsion_Eng1()
    {
        currentDisp = stringStorage.msn_maint_apdms2_faults_prop2_eng1.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_Propulsion_Eng2()
    {
        currentDisp = stringStorage.msn_maint_apdms2_faults_prop2_eng2.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_Propulsion_Eng3()
    {
        currentDisp = stringStorage.msn_maint_apdms2_faults_prop2_eng3.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_Fault_Propulsion_Eng4()
    {
        currentDisp = stringStorage.msn_maint_apdms2_faults_prop2_eng4.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_EgtHistory_Main()
    {
        currentDisp = stringStorage.msn_maint_apdms2_EgtHist.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_EgtHistory_Eng1()
    {
        currentDisp = stringStorage.msn_maint_apdms2_EgtHist_Eng1.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_EgtHistory_Eng2()
    {
        currentDisp = stringStorage.msn_maint_apdms2_EgtHist_Eng2.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_EgtHistory_Eng3()
    {
        currentDisp = stringStorage.msn_maint_apdms2_EgtHist_Eng3.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_APDMC2_EgtHistory_Eng4()
    {
        currentDisp = stringStorage.msn_maint_apdms2_EgtHist_Eng4.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display APDMC2 fault history
    /// </summary>
    void disp_APDMC2_FaultHist()
    {
        currentDisp = stringStorage.msn_maint_apdms2_fault_list.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_EFCS_Main()
    {
        currentDisp = stringStorage.msn_maint_EFCS_main.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_EFCS_FaultList()
    {
        currentDisp = stringStorage.msn_maint_EFCS_FaultList.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    void disp_EFCS_FaultHistory()
    {
        currentDisp = stringStorage.msn_maint_EFCS_FaultHistory.ToString();        // Pull text from text file, we can't read because HTML5 blocks this
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Generates avionics fault list based on current faults
    /// </summary>
    /// <param name="p"></param>
    void genAvFaults(int p)
    {
        //if(debugMode==true)
        avionicsFaultPage.UpdatePages();                // *** Holy crap will this be inefficient, should only be called if faults have been modified at runtime only used for debug testing. But here we are.***

        avionicsFaults_TotalPages = avionicsFaultPage.totalPages;

        if (p > avionicsFaultPage.totalPages) {
            p = 1;
            avionicsFaults_CurrentPage = avionicsFaultPage.totalPages;
        }


        int startIndex = (p-1) * 6;
        int endIndex = ((p-1) * 6) + 6;


        currentDisp =
        "   avionics fault list  " + "\n" +
        "                        " + "\n";
        
        for(int i = startIndex; i< endIndex; i++)
        {
            currentDisp += avionicsFaultPage.faults[i];
            

            // Status line
            if (i == startIndex)
            {
                for (int j = 0; j< 17 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "STATUS<";
            }

            // Ethernet line
            if (i == startIndex + 1)
            {
                for (int j = 0; j < 15 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "ETHERNET<";
            }

            // Return line
            if (i == startIndex + 5)
            {
                for (int j = 0; j < 17 - avionicsFaultPage.faults[i].Length; j++)
                {
                    currentDisp += " ";
                }
                currentDisp += "RETURN<";
            }

            if (i < endIndex - 1) currentDisp += "\n\n";
        }
        


        currentDisp += GenerateLastLine();
        currentDisp += p + "/" + avionicsFaultPage.totalPages;

        currentDispRev = "";

        /*for(int i = 0; i < totalFaults; i++)
        {
            //if (currentFaults > 6) break;
            //if (faults[i].active == true)
            //{
                if (currentFaults > p * 6 && currentFaults <= (p * 6) + 6)
                {
                    
                if (currentFaults == (p * 6) + 6)
                {
                    txtString = txtString + faults[i].name;
                    for (int j = 0; j < 17 - faults[i].name.Length; j++)
                    {
                        txtString += " ";
                    }
                    txtString += "return<\n";
                

                }
                    else txtString = txtString + faults[i].name + "\n";

                    if (currentFaults != (p * 6) + 6)
                    {
                        txtString = txtString + "\n";
                    }
                }
                currentFaults++;
            //}
        }

        if (p == avionicsFaults_TotalPages)
        {
            for (int i = 0; i < 6 - (totalFaults % 6); i++)
            {
                if (i < (6 - (totalFaults % 6))-1) txtString += "\n\n";
                else txtString +=
                        "\n\n" +
                        "                 return<" + "\n";
            }
            txtString += "";
        }

        txtString += GenerateLastLine();

        txtString += p.ToString() + "/" + avionicsFaults_TotalPages.ToString();
        currentDisp = txtString;*/
    }

    /// <summary>
    /// Display non-avionics faults
    /// </summary>
    void disp_NonAv()
    {
        if (nonAvionicsFaults_CurrentPage == 1)
        {
            currentDisp =
        " non-avionics fault list" + "\n" +
        "                        " + "\n" +
        "APM-A1APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A2APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A3APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-A4APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B1APMRXND           " + "\n" +
        "                        " + "\n" +
        "APM-B2APMRXND    return>";
        }
        else
        {
            if (globalAccess.fault_BATT == false)
            {
                currentDisp =
            " non-avionics fault list" + "\n" +
            "                        " + "\n" +
            "APM-B3APMRXND           " + "\n" +
            "                        " + "\n" +
            "APM-B4APMRXND           " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                 return>";
            }
            else
            {
                currentDisp =
            " non-avionics fault list" + "\n" +
            "                        " + "\n" +
            "APM-B3APMRXND           " + "\n" +
            "                        " + "\n" +
            "APM-B4APMRXND           " + "\n" +
            "                        " + "\n" +
            "WCC-BAT CHG 1           " + "\n" +
            "                        " + "\n" +
            "WCC-BAT 1               " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                        " + "\n" +
            "                 return>";
            }
        }
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    /// <summary>
    /// Display exit confirmation
    /// </summary>
    void disp_Quit()
    {
        currentDisp =
        " ARE YOU SURE YOU WANT  " + "\n" +
        "      TO QUIT?          " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "                        " + "\n" +
        "<YES                 NO>";
        currentDisp += GenerateLastLine();

        currentDispRev = "";
    }

    //////////////// Single use functions ///////////////
    void CheckHFStatus()
    {
        if(HF1_LastStatus != globalAccess.misc_HF1_On && globalAccess.misc_HF1_On)
        {
            sndHFchime.Play();
        }
        if (HF2_LastStatus != globalAccess.misc_HF2_On && globalAccess.misc_HF2_On)
        {
            sndHFchime.Play();
        }

        HF1_LastStatus = globalAccess.misc_HF1_On;
        HF2_LastStatus = globalAccess.misc_HF2_On;
    }

    ///////////////// Status light functions ////////////////////

    
    void SetStatusLight()       // Sets the status light
    {
        // Update message light status, should only be done once per frame
        if (Msg_On == false)
        {
            msg_btn_off.enabled = false;
            msg_btn_on.enabled = true;
        }
        else
        {
            msg_btn_off.enabled = true;
            msg_btn_on.enabled = false;
        }
    }

    // Clears the status in a clumsy way
    void ClearNextStatus()
    {
        if (globalAccess.stat_NotSim == true)       // If not sim message is shown, ignore response.
        {
            return;
        }

        else if (globalAccess.stat_DBmismatch == true)
        {
            globalAccess.stat_DBmismatch = false;
            return;
        }
        else if (globalAccess.stat_ChMast == true)
        {
            globalAccess.stat_ChMast = false;
            return;
        }
        else if (globalAccess.stat_datalink == true)
        {
            globalAccess.stat_datalink = false;
            return;
        }

    }

    // Gets our status string, called when rendering the MCD page
    string UpdateStatus()
    {
        if (globalAccess.stat_NotSim == true)
        {
            Msg_On = true;
            return "NOT SIMULATED";
        }

        else if (globalAccess.stat_DBmismatch == true)
        {
            Msg_On = true;
            return "COMM DB MISCOMPARE";
        }
        else if (globalAccess.stat_ChMast == true)
        {
            Msg_On = true;
            return "CHANGE MASTER";
        }
        else if (globalAccess.stat_datalink == true)
        {
            Msg_On = true;
            return "NO HF DATALINK";
        }
        else
        {
            Msg_On = false;
            return "(              )";
        }
    
    }


    /// <summary>
    /// Generates the last line of the MCD based on scratch pad status
    /// </summary>
    /// <returns>A string with the last line, either a fault or a typing prompt</returns>
    string GenerateLastLine()
    {
        string lastLine;
        string statusMessage = UpdateStatus();
        lastLine = "\n" + statusMessage;

        for (int i = 0; i < 21 - statusMessage.Length; i++)
        {
            lastLine += " ";
        }
        return (lastLine);
    }

    // Flips a bool coin, true or false. Should be 50/50?
    bool CoinFlip()
    {   
        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) return true;
        else return false;
    }
    
    /// <summary>
    /// sX(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">X position from left to right</param>
    /// <returns></returns>    
    public float sX(float i)
    {
        i = (float)Screen.width * i;
        return i;
    }

    /// <summary>
    /// sY(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">Y position from top to bottom</param>
    /// <returns></returns>    
    public float sY(float i)
    {
        i = (float)Screen.height * i;
        return i;
    }
}

