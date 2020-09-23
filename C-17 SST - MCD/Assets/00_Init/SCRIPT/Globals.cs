using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Globally used variables:
/// Used to maintain variables between the menu and the MCD scenes.
/// </summary>
public class Globals : MonoBehaviour {

    ////////////////// PRESENTATION MODE /////////////////////////
    public bool demo_mode = false;              // Fake power point for the wowie zowie


    ////////////////// Used for all scenes ///////////////////////
    public bool lockUI = true;                         // Used to lock out UI during loading process

    
    ////////////////// USED FOR MCD SIMULATOR ////////////////////
    // Aircraft Status - Most common generation of status faults
    public bool misc_EFCSreset = false;         // Was EFCS reset pressed (Clears about 3 pages of FCC faults, could occur via habit of other maintainence personnel, ops check, or if the plane has been flown)
    public bool misc_IRUaligned = false;        // Inertial Reference Units should be aligned, but in many cases gets overlooked generating nuisance jobs
    public bool misc_Rad1_On = false;             // Radar altimeters should generate faults when off, but might be on in some cases eliminating this fault
    public bool misc_Rad2_On = false;             // Radar altimeters should generate faults when off, but might be on in some cases eliminating this fault
    public bool misc_HF1_On = false;              // HF radio/Auto Comm Processor might be off for refuel/defuel, causing extra faults to appear.
    public bool misc_HF2_On = false;              // Hidden option, can be toggled individually from MCD


    // Aircraft Faults
    public bool fault_TAWS = false;             // Common fault with Terrain Awareness Warning/Video Information Processor
    public bool fault_CCU1 = false;             // Common fault with Communications Control Unit
    public bool fault_OBIGGS = false;           // Uncommon fault with OBIGGS, is highlighted in workcards as it could cause a fire
    public bool fault_BATT = false;             // Fault that occurs with aircraft battery failure

    // Status messages
    public bool stat_NotSim = false;
    public bool stat_DBmismatch = false;        // Nuisance message in the status display, often causes unneeded work and is not a fault
    public bool stat_ChMast = false;            // Nuisance message in the status display, often causes unneeded work and is not a fault
    public bool stat_datalink = false;          // Nuisance message in the status display, occurs if Aero-I & HF is offline


    /// <summary>
    /// Enables all faults
    /// </summary>
    public void EnableAll()
    {
        misc_EFCSreset = false;             // This button may have been hit by a pilot or seasoned crew chief, clears about 3 to 5 pages of faults
        misc_IRUaligned = false;            // Occasionally left on by flight crew and numerous ops checks, generates a page or two of faults
        misc_Rad1_On = false;                 // Occasionally left on by flight crew post flight. Should have been turned off during parking task, but due to it being a recent change it's often overlooked
        misc_Rad2_On = false;                 // Occasionally left on by flight crew post flight. Should have been turned off during parking task, but due to it being a recent change it's often overlooked

        misc_HF1_On = false;                // Might be on or off, causes ACP faults
        misc_HF2_On = false;

        fault_TAWS = true;                  // Add TAWS/VIP faults
        fault_CCU1 = true;                  // CCU 1 fault
        fault_OBIGGS = true;                // OBIGGS fault
        fault_BATT = true;                  // Non-Avonics fault

        stat_NotSim = false;
        stat_DBmismatch = true;             // Database mismatch message
        stat_ChMast = true;                 // Other common scratch pad messages
        stat_datalink = true;               // No datalink sounds bad, means nothing
    }

        /// <summary>
        /// GenRandom() - Generates random faults for fault list
        /// </summary>
    public void GenRandom()
    {
        // Reset Aircraft Status before coinflip, this is required if you exit to the menu
        misc_EFCSreset = false;             // This button may have been hit by a pilot or seasoned crew chief, clears about 3 to 5 pages of faults
        misc_IRUaligned = false;            // Occasionally left on by flight crew and numerous ops checks, generates a page or two of faults
        misc_Rad1_On = false;                 // Occasionally left on by flight crew post flight. Should have been turned off during parking task, but due to it being a recent change it's often overlooked
        misc_Rad2_On = false;                 // Occasionally left on by flight crew post flight. Should have been turned off during parking task, but due to it being a recent change it's often overlooked

        misc_HF1_On = false;                  // Might be on or off, causes ACP faults
        misc_HF2_On = false;                  // Might be on or off, causes ACP faults

        fault_TAWS = false;                  // Add TAWS/VIP faults
        fault_CCU1 = false;                  // CCU 1 fault
        fault_OBIGGS = false;                // OBIGGS fault
        fault_BATT = false;                  // Non-Avonics fault

        stat_NotSim = false;
        stat_DBmismatch = false;             // Database mismatch message
        stat_ChMast = false;                 // Other common scratch pad messages
        stat_datalink = false;               // No datalink sounds bad, means nothing
        
        // Perform a coin flip to see if values should be set
        if (CoinFlip() == true) misc_EFCSreset = true;
        if (CoinFlip() == true) misc_IRUaligned = true;
        if (CoinFlip() == true)
        {
            misc_Rad1_On = true;
            misc_Rad2_On = true;
        }
        if (CoinFlip() == true)
        {
            misc_HF1_On = true;
            misc_HF2_On = true;
        }
               
        if (CoinFlip() == true) fault_TAWS = true;
        if (CoinFlip() == true) fault_CCU1 = true;
        if (CoinFlip() == true) fault_OBIGGS = true;
        if (CoinFlip() == true) fault_BATT = true;
        
        if (CoinFlip() == true) stat_DBmismatch = true;
        if (CoinFlip() == true) stat_ChMast = true;
        if (CoinFlip() == true) stat_datalink = true;
    }

    /// <summary>
    /// Flips a coin to set a random value
    /// </summary>
    /// <returns>Heads or Tails (True or false)</returns>
    bool CoinFlip()
    {
        if (Random.Range(0.0f, 1.0f) > 0.5f) return true;
        else return false;                                       
    }
}
