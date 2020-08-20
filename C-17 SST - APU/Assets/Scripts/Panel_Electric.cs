using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Panel_Electric : MonoBehaviour
{
    // Electrical status
    bool APU_Avail = false;
    bool APU_On = false;
    bool AC_X_Tie = false;
    bool GPU_Avail = true;          // Going to always assume true
    bool GPU_On = false;          // Going to always assume true
    
    bool BattOn = false;
    bool EmergPower = false;
    bool XferBus = false;
    bool L_Av = false;
    bool DC_X_Tie = false;
    bool R_Av = false;

    // Switch positions, used to handle visual aspect might mismatch actual status depending on configuration
    int sw_APU = 1;                     // 0 - Reset, 1 - Auto, 2 - On
    int sw_AC_X_Tie = 0;                // 0 - Off, 1 - Auto, 2 - Tie
    int sw_GPU = 0;

    int sw_L_Av = 0;                    // 0 - Off, 1 - Auto, 2 - Tie
    int sw_DC_X_Tie = 0;
    int sw_R_Av = 0;                    // 0 - Off, 1 - Auto, 2 - Tie
    int sw_Battery = 0;
    int sw_EmergencyPower = 0;          // 0 - Off, 1 - Auto, 2 - On
    int sw_XferBus = 0;

    // Game objects, animations exist but wouldn't export to a Unity compatible format
    public GameObject mdl_APU_Reset;
    public GameObject mdl_APU_Off;
    public GameObject mdl_APU_On;
    public GameObject mdl_AC_Tie_Off;
    public GameObject mdl_AC_Tie_Auto;
    public GameObject mdl_GPU_Off;
    public GameObject mdl_GPU_On;

    public GameObject mdl_L_Av_Off;
    public GameObject mdl_L_Av_On;
    public GameObject mdl_DC_Tie_Off;
    public GameObject mdl_DC_Tie_Auto;
    public GameObject mdl_R_Av_Off;
    public GameObject mdl_R_Av_On;

    public GameObject mdl_Batt_Off;
    public GameObject mdl_Batt_On;
    public GameObject mdl_Emerg_Of;
    public GameObject mdl_Emerg_Auto;
    public GameObject mdl_Emerg_On;
    public GameObject mdl_Xfer_Off;
    public GameObject mdl_Xfer_On;


    // Indicator Lights
    public Light_Generic Ind_APU;
    public Light_Generic Ind_GPU;

    public Light_Generic Ind_Bus_1;
    public Light_Generic Ind_Bus_2;
    public Light_Generic Ind_Bus_3;
    public Light_Generic Ind_Bus_4;

    public Light_Generic Ind_AC_Tie;
    public Light_Generic Ind_DC_Tie;

    public Light_Generic Ind_L_DC;
    public Light_Generic Ind_R_DC;

    public Light_Generic Ind_Batt;
    public Light_Generic Ind_Emerg;
    public Light_Generic Ind_Xfer;

    // Sounds
    public AudioSource snd_Wap_Init;
    public AudioSource snd_Wap_Loop;
    public AudioSource snd_Switch_Toggle;
    // Panel status
    bool UpdateReq = true;              // In case anything is changed on the default status check switches on first status
    float BattTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        //UpdateReq = false;
        if(sw_Battery == 0)
        {
            // Turn everything off
            return;
        }
        else
        {
            BattTimer += Time.deltaTime;
            if (!XferBus)
            {
                float InitDuration = snd_Wap_Init.clip.length;
                if (BattTimer < InitDuration)
                {
                    if (!snd_Wap_Init.isPlaying) snd_Wap_Init.Play();
                }
                else
                {
                    if (!snd_Wap_Loop.isPlaying) snd_Wap_Loop.Play();
                    float FadeVolume = 1.0f - (BattTimer / 10.0f);
                    if (FadeVolume < 0.1f) FadeVolume = 0.1f;

                    snd_Wap_Loop.volume = FadeVolume;
                }
            }
        }
        

    }

    public void ToggleBattery()
    {
        UpdateReq = true;
        if (sw_Battery == 0)
        {
            sw_Battery = 1;
            BattOn = true;
            mdl_Batt_On.SetActive(true);
            mdl_Batt_Off.SetActive(false);
            BattTimer = 0.0f;
            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_Battery = 0;
            BattOn = false;
            mdl_Batt_Off.SetActive(true);
            mdl_Batt_On.SetActive(false);
            snd_Switch_Toggle.Play();
        }

    }
}
