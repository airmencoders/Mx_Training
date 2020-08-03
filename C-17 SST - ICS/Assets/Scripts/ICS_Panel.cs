using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container for all ICS panel variables so we can simulate all positions
/// This is only so big due to time constraints otherwise it would be broken up into multiple modules
/// </summary>
public class ICS_Panel : MonoBehaviour {

    public bool debugMode = false;
    bool CCUfail = true;

    // These are the 8 displays on the ICS Panel, this could be an array but there are 4 different behaviours
    public TextMesh LCD_Rad1;
    public TextMesh LCD_Rad2;
    public TextMesh LCD_Rad3;
    public TextMesh LCD_Rad4;
    public TextMesh LCD_Rad5;
    public TextMesh LCD_ICS;
    public TextMesh LCD_NAV;
    public TextMesh LCD_PSNL;

    public ICS_Mic_Switch mic_Rad1;
    public ICS_Mic_Switch mic_Rad2;
    public ICS_Mic_Switch mic_Rad3;
    public ICS_Mic_Switch mic_Rad4;
    public ICS_Mic_Switch mic_Rad5;
    public ICS_Mic_Switch mic_ICS;

    // Drives all of the audio, and manages required timers to keep said audio in sync
    public ICS_AudioScript ICS_Audio;

    // TODO - Will be used to handle locations of ICS panel, and options available. (LM panel has extra options)
    public enum Location
    {
        Pilot,
        Copilot,
        LACM,
        RACM,
        Crewrest,
        FwdLoadmaster,
        AftLoadmaster
    }

    //////////// RADIO ////////////
    const int totalRadios = 9;

    enum Radio_Type                                      // Types of radios, also used to generate display strings
    {
        COM1,
        COM2,
        UHF,
        VHF,
        HF1,
        HF2,
        AER1,
        AER2,
        NONE
    }

    int[] Radio_Volume = new int[totalRadios]            // Volumes for each radio
    {
        5,          // Com 1
        5,          // Com 2
        5,          // UHF
        5,          // VHF
        5,          // HF1
        5,          // HF2
        5,          // Aero-I Ch 1
        5,          // Aero-I Ch 2
        5           // BLANK, Not used
    };

    bool[] Radio_Recieve = new bool[totalRadios]         // Which radios are active
    {
        false,      // COM 1
        false,      // COM 2
        false,      // UHF
        false,      // VHF
        false,      // HF1
        false,      // HF2
        false,      // Aero-I Ch 1
        false,      // Aero-I Ch 2
        false       // BLANK, Not used
    };

    Radio_Type[] Radio_Select = new Radio_Type[5]{              // Which channels are displayed by default
        Radio_Type.COM1,
        Radio_Type.COM2,
        Radio_Type.UHF,
        Radio_Type.VHF,
        Radio_Type.NONE
    };

    bool[] Radio_Available = new bool[totalRadios]         // Which radios are available to be selected
    {
        false,      // COM 1
        false,      // COM 2
        false,      // UHF
        false,      // VHF
        true,      // HF1
        true,      // HF2
        true,      // Aero-I Ch 1
        true,      // Aero-I Ch 2
        true       // BLANK, Not used
    };


    //////////// ICS //////////// - See comm section for layout details
    enum ICS_Type
    {
        ICS,            // ICS+ HOT5
        PA,             // T when using PA
        HPA,
        PVT             // ON/OFF
    }

    int[] ICS_Volume = new int[]
    {
        5,
        5,
        5,
        5
    };

    bool[] ICS_Recieve = new bool[4]
    {
        true,
        false,
        false,
        false
    };

    ICS_Type ICS_Select = ICS_Type.ICS;

    //////////// NAV //////////// - See comm section for layout details

    enum NAV_Type
    {
        NAV1,
        NAV2,
        ADF,
        TAC,
        MB,
        NONE
    }

    int[] NAV_Volume = new int[]
    {
        5,
        5,
        5,
        5,
        5,
        5
    };

    bool[] NAV_Recieve = new bool[]
    {
        false,
        false,
        false,
        false,
        false,
        false
    };

    NAV_Type NAV_Select = NAV_Type.NONE;


    //////////// PSNL //////////// - See comm section for layout details

    enum PSNL_Type
    {
        HDST,
        SPKR,
        BRT,
        VOX                 // TH 5
    }

    int[] PSNL_Volume = new int[]
    {
        5,
        5,
        5,
        5
    };

    bool[] PSNL_Recieve = new bool[4]
    {
        false,
        false,
        false,
        false,
    };

    PSNL_Type PSNL_Select = PSNL_Type.HDST;

    // Obtain access to scripts on the control knobs
    public KnobController[] Rad_Knob = new KnobController[5];
    public KnobController ICS_Knob;
    public KnobController NAV_Knob;
    public KnobController PSNL_Knob;

    public SW_Hot_Xmit sw_hot_xmit;
    public SW_Mode sw_mode;
    public SW_PA_CALL sw_pa_call;

    public Switch_BackupAudio sw_backup;
    public Switch_Cargo_Isolate sw_cargo;

    // Use this for initialization
    void Start () {
        // These lines address a rendering flaw in Text Mesh, and allows for it to be drawn behind other objects. Removing this will break fading, and help pages once they are enabled
        // LCD_Rad1.GetComponent<MeshRenderer>().sortingLayerName = "UI";
        // LCD_Rad1.GetComponent<MeshRenderer>().sortingOrder = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (sw_mode.mode < 2 || CCUfail || sw_backup.isOn)
        {
            ReadRadKnobs();         // Poll radio knobs for changes
            ReadICSKnob();          // Same as above, etc. etc.
            ReadNAVKnob();
            ReadPSNLKnob();
            ReadModeKnob();
        }
        SetAudio();             // Pass panel settings to audio playback module
        UpdateLCDS();           // Update LCD screens


    }

    ////////////// INPUT CONTROLS ///////////////////
    void ReadRadKnobs()
    {

        for (int i = 0; i < 5; i++)
        {
            // Check for radio change
            // TODO: ADD CHECK TO SKIP RADIOS ALREADY SELECTED
            if (Rad_Knob[i].turnedCCW == true && Rad_Knob[i].isPushed == true)
            {
                if (Radio_Select[i] > Radio_Type.COM1) Radio_Select[i] = DecrementRadioChannel(i);
                else Radio_Select[i] = Radio_Type.AER2;
                Rad_Knob[i].turnedCCW = false;
            }
            else if (Rad_Knob[i].turnedCW == true && Rad_Knob[i].isPushed == true)
            {
                //if (Radio_Select[i] < Radio_Type.AER2) Radio_Select[i]++;
                if (Radio_Select[i] < Radio_Type.AER2) Radio_Select[i] = IncrementRadioChannel(i);
                else Radio_Select[i] = Radio_Type.COM1;
                Rad_Knob[i].turnedCW = false;
            }

            // Check for volume change
            if (Rad_Knob[i].turnedCCW == true && Rad_Knob[i].isPushed == false)
            {
                if (Radio_Volume[(int)Radio_Select[i]] > 0) Radio_Volume[(int)Radio_Select[i]]--;
                Rad_Knob[i].turnedCCW = false;
            }
            else if (Rad_Knob[i].turnedCW == true && Rad_Knob[i].isPushed == false)
            {
                if (Radio_Volume[(int)Radio_Select[i]] < 9) Radio_Volume[(int)Radio_Select[i]]++;
                Rad_Knob[i].turnedCW = false;
            }

            // Check for mic change
            if (Rad_Knob[i].wasPulled == true)
            {
                Radio_Recieve[(int)Radio_Select[i]] = !Radio_Recieve[(int)Radio_Select[i]];
                Rad_Knob[i].wasPulled = false;
            }
        }

    }


    void ReadICSKnob()
    {
        // Check for ICS change
        // Check for radio change
        // TODO: ADD CHECK TO SKIP RADIOS ALREADY SELECTED
        if (ICS_Knob.turnedCCW == true && ICS_Knob.isPushed == true)
        {
            if (ICS_Select > ICS_Type.ICS) ICS_Select--;
            else ICS_Select = ICS_Type.PVT;
            ICS_Knob.turnedCCW = false;
        }
        else if (ICS_Knob.turnedCW == true && ICS_Knob.isPushed == true)
        {
            if (ICS_Select < ICS_Type.PVT) ICS_Select++;
            else ICS_Select = ICS_Type.ICS;
            ICS_Knob.turnedCW = false;
        }

        // Check for volume change
        if (ICS_Knob.turnedCCW == true && ICS_Knob.isPushed == false)
        {
            if (ICS_Volume[(int)ICS_Select] > 0) ICS_Volume[(int)ICS_Select]--;
            ICS_Knob.turnedCCW = false;
        }
        else if (ICS_Knob.turnedCW == true && ICS_Knob.isPushed == false)
        {
            if (ICS_Volume[(int)ICS_Select] < 9) ICS_Volume[(int)ICS_Select]++;
            ICS_Knob.turnedCW = false;
        }

        // Check for mic change
        if (ICS_Knob.wasPulled == true)
        {
            ICS_Recieve[(int)ICS_Select] = !ICS_Recieve[(int)ICS_Select];
            ICS_Knob.wasPulled = false;
        }
    }

    void ReadNAVKnob()
    {
        // Check for ICS change
        // Check for radio change
        // TODO: ADD CHECK TO SKIP RADIOS ALREADY SELECTED
        if (NAV_Knob.turnedCCW == true && NAV_Knob.isPushed == true)
        {
            if (NAV_Select > NAV_Type.NAV1) NAV_Select--;
            else NAV_Select = NAV_Type.NONE;
            NAV_Knob.turnedCCW = false;
        }
        else if (NAV_Knob.turnedCW == true && NAV_Knob.isPushed == true)
        {
            if (NAV_Select < NAV_Type.NONE) NAV_Select++;
            else NAV_Select = NAV_Type.NAV1;
            NAV_Knob.turnedCW = false;
        }

        // Check for volume change
        if (NAV_Knob.turnedCCW == true && NAV_Knob.isPushed == false)
        {
            if (NAV_Volume[(int)NAV_Select] > 0) NAV_Volume[(int)NAV_Select]--;
            NAV_Knob.turnedCCW = false;
        }
        else if (NAV_Knob.turnedCW == true && NAV_Knob.isPushed == false)
        {
            if (NAV_Volume[(int)NAV_Select] < 9) NAV_Volume[(int)NAV_Select]++;
            NAV_Knob.turnedCW = false;
        }

        // Check for mic change
        if (NAV_Knob.wasPulled == true)
        {
            NAV_Recieve[(int)NAV_Select] = !NAV_Recieve[(int)NAV_Select];
            NAV_Knob.wasPulled = false;
        }
    }

    void ReadPSNLKnob()
    {
        // Check for PSNL change
        // Check for radio change
        // TODO: ADD CHECK TO SKIP RADIOS ALREADY SELECTED
        if (PSNL_Knob.turnedCCW == true && PSNL_Knob.isPushed == true)
        {
            if (PSNL_Select > PSNL_Type.HDST) PSNL_Select--;
            else PSNL_Select = PSNL_Type.VOX;
            PSNL_Knob.turnedCCW = false;
        }
        else if (PSNL_Knob.turnedCW == true && PSNL_Knob.isPushed == true)
        {
            if (PSNL_Select < PSNL_Type.VOX) PSNL_Select++;
            else PSNL_Select = PSNL_Type.HDST;
            PSNL_Knob.turnedCW = false;
        }

        // Check for volume change
        if (PSNL_Knob.turnedCCW == true && PSNL_Knob.isPushed == false)
        {
            if (PSNL_Volume[(int)PSNL_Select] > 0) PSNL_Volume[(int)PSNL_Select]--;
            PSNL_Knob.turnedCCW = false;
        }
        else if (PSNL_Knob.turnedCW == true && PSNL_Knob.isPushed == false)
        {
            if (PSNL_Volume[(int)PSNL_Select] < 9) PSNL_Volume[(int)PSNL_Select]++;
            PSNL_Knob.turnedCW = false;
        }

        // Check for mic change
        if (PSNL_Knob.wasPulled == true)
        {
            PSNL_Recieve[(int)PSNL_Select] = !PSNL_Recieve[(int)PSNL_Select];
            PSNL_Knob.wasPulled = false;
        }
    }

    void ReadModeKnob()
    {
        Debug.Log(sw_mode.wasPulled);
        if (sw_mode.wasPulled == true)
        {
            sw_mode.wasPulled = false;
            RebootIt();
        }
    }

    public void Randomize()
    {
        Debug.Log("RANDOM MODE");
        for (int i = 0; i < 9; i++)
        {
            Radio_Recieve[i] = (Random.value > 0.5f);
            Radio_Volume[i] = 5;
        }
        for (int i = 0; i < 6; i++)
        {
            NAV_Recieve[i] = false;//(Random.value > 0.5f);
            NAV_Volume[i] = 5;
        }
        
        sw_backup.isOn = (Random.value > 0.5f);
        sw_mode.knob_pos = (int)Random.Range(0.0f, 3.0f);
    }


    public void RebootIt()
    {
        Debug.Log("Reset panel");
        for (int i = 0; i < 9; i++)
        {
            Radio_Recieve[i] = false;
        }
        for (int i = 0; i < 6; i++)
        {
            NAV_Recieve[i] = false;
        }

        Radio_Select[0] = Radio_Type.COM1;
        Radio_Select[1] = Radio_Type.COM2;
        Radio_Select[2] = Radio_Type.UHF;
        Radio_Select[3] = Radio_Type.VHF;
        Radio_Select[4] = Radio_Type.NONE;

        ICS_Select = ICS_Type.ICS;

        NAV_Select = NAV_Type.NONE;

        sw_backup.isOn = false;
        sw_mode.knob_pos = 0;
    }

    public void MuteIt()
    {
        Debug.Log("Reset panel");
        for (int i = 0; i < 9; i++)
        {
            Radio_Recieve[i] = false;
        }
        for (int i = 0; i < 6; i++)
        {
            NAV_Recieve[i] = false;
        }

    }

    public void AllMicsOff()
    {
        if (sw_hot_xmit.knob_pos == 0) mic_ICS.MicOn = true;
        else mic_ICS.MicOn = false;
        mic_Rad1.MicOn = false;
        mic_Rad2.MicOn = false;
        mic_Rad3.MicOn = false;
        mic_Rad4.MicOn = false;
        mic_Rad5.MicOn = false;
    }

    ////////////// OUTPUT CONTROLS ///////////////////
    /// <summary>
    /// Controls which audio is to play, and at what volume (The following are not utilized, AERO-I Ch1, AERO-I Ch2, NAV1, NAV2, ICS, PVT
    /// </summary>
    void SetAudio()
    {
        if(sw_mode.mode == 2 || CCUfail)
        {
            ICS_Audio.COM1_Enabled = false;
            ICS_Audio.COM2_Enabled = false;
            ICS_Audio.UHF_Enabled = false;
            ICS_Audio.VHF_Enabled = false;
            ICS_Audio.HF1_Enabled = false;
            ICS_Audio.HF2_Enabled = false;
            ICS_Audio.MarkerBeacon_Enabled = false;
            ICS_Audio.TACAN_Enabled = false;
            ICS_Audio.ADF_Enabled = false;
            return;
        }
        else if(sw_mode.mode == 3 && sw_backup.isOn)
        {
            Debug.Log("PLAYING COM 1");
            ICS_Audio.COM1_Enabled = true;
            return;
        }



        if (Radio_Recieve[0]) ICS_Audio.COM1_Enabled = true;
        else ICS_Audio.COM1_Enabled = false;

        if (Radio_Recieve[1]) ICS_Audio.COM2_Enabled = true;
        else ICS_Audio.COM2_Enabled = false;

        if (Radio_Recieve[2]) ICS_Audio.UHF_Enabled = true;
        else ICS_Audio.UHF_Enabled = false;

        if (Radio_Recieve[3]) ICS_Audio.VHF_Enabled = true;
        else ICS_Audio.VHF_Enabled = false;

        if (Radio_Recieve[4]) ICS_Audio.HF1_Enabled = true;
        else ICS_Audio.HF1_Enabled = false;

        if (Radio_Recieve[5]) ICS_Audio.HF2_Enabled = true;
        else ICS_Audio.HF2_Enabled = false;

        if (NAV_Recieve[4]) ICS_Audio.MarkerBeacon_Enabled = true;
        else ICS_Audio.MarkerBeacon_Enabled = false;

        if (NAV_Recieve[3]) ICS_Audio.TACAN_Enabled = true;
        else ICS_Audio.TACAN_Enabled = false;

        if (NAV_Recieve[2]) ICS_Audio.ADF_Enabled = true;
        else ICS_Audio.ADF_Enabled = false;

        ICS_Audio.Volume_COM1 = (float)Radio_Volume[0] * 0.1f;
        ICS_Audio.Volume_COM2 = (float)Radio_Volume[1] * 0.1f;
        ICS_Audio.Volume_UHF = (float)Radio_Volume[2] * 0.1f;
        ICS_Audio.Volume_VHF = (float)Radio_Volume[3] * 0.1f;
        ICS_Audio.Volume_HF1 = (float)Radio_Volume[4] * 0.1f;
        ICS_Audio.Volume_HF2 = (float)Radio_Volume[5] * 0.1f;

        ICS_Audio.Volume_MarkerBeacon = (float)NAV_Volume[4] * 0.1f;
        ICS_Audio.Volume_TACAN = (float)NAV_Volume[3] * 0.1f;
        ICS_Audio.Volume_ADF = (float)NAV_Volume[2] * 0.1f;
    }

    /// <summary>
    /// Updates all LCD screens based on switch positions
    /// </summary>
    void UpdateLCDS()
    {
        string LCD_String;

        if (CCUfail)
        {
            LCD_String = "CCU\nFAIL";

            LCD_Rad1.text = LCD_String;
            LCD_Rad2.text = LCD_String;
            LCD_Rad3.text = LCD_String;
            LCD_Rad4.text = LCD_String;
            LCD_Rad5.text = LCD_String;

            LCD_ICS.text = LCD_String;
            LCD_NAV.text = LCD_String;
            LCD_PSNL.text = LCD_String;
            return;
        }
        else if (sw_backup.isOn)
        {
            LCD_String = "BKUP\nAUD";

            LCD_Rad1.text = LCD_String;
            LCD_Rad2.text = LCD_String;
            LCD_Rad3.text = LCD_String;
            LCD_Rad4.text = LCD_String;
            LCD_Rad5.text = LCD_String;

            LCD_ICS.text = LCD_String;
            LCD_NAV.text = LCD_String;
            LCD_PSNL.text = LCD_String;
            return;
        }
        else if (sw_mode.mode > 1)
        {
            LCD_String = "CHCK\nSW";

            LCD_Rad1.text = LCD_String;
            LCD_Rad2.text = LCD_String;
            LCD_Rad3.text = LCD_String;
            LCD_Rad4.text = LCD_String;
            LCD_Rad5.text = LCD_String;

            LCD_ICS.text = LCD_String;
            LCD_NAV.text = LCD_String;
            LCD_PSNL.text = LCD_String;
            return;
        }

        if (Radio_Select[0] == Radio_Type.NONE)
        {
            LCD_String = "";
            LCD_Rad1.text = LCD_String;
        }
        else
        {
            LCD_String = Radio_Select[0].ToString() + "\n";
            if (Radio_Recieve[(int)Radio_Select[0]] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += Radio_Volume[(int)Radio_Select[0]];
            LCD_Rad1.text = LCD_String;
        }

        if (Radio_Select[1] == Radio_Type.NONE)
        {
            LCD_String = "";
            LCD_Rad2.text = LCD_String;
        }
        else
        {
            LCD_String = Radio_Select[1].ToString() + "\n";
            if (Radio_Recieve[(int)Radio_Select[1]] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += Radio_Volume[(int)Radio_Select[1]];
            LCD_Rad2.text = LCD_String;
        }

        if (Radio_Select[2] == Radio_Type.NONE)
        {
            LCD_String = "";
            LCD_Rad3.text = LCD_String;
        }
        else
        {
            LCD_String = Radio_Select[2].ToString() + "\n";
            if (Radio_Recieve[(int)Radio_Select[2]] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += Radio_Volume[(int)Radio_Select[2]];
            LCD_Rad3.text = LCD_String;
        }

        if (Radio_Select[3] == Radio_Type.NONE)
        {
            LCD_String = "";
            LCD_Rad4.text = LCD_String;
        }
        else
        {
            LCD_String = Radio_Select[3].ToString() + "\n";
            if (Radio_Recieve[(int)Radio_Select[3]] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += Radio_Volume[(int)Radio_Select[3]];
            LCD_Rad4.text = LCD_String;
        }

        if (Radio_Select[4] == Radio_Type.NONE)
        {
            LCD_String = "";
            LCD_Rad5.text = LCD_String;
        }
        else
        {
            LCD_String = Radio_Select[4].ToString() + "\n";
            if (Radio_Recieve[(int)Radio_Select[4]] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += Radio_Volume[(int)Radio_Select[4]];
            LCD_Rad5.text = LCD_String;
        }

        // ICS Section
        LCD_String = ICS_Select.ToString() + "\n";
        if (sw_hot_xmit.knob_pos == 0) LCD_String += "HOT";
        else if (ICS_Recieve[(int)ICS_Select] == true) LCD_String += "R  ";
        else LCD_String += "   ";
        LCD_String += ICS_Volume[(int)ICS_Select];
        LCD_ICS.text = LCD_String;


        // NAV SECTION
        if (NAV_Select == NAV_Type.NONE)
        {
            LCD_String = "";
        }
        else
        {
            LCD_String = NAV_Select.ToString() + "\n";
        
            if (NAV_Recieve[(int)NAV_Select] == true) LCD_String += "R  ";
            else LCD_String += "   ";
            LCD_String += NAV_Volume[(int)NAV_Select];
        }
        LCD_NAV.text = LCD_String;

        
        LCD_String = PSNL_Select.ToString() + "\n";
        if (Radio_Recieve[(int)PSNL_Select] == true) LCD_String += "R  ";
        else LCD_String += "   ";
        LCD_String += PSNL_Volume[(int)PSNL_Select];
        LCD_PSNL.text = LCD_String;
    }

    Radio_Type IncrementRadioChannel(int i)
    {
        Radio_Type currentRadio = Radio_Select[i];
        bool loop = true;
        bool radioGood = true;

        while (loop)
        {
            radioGood = true;
            currentRadio++;
            if (i == 0)
            {
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 1)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 2)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 3)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 4)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
            }
            Debug.Log(currentRadio);
            if (radioGood == true) loop = false;
        }
        return currentRadio;

    }

    Radio_Type DecrementRadioChannel(int i)
    {
        Radio_Type currentRadio = Radio_Select[i];
        bool loop = true;
        bool radioGood = true;

        while (loop)
        {
            radioGood = true;
            currentRadio--;
            if (i == 0)
            {
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 1)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 2)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 3)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[4]) radioGood = false;
            }
            if (i == 4)
            {
                if (currentRadio == Radio_Select[0]) radioGood = false;
                if (currentRadio == Radio_Select[1]) radioGood = false;
                if (currentRadio == Radio_Select[2]) radioGood = false;
                if (currentRadio == Radio_Select[3]) radioGood = false;
            }
            Debug.Log(currentRadio);
            if (radioGood == true) loop = false;
        }
        return currentRadio;
    }

    /////////////// DEBUGGING FUNCTIONS ///////////////
    private void OnGUI()
    {
        if (!debugMode) return; 

        string debugString = "";

        for (int i = 0; i < 5; i++)
        {

            debugString += "RAD" + i.ToString() + ": " + Radio_Select[i].ToString();
            debugString += "\nRAD1 Volume: \"";
            if (Radio_Recieve[(int)Radio_Select[i]] == true) debugString += "R  ";
            else debugString += "   ";
            debugString += Radio_Volume[(int)Radio_Select[i]] + "\"";
            debugString += "\n\n";
        }

        debugString += "ICS: " + ICS_Select.ToString();
        debugString += "\nICS Volume: \"";
        if (ICS_Recieve[(int)ICS_Select] == true) debugString += "R  ";
        else debugString += "   ";
        debugString += ICS_Volume[(int)ICS_Select] + "\"";
        debugString += "\n\n";


        debugString += "NAV: " + NAV_Select.ToString();
        debugString += "\nNAV Volume: \"";
        if (NAV_Recieve[(int)NAV_Select] == true) debugString += "R  ";
        else debugString += "   ";
        debugString += NAV_Volume[(int)NAV_Select] + "\"";
        debugString += "\n\n";

        debugString += "PSNL: " + PSNL_Select.ToString();
        debugString += "\nPSNL Volume: \"";
        if (PSNL_Recieve[(int)PSNL_Select] == true) debugString += "R  ";
        else debugString += "   ";
        debugString += PSNL_Volume[(int)PSNL_Select] + "\"";
        debugString += "\n\n";

        GUI.Label(new Rect(ScaleX(0.0f), ScaleY(0.1f), ScaleX(1.0f), ScaleY(1.0f)), debugString);
    }

    /// <summary>
    /// Simulate normal operation. Provides access to Unity UI
    /// </summary>
    public void CCU_Enabled()
    {
        CCUfail = false;
    }
    
    /// <summary>
    /// Simulates what would happen if the CCU were to fail. Provides access to Unity UI
    /// </summary>
    public void CCU_Disabled()
    {
        CCUfail = false;
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
