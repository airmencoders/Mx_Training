using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This was divorced from the MCD_Manager page due to the complexity of this section, and to allow for real-time updates
/// </summary>
public class AvionicsFaultPage : MonoBehaviour {

    GameObject globalObject;
    Globals globalAccess;

    public GameObject MCD_Manager_Object;
    MCD_Manager MCD_Manager_Access;

    public bool isReady = false;
    public int totalFaults = 0;
    public int totalPages = 0;

    public string[] faults = new string[60];            //This will store all fault string data, we shouldn't ever see more than 48 anyways. Increasing this absurd values will impact performance negatively.


    // Use this for initialization
    void Start () {
        MCD_Manager_Access = MCD_Manager_Object.GetComponent<MCD_Manager>();

        
    }
	
    

	// Update is called once per frame
	void Update () {
        if (MCD_Manager_Access.MCD_Is_Ready == false) return;           // Wait for MCD Manager to finish initialization

        if (isReady == false)
        {
            SetGlobalAccess();                        // Attempt to get global variable access
            UpdatePages();
        }
    }

    public void UpdatePages()
    {
        int currentFault = 0;

        // These faults are always present, future optimization
        faults[currentFault] = "WIU-EXT";
        currentFault++;

        if (globalAccess.misc_IRUaligned == false)
        {
            faults[currentFault] = "TCAS-MC_ATT";
            currentFault++;
        }

        if (globalAccess.misc_Rad1_On == false)
        {
            faults[currentFault] = "TCAS-MC_RALT 1";
            currentFault++;
        }

        faults[currentFault] = "TCAS-UPPERANT";
        currentFault++;

        faults[currentFault] = "WCC-ABC";       //
        currentFault++;

        faults[currentFault] = "APM-SAU";       //
        currentFault++;

        faults[currentFault] = "SEF-F_RAD";     //
        currentFault++;

        faults[currentFault] = "FCC-F_RAD";
        currentFault++;

        faults[currentFault] = "FCC-F_IRU";
        currentFault++;

        faults[currentFault] = "FCC-F_APDMC";
        currentFault++;

        faults[currentFault] = "FCC-F_NGS";
        currentFault++;

        faults[currentFault] = "FCC-SEF_2";
        currentFault++;

        // HIGH FREQ RADIO OFF
        if (globalAccess.misc_HF1_On == false)
        {
            faults[currentFault] = "CMU-1_ACP1";
            currentFault++;

            faults[currentFault] = "CMU-1_ACP2";
            currentFault++;
        }

        if (globalAccess.misc_HF2_On == false)
        {
            faults[currentFault] = "CMU-2_ACP1";
            currentFault++;

            faults[currentFault] = "CMU-2_ACP2";
            currentFault++;
        }

        // EFCS RESET NOT PRESSED
        if (globalAccess.misc_EFCSreset == false)
        {
            faults[currentFault] = "SEF-F_ADC";
            currentFault++;

            faults[currentFault] = "SEF-F_FAP";
            currentFault++;

            faults[currentFault] = "SEF-F_FPA_4";
            currentFault++;

            faults[currentFault] = "SEF-F_FPA_3";
            currentFault++;

            faults[currentFault] = "SEF-F_FPA_2";
            currentFault++;

            faults[currentFault] = "SEF-F_FPA_1";
            currentFault++;

            faults[currentFault] = "FCC-F_LI_EFM";
            currentFault++;

            faults[currentFault] = "FCC-F_RI_EFM";
            currentFault++;

            faults[currentFault] = "FCC-F_RO_EFM";
            currentFault++;

            faults[currentFault] = "FCC-F_UP_RFM";
            currentFault++;

            faults[currentFault] = "FCC-F_NGS";
            currentFault++;

            faults[currentFault] = "FCC-FCC_1";
            currentFault++;

            faults[currentFault] = "FCC-FCC_2";
            currentFault++;

            faults[currentFault] = "FCC-FCC_3";
            currentFault++;

            faults[currentFault] = "FCC-FCC_4";
            currentFault++;

            faults[currentFault] = "FCC-F_L_AFM";
            currentFault++;

            faults[currentFault] = "FCC-F_R_AFM";
            currentFault++;

            faults[currentFault] = "FCC-F_LO_EFM";
            currentFault++;
        }

        // IRUS OFF AND NOT ALIGNED
        if (globalAccess.misc_IRUaligned == false)
        {
            faults[currentFault] = "IRU1";
            currentFault++;

            faults[currentFault] = "IRU2";
            currentFault++;

            faults[currentFault] = "IRU3";
            currentFault++;

            faults[currentFault] = "IRU4";
            currentFault++;

            faults[currentFault] = "FMCIRU1OR";
            currentFault++;

            faults[currentFault] = "FMCIRU2OR";
            currentFault++;

            faults[currentFault] = "FMCIRU3OR";
            currentFault++;

            faults[currentFault] = "FMCIRU4OR";
            currentFault++;

            faults[currentFault] = "FMCIRU1FC";
            currentFault++;

            faults[currentFault] = "FMCIRU2FC";
            currentFault++;

            faults[currentFault] = "FMCIRU3FC";
            currentFault++;

            faults[currentFault] = "FMCIRU4FC";
            currentFault++;
        }

        // RADAR ALTIMITER OFF
        if (globalAccess.misc_Rad1_On == false)
        {
            faults[currentFault] = "RAD1";
            currentFault++;
        }

        if (globalAccess.misc_Rad2_On == false)
        {
            faults[currentFault] = "RAD2";
            currentFault++;
        }

        // CHECK FOR RAD ALT AND IRU POWER
        if (globalAccess.misc_Rad1_On == false || globalAccess.misc_IRUaligned == false)
        {
            faults[currentFault] = "FMCSKE";
            currentFault++;
        }

        // TAWS FAULT
        if (globalAccess.fault_TAWS == true)
        {

            faults[currentFault] = "VIP 1";
            currentFault++;
        }

        // CCU FAULT
        if (globalAccess.fault_CCU1 == true)
        {

            faults[currentFault] = "CCU 1";
            currentFault++;

            faults[currentFault] = "CCU-CCU 1";
            currentFault++;
        }

        // OBIGGS NON-A FAULT
        if (globalAccess.fault_OBIGGS == true)
        {

            faults[currentFault] = "WCC-MFDC";
            currentFault++;
        }

        totalFaults = currentFault;
        totalPages = (totalFaults + 5) / 6;         

        // Fill the rest in as empty strings, so we always have 6 items to display
        for (int i = currentFault; i < 60; i++)
        {
            faults[i] = "";
        }
    }

    void ResetFaults()
    {
        globalAccess.misc_EFCSreset = false;         // Was EFCS reset pressed (Clears about 3 pages of FCC faults, could occur via habit of other maintainence personnel, ops check, or if the plane has been flown)
        globalAccess.misc_IRUaligned = false;        // Inertial Reference Units should be aligned, but in many cases gets overlooked generating nuisance jobs
        globalAccess.misc_Rad1_On = false;             // Radar altimeters should generate faults when off, but might be on in some cases eliminating this fault
        globalAccess.misc_Rad2_On = false;             // Radar altimeters should generate faults when off, but might be on in some cases eliminating this fault
        globalAccess.misc_HF1_On = false;              // HF radio/Auto Comm Processor might be off for refuel/defuel, causing extra faults to appear.
        globalAccess.misc_HF2_On = false;              // Hidden option, can be toggled individually from MCD


        // Aircraft Faults
        globalAccess.fault_TAWS = false;             // Common fault with Terrain Awareness Warning/Video Information Processor
        globalAccess.fault_CCU1 = false;             // Common fault with Communications Control Unit
        globalAccess.fault_OBIGGS = false;           // Uncommon fault with OBIGGS, is highlighted in workcards as it could cause a fire
        globalAccess.fault_BATT = false;             // Fault that occurs with aircraft battery failure

        // Status messages
        globalAccess.stat_NotSim = false;
        globalAccess.stat_DBmismatch = false;        // Nuisance message in the status display, often causes unneeded work and is not a fault
        globalAccess.stat_ChMast = false;            // Nuisance message in the status display, often causes unneeded work and is not a fault
        globalAccess.stat_datalink = false;          // Nuisance message in the status display, occurs if Aero-I & HF is offline
    }

    public void SetRandomFaults()
    {
        ResetFaults();
        
        // Perform a coin flip to see if values should be set
        if (CoinFlip() == true) globalAccess.misc_EFCSreset = true;
        if (CoinFlip() == true) globalAccess.misc_IRUaligned = true;
        if (CoinFlip() == true)
        {
            globalAccess.misc_Rad1_On = true;
            globalAccess.misc_Rad2_On = true;
        }
        if (CoinFlip() == true)
        {
            globalAccess.misc_HF1_On = true;
            globalAccess.misc_HF2_On = true;
        }

        if (CoinFlip() == true) globalAccess.fault_TAWS = true;
        if (CoinFlip() == true) globalAccess.fault_CCU1 = true;
        if (CoinFlip() == true) globalAccess.fault_OBIGGS = true;
        if (CoinFlip() == true) globalAccess.fault_BATT = true;

        if (CoinFlip() == true) globalAccess.stat_DBmismatch = true;
        if (CoinFlip() == true) globalAccess.stat_ChMast = true;
        if (CoinFlip() == true) globalAccess.stat_datalink = true;
    }

    /// <summary>
    /// Used to gain access to global variables
    /// </summary>
    void SetGlobalAccess()
    {
        try
        {
            globalObject = GameObject.Find("~Globals");
            globalAccess = globalObject.GetComponent<Globals>();
            isReady = true;
            Debug.Log("Avionics fault listing: OK");
        }
#pragma warning disable CS0168 // Variable is declared but never used
        catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
        {
            globalObject = GameObject.Find("~Globals(Clone)");
            globalAccess = globalObject.GetComponent<Globals>();
            isReady = true;
            Debug.Log("Avionics fault listing in standalone mode");
        }
    }

    /// <summary>
    /// Flips a coin to set a random value
    /// </summary>
    /// <returns>Heads or Tails (True or false)</returns>
    bool CoinFlip()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) return true;
        else return false;
    }
}
