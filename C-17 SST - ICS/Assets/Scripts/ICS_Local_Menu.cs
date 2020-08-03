using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ICS_Local_Menu : MonoBehaviour {

    Load_TitleScrolling loadscreen;

    // Is menus shown
    bool show_Main = false;
    bool show_Position = false;
    bool show_Faults = false;
    bool show_Options = false;

    // Modes
    bool DrawingStation = false;

    public ICS_Panel MainPanel;

    public GameObject Backup_Panel;
    public GameObject ICS_Panel;
    public Renderer ICS_Pilot;
    public Renderer ICS_CoPilot;
    public Renderer ICS_LACM;
    public Renderer ICS_RACM;
    public Renderer ICS_CrewRest;
    public Renderer ICS_Forward_Loadmaster;
    public Renderer ICS_Aft_Loadmaster;

    public ICS_Panel ics_panel;

    bool directLoad = false;
    
    // Use this for initialization
    void Start () {

        try
        {
            loadscreen = GameObject.Find("Loadscreen Script").GetComponent<Load_TitleScrolling>();
        }
        catch
        {
            Debug.Log("PickleSticks: Couldn't find the load screen, using direct loading");
            directLoad = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        if(DrawingStation) // Hide all buttons and UI, to show station and provide method of tapping to remove
        {
            if (GUI.Button(new Rect(ScaleX(0.0f), ScaleY(0.0f), ScaleX(1.0f), ScaleY(1.0f)), "", GUIStyle.none))
            {
                Backup_Panel.SetActiveRecursively(true);
                ICS_Panel.SetActiveRecursively(true);
                ICS_Pilot.enabled = false;
                ICS_CoPilot.enabled = false;
                ICS_LACM.enabled = false;
                ICS_RACM.enabled = false;
                ICS_CrewRest.enabled = false;
                ICS_Forward_Loadmaster.enabled = false;
                ICS_Aft_Loadmaster.enabled = false;
                show_Position = !show_Position;
                DrawingStation = false;
                return;
            }
        }

        if (GUI.Button(new Rect(ScaleX(0.135f), ScaleY(0.02f), ScaleX(0.085f), ScaleY(0.16f)), "", GUIStyle.none))
        {
        
        
            show_Position = !show_Position;
        }
        if (GUI.Button(new Rect(ScaleX(0.255f), ScaleY(0.02f), ScaleX(0.085f), ScaleY(0.16f)), "", GUIStyle.none))
        {
            ics_panel.CCUfail = !ics_panel.CCUfail;
        }

        if (GUI.Button(new Rect(ScaleX(0.91f), ScaleY(0.02f), ScaleX(0.085f), ScaleY(0.16f)), "", GUIStyle.none))
        {
            if (directLoad) SceneManager.LoadScene("10_Main Menu");
            else loadscreen.LoadScreen("10_Main Menu");
        }


        if (show_Position == true) RenderMenu_Position();
        DrawStatus();
    }

    void DrawStatus()
    {
        /*string debugString;
        debugString = "Faults Enabled:\n";

        if (ics_panel.CCUfail) debugString += "CCU FAIL\n";
        else debugString += "No faults enabled\n";

        //debugString += "\n\nDISCLAIMER: FOR TEST/EVAL, NOT FOR GENERAL CONSUMPTION\n" +
          //  "Graphics/UI subject to change";
          
        GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.9f), ScaleX(1.0f), ScaleY(0.1f)), debugString);*/
    }

    void RenderMenu_Position()
    {
        /*if (GUI.Button(new Rect(ScaleX(0.1f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "System Overview"))
        {

            Backup_Panel.SetActiveRecursively(true);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
        }*/
        if (GUI.Button(new Rect(ScaleX(0.2f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Pilot"))
        {

            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = true;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.3f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Co-Pilot"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = true;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            Backup_Panel.SetActiveRecursively(false);
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.4f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Left Aft Crew Member"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = true;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.5f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Right Aft Crew Member"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = true;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.6f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Crew Rest"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = true;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.7f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Forward Loadmaster"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = true;
            ICS_Aft_Loadmaster.enabled = false;
            show_Position = !show_Position;
            DrawingStation = true;
        }
        if (GUI.Button(new Rect(ScaleX(0.8f), ScaleY(0.19f), ScaleX(0.1f), ScaleY(0.05f)), "Aft Loadmaster"))
        {
            Backup_Panel.SetActiveRecursively(false);
            ICS_Panel.SetActiveRecursively(false);
            ICS_Pilot.enabled = false;
            ICS_CoPilot.enabled = false;
            ICS_LACM.enabled = false;
            ICS_RACM.enabled = false;
            ICS_CrewRest.enabled = false;
            ICS_Forward_Loadmaster.enabled = false;
            ICS_Aft_Loadmaster.enabled = true;
            show_Position = !show_Position;
            DrawingStation = true;
        }
    }



    /// <summary>
    /// Public call to allow Unity UI objects to exit the program.
    /// </summary>
    public void QuitProgram()
    {
        Application.Quit();
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
