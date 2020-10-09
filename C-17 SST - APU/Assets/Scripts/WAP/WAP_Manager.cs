using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the WAP
/// </summary>
public class WAP_Manager : MonoBehaviour
{
    public bool Debug_External_Power = true;
    
    string[] Cue_Messages = new string[5]
    {
        "",
        "ELEC",
        "FLIGHT",
        "MESSAGE",
        "RANDOM"
    };

    int currentCue = 4;

    bool Caution_Set = false;
    bool Warning_Set = false;

    public Pwr_Tied_Light Ind_Caution;
    public Pwr_Tied_Light Ind_Warning;

    public TextMeshPro Text_WAP_Cue;

    // Start is called before the first frame update
    void Start()
    {
        ResetWAP();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Debug_External_Power)
        {
            Text_WAP_Cue.text = "";
            Ind_Caution.SetMaterial(0);
            Ind_Warning.SetMaterial(0);
            ResetWAP();
            return;
        }
        else
        {
            Text_WAP_Cue.text = Cue_Messages[currentCue];
        }

        if (Caution_Set) Ind_Caution.SetMaterial(1);
        else Ind_Caution.SetMaterial(0);

        if (Warning_Set) Ind_Warning.SetMaterial(1);
        else Ind_Warning.SetMaterial(0);
    }

    public void ResetWAP()
    {
        Cue_Messages[1] = "ELEC";
        Cue_Messages[2] = "FLIGHT";
        Cue_Messages[3] = "MESSAGE";
        Cue_Messages[4] = "RANDOM";

        currentCue = 4;
        Caution_Set = true;
        Warning_Set = false;
    }

    /// <summary>
    /// Clear cue button pressed, goto next object
    /// </summary>
    public void ClearCue()
    {
        if (currentCue < 1) return; // No cue to clear in position 0
        Cue_Messages[currentCue] = "";  // Clear the message
        currentCue--;   // Go to next message
        
        if (currentCue == 0)
        {
            Warning_Set = false;
            Caution_Set = false;
        }
    }

    /// <summary>
    /// Adds an item to the cue
    /// </summary>
    public void AddCue(string message)
    {
        if (Cue_Messages[currentCue] == message)
        {
            Caution_Set = true;
            return;
        }

            if (currentCue < 3)     // Limit to buffer of 5 items (This is wrong yes, but for now we are not simulating the WAP)
        {
            Cue_Messages[currentCue + 1] = message;     // Set previous message
            currentCue++;   // Select that new message
        }
        else
        {
            Cue_Messages[currentCue] = message;     // Set the message, buffer is maxed out
        }

        if (message == "APU") Warning_Set = true;
        else Caution_Set = true;
    }


    public void Reset_Caution()
    {
        Caution_Set = false;
    }

    public void Reset_Warning()
    {
        Warning_Set = false;
    }
}
