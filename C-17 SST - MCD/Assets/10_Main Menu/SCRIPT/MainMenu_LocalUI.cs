using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           // Needed to interface with pre-fabbed UI items
using UnityEngine.SceneManagement;

public class MainMenu_LocalUI : MonoBehaviour {

    

    public GameObject GlobalDebug;  // Default Prefab for g setup
    GameObject GlobalObject;        // Used to access gs
    public Globals g;               // Globals are bad, blah, blah, blah (Used to save variables between scenes

    LoadingMaster loadscreen;

    public Text text_title;

    public Text text_L1;
    public Text text_L2;
    public Text text_L3;
    public Text text_L4;
    public Text text_L5;

    public Text text_R1;
    public Text text_R2;
    public Text text_R3;
    public Text text_R4;
    public Text text_R5;

    public Text text_brief;

    public GUIStyle GUI_brief;

    AudioSource AvOff;

    enum Menus
    {
        Main,
        Help,
        About,
        Exit,

        ICS_Ops_Brief,

        MCD_Main,
        MCD_Config_Status,
        MCD_Config_Fault,
        MCD_Config_Msg,
        MCD_Brief_All,
        MCD_Brief_Random,
        MCD_Brief_Preset,

        Refuel_Main,
        Refuel_Config,
        Refuel_Fault,
        Refuel_Brief_Max,
        Refuel_Brief_Rampload,
        Refuel_Brief_Empty
    }

    Menus CurrentMenus = Menus.Main;
    Menus LastMenus = Menus.Main;

    float MenusTimer = -1.0f;

    bool directLoad = false;

    // Use this for initialization
    void Start ()
    {

        try
        {
            loadscreen = GameObject.Find("~Loading Screen Essentials").GetComponent<LoadingMaster>();
        }
        catch
        {
            Debug.Log("PickleSticks: Couldn't find the load screen, using direct loading");
            directLoad = true;
        }

        try
        {
            GlobalObject = GameObject.Find("~Globals");
            g = GlobalObject.GetComponent<Globals>();
        }
        catch
        {
            Debug.Log("CAUTION: Global variables not initialized, loading defaults.");
            GlobalObject = Instantiate(GlobalDebug);
            g = GlobalObject.GetComponent<Globals>();
        }

    }
	
	// Update is called once per frame
	void Update () {
        GUI_brief.fontSize = (int)(Screen.height * 0.050f);
        MenusTimer += Time.deltaTime;
	}

    private void OnGUI()
    {
        if (CurrentMenus == Menus.Main) DrawMenus_Main();

        else if (CurrentMenus == Menus.MCD_Main) DrawMenus_MCD_Main();
        else if (CurrentMenus == Menus.MCD_Config_Status) DrawMenus_MCD_Status();
        else if (CurrentMenus == Menus.MCD_Config_Fault) DrawMenus_MCD_Faults();
        else if (CurrentMenus == Menus.MCD_Config_Msg) DrawMenus_MCD_Message();
        else if (CurrentMenus == Menus.MCD_Brief_All) DrawMenus_MCD_Brief_All();
        else if (CurrentMenus == Menus.MCD_Brief_Random) DrawMenus_MCD_Brief_Random();
        else if (CurrentMenus == Menus.MCD_Brief_Preset) DrawMenus_MCD_Brief_Preset();

        else if (CurrentMenus == Menus.ICS_Ops_Brief) DrawMenus_ICS_Brief();
    }

    void DrawMenus_Main()
    {
        text_title.text = "C-17 SINGLE SYSTEM TRAINER";

        text_L1.text = ">MCD";
        text_L2.text = ">ICS";
        if (g.demo_mode) text_L3.text = ">REFUEL (IN DEVELOPMENT)";
        else text_L3.text = "";
        text_L4.text = "";
        text_L5.text = "";
        text_L4.text = " ";
        text_L5.text = "Vote in AFWERX!\nsee manual for link!";

        if (g.demo_mode) text_R1.text = "OUTRO<";
        else text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        if (Application.platform.ToString() == "WebGLPlayer") text_R3.text = "DEBUG NOTE:";
        text_R4.text = "";
        if(Application.platform.ToString() == "WebGLPlayer") text_R4.text = "Alt-F4 to quit in browser";
        text_R5.text = "QUIT<";
    }           // GOOD

    /// <summary>
    /// Draws the MCD main menu
    /// </summary>
    void DrawMenus_MCD_Main()
    {
        ClearText();

        text_title.text = "NUISENCE FAULT TRAINER";

        text_L1.text = ">ALL FAULTS";
        text_L2.text = ">RANDOM FAULTS";
        text_L3.text = ">CUSTOM FAULTS";
        
        text_L5.text = ">BACK";

        
    }

    void DrawMenus_MCD_Status()
    {
        text_title.text = "MCD - STATUS CONFIGURATION";

        if(g.misc_EFCSreset == true) text_L1.text = ">EFCS RESET: ON";
        else text_L1.text = ">EFCS RESET: OFF";

        if (g.misc_IRUaligned == true) text_L2.text = ">IRU: ALIGNED";
        else text_L2.text = ">IRU: OFF";

        if(g.misc_Rad1_On == true) text_L3.text = ">RADAR ALT: T/R";
        else text_L3.text = ">RADAR ALT: STANDBY";

        if(g.misc_HF1_On == true) text_L4.text = ">HF RADIO: T/R";
        else text_L4.text = ">HF RADIO: STANDBY";
        text_L5.text = ">BACK";

        text_R1.text = "STAT ";
        text_R2.text = "FAULT<";
        text_R3.text = "MSG<";
        text_R4.text = "";
        text_R5.text = "START<";
    }


    void DrawMenus_MCD_Faults()
    {
        text_title.text = "MCD - FAULT CONFIGURATION";

        if(g.fault_BATT == true) text_L1.text = ">BATTERY 1 FAIL";
        else text_L1.text = ">BATTERY 1 OKAY";

        if(g.fault_CCU1 == true) text_L2.text = ">CCU FAIL";
        else text_L2.text = ">CCU OKAY";

        if (g.fault_OBIGGS == true) text_L3.text = ">MFDC FAIL";
        else text_L3.text = ">MFDC OKAY";

        if (g.fault_TAWS == true) text_L4.text = ">VIP FAIL";
        else text_L4.text = ">VIP OKAY";

        text_L5.text = ">BACK";

        text_R1.text = "STAT<";
        text_R2.text = "FAULT ";
        text_R3.text = "MSG<";
        text_R4.text = "";
        text_R5.text = "START<";
    }


    void DrawMenus_MCD_Message()
    {
        text_title.text = "MCD - MESSAGE CONFIGURATION";

        if (g.stat_DBmismatch == true) text_L1.text = ">DB MISMATCH: ON";
        else text_L1.text = ">DB MISMATCH: OFF";

        if (g.stat_ChMast == true) text_L2.text = ">CHG MASTER: ON";
        else text_L2.text = ">CHG MASTER: OFF";

        if (g.stat_datalink == true) text_L3.text = ">HF DATALINK: ON";
        else text_L3.text = ">HF DATALINK: OFF";
        text_L4.text = "";
        text_L5.text = ">BACK";

        text_R1.text = "STAT<";
        text_R2.text = "FAULT<";
        text_R3.text = "MSG ";
        text_R4.text = "";
        text_R5.text = "START<";
    }

    void DrawMenus_MCD_Brief_All()
    {
        text_title.text = "SCENARIO";
        text_L1.text = "";
        text_L2.text = "";
        text_L3.text = "";
        text_L4.text = "";
        text_L5.text = ">BACK";
        text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        text_R4.text = "";
        text_R5.text = "GO<";

        text_brief.text = "A C-17 has landed and requires a combined\n" +
            "preflight & postflight inspection (BPO)\n" +
            "Your task is to go through the avionics\n" +
            "fault list.\n" +
            "\n" +
            "(All faults enabled, send it!)\n" +
            "\n" +
            "\n" +
            "GOAL:\n" +
            "Find and identify reportable & nuisance faults.";
    }
    void DrawMenus_MCD_Brief_Random()
    {
        text_title.text = "SCENARIO";
        text_L1.text = "";
        text_L2.text = "";
        text_L3.text = "";
        text_L4.text = "";
        text_L5.text = ">BACK";
        text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        text_R4.text = "";
        text_R5.text = "GO<";

        text_brief.text = "A C-17 has landed and requires a combined\n" +
            "preflight & postflight inspection (BPO)\n" +
            "Your task is to go through the avionics\n" +
            "fault list.\n" +
            "\n" +
            "(Faults will be randomized)\n" +
            "\n" +
            "\n" +
            "GOAL:\n" +
            "Find and identify reportable & nuisance faults.   \n";
    }
    void DrawMenus_MCD_Brief_Preset()
    {
        text_title.text = "SCENARIO";
        text_L1.text = "";
        text_L2.text = "";
        text_L3.text = "";
        text_L4.text = "";
        text_L5.text = ">BACK";
        text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        text_R4.text = "";
        text_R5.text = "GO<";

        text_brief.text = "A C-17 has landed and requires a combined\n" +
            "preflight & postflight inspection (BPO)\n" +
            "Your task is to go through the avionics\n" +
            "fault list.\n" +
            "\n" +
            "(Faults will be based on your settings)\n" +
            "\n" +
            "\n" +
            "GOAL:\n" +
            "Find and identify reportable & nuisance faults.";
    }

    void DrawMenus_ICS_Brief()
    {
        text_title.text = "ICS PANEL SIMULATOR";
        text_L1.text = "";
        text_L2.text = "";
        text_L3.text = "";
        text_L4.text = "";
        text_L5.text = ">BACK";
        text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        text_R4.text = "";
        text_R5.text = "GO<";

        text_brief.text = "Objective:\n" +
            "Operate a C-17 Interphone Control Set\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "\n" +
            "GOAL:\n" +
            "Operate the ICS panel per your technical orders\n";
    }
    
    void DrawMenus_Refuel_Main()
    {
        text_title.text = "REFUEL PANEL TRAINER";

        text_L1.text = ">CONFIGURATION";
        text_L2.text = ">FAULTS";


        text_L5.text = ">BACK";
    }

    void DrawMenus_Refuel_Config()
    {

    }

    void DrawMenus_Refuel_Fault()
    {

    }

    void DrawMenus_Brief_Max()
    {

    }

    void DrawMenus_Brief_Rampload()
    {

    }

    void DrawMenus_Brief_Empty()
    {

    }

    /// <summary>
    /// Clears all text boxes
    /// </summary>
    void ClearText()
    {
        text_title.text = "";

        text_L1.text = "";
        text_L2.text = "";
        text_L3.text = "";
        text_L4.text = "";
        text_L5.text = "";

        text_R1.text = "";
        text_R2.text = "";
        text_R3.text = "";
        text_R4.text = "";
        text_R5.text = "";

        text_brief.text = "";
    }
    

    public void Process_Button_L1()
    {
        if (CurrentMenus == Menus.Main) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Main) ChangeMenus(Menus.MCD_Brief_All);
        else if (CurrentMenus == Menus.MCD_Config_Status) g.misc_EFCSreset = !g.misc_EFCSreset;
        else if (CurrentMenus == Menus.MCD_Config_Fault) g.fault_BATT = !g.fault_BATT;
        else if (CurrentMenus == Menus.MCD_Config_Msg) g.stat_DBmismatch = !g.stat_DBmismatch;
    }
    public void Process_Button_L2()
    {
        if (CurrentMenus == Menus.Main) ChangeMenus(Menus.ICS_Ops_Brief);
        else if (CurrentMenus == Menus.MCD_Main) ChangeMenus(Menus.MCD_Brief_Random);
        else if (CurrentMenus == Menus.MCD_Config_Status) g.misc_IRUaligned = !g.misc_IRUaligned;
        else if (CurrentMenus == Menus.MCD_Config_Fault) g.fault_CCU1 = !g.fault_CCU1;
        else if (CurrentMenus == Menus.MCD_Config_Msg) g.stat_ChMast = !g.stat_ChMast;
        
    }
    public void Process_Button_L3()
    {
        if (CurrentMenus == Menus.MCD_Config_Status)
        {
            g.misc_Rad1_On = !g.misc_Rad1_On;
            g.misc_Rad2_On = g.misc_Rad1_On;
        }
        else if (CurrentMenus == Menus.Main && g.demo_mode == true)
        {
            SceneManager.LoadScene("40_Refuel Panel");
        }
        else if (CurrentMenus == Menus.MCD_Main) ChangeMenus(Menus.MCD_Config_Status);
        else if (CurrentMenus == Menus.MCD_Config_Fault) g.fault_OBIGGS = !g.fault_OBIGGS;
        else if (CurrentMenus == Menus.MCD_Config_Msg) g.stat_datalink = !g.stat_datalink;
    }
    public void Process_Button_L4()
    {
        if (CurrentMenus == Menus.MCD_Config_Status)
        {
            g.misc_HF1_On = !g.misc_HF1_On;
            g.misc_HF2_On = g.misc_HF1_On;
        }
        else if (CurrentMenus == Menus.MCD_Config_Fault) g.fault_TAWS = !g.fault_TAWS;
        
    }
    public void Process_Button_L5()
    {
        if (CurrentMenus == Menus.Main) Debug.Log("TODO: SST Help");
        else if (CurrentMenus == Menus.MCD_Main) ChangeMenus(Menus.Main);
        else if (CurrentMenus == Menus.MCD_Config_Fault) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Config_Msg) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Config_Status) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Brief_All) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Brief_Random) ChangeMenus(Menus.MCD_Main);
        else if (CurrentMenus == Menus.MCD_Brief_Preset) ChangeMenus(Menus.MCD_Config_Fault);
        else if (CurrentMenus == Menus.ICS_Ops_Brief) ChangeMenus(Menus.Main);

    }

    public void Process_Button_R1()
    {
        //if (CurrentMenus == Menus.Main) Debug.Log("TODO: Options?");
        if (CurrentMenus == Menus.MCD_Config_Fault) ChangeMenus(Menus.MCD_Config_Status);
        else if (CurrentMenus == Menus.MCD_Config_Msg) ChangeMenus(Menus.MCD_Config_Status);
        else if (CurrentMenus == Menus.Main && g.demo_mode == true)
        {
            SceneManager.LoadScene("95_Outro");
            AvOff.Play();
        }

    }
    public void Process_Button_R2()
    {
        if (CurrentMenus == Menus.MCD_Config_Status) ChangeMenus(Menus.MCD_Config_Fault);
        else if (CurrentMenus == Menus.MCD_Config_Msg) ChangeMenus(Menus.MCD_Config_Fault);
    }
    public void Process_Button_R3()
    {
        if (CurrentMenus == Menus.MCD_Config_Fault) ChangeMenus(Menus.MCD_Config_Msg);
        else if (CurrentMenus == Menus.MCD_Config_Status) ChangeMenus(Menus.MCD_Config_Msg);
    }
    public void Process_Button_R4()
    {
        //if (CurrentMenus == Menus.Main) ChangeMenus(Menus.MCD_Main);
    }
    public void Process_Button_R5()
    {
        if (CurrentMenus == Menus.Main)
        {
            if(Application.platform.ToString() == "WebGLPlayer")
            {
                Application.OpenURL("about:blank");
            }
            Application.Quit();
        }
        else if (CurrentMenus == Menus.MCD_Config_Status) ChangeMenus(Menus.MCD_Brief_Preset);
        else if (CurrentMenus == Menus.MCD_Config_Fault) ChangeMenus(Menus.MCD_Brief_Preset);
        else if (CurrentMenus == Menus.MCD_Config_Msg) ChangeMenus(Menus.MCD_Brief_Preset);
        else if (CurrentMenus == Menus.MCD_Brief_All)
        {
            g.EnableAll();
            if (directLoad) SceneManager.LoadScene("20_MCD");
            else loadscreen.LoadScreen("20_MCD");
        }
        else if (CurrentMenus == Menus.MCD_Brief_Random)
        {
            g.GenRandom();
            if (directLoad) SceneManager.LoadScene("20_MCD");
            else loadscreen.LoadScreen("20_MCD");
        }
        else if (CurrentMenus == Menus.MCD_Brief_Preset)
        {
            if (directLoad) SceneManager.LoadScene("20_MCD");
            else loadscreen.LoadScreen("20_MCD");
        }
        else if (CurrentMenus == Menus.ICS_Ops_Brief)
        {
            if (directLoad) SceneManager.LoadScene("30_ICS");
            else loadscreen.LoadScreen("30_ICS");
        }
    }
    
    /// <summary>
    /// Manages menus transistion
    /// </summary>
    /// <param name="m"></param>
    void ChangeMenus(Menus m)
    {
        text_brief.text = "";
        LastMenus = CurrentMenus;
        MenusTimer = -1.0f;
        CurrentMenus = m;
       return;
    }

    /// <summary>
    /// sX(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">X position from left to right</param>
    /// <returns></returns>    
    public float ScaleX(float i)
    {
        return (float)Screen.width * i;
    }

    /// <summary>
    /// sY(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">Y position from top to bottom</param>
    /// <returns></returns>    
    public float ScaleY(float i)
    {
        return (float)Screen.height * i;
    }
}
