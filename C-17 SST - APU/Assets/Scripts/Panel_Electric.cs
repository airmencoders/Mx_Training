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
    bool GPU_Avail = false;          // Only option with a UI toggle
    bool GPU_On = false;          // Going to always assume true
    
    bool BattOn = false;
    bool EmergPower = false;
    bool XferBus = false;
    bool L_Av = false;
    bool DC_X_Tie = false;
    bool R_Av = false;

    // Switch positions, used to handle visual aspect might mismatch actual status depending on configuration
    int sw_APU = 0;                     // 0 - Reset, 1 - Auto, 2 - On
    int sw_APUreset = 0;
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
    public GameObject mdl_DC_Tie_Auto;
    public GameObject mdl_DC_Tie_On;
    public GameObject mdl_R_Av_Off;
    public GameObject mdl_R_Av_On;

    public GameObject mdl_Batt_Off;
    public GameObject mdl_Batt_On;
    public GameObject mdl_Emerg_Off;
    public GameObject mdl_Emerg_Auto;
    public GameObject mdl_Emerg_On;
    public GameObject mdl_Xfer_Off;
    public GameObject mdl_Xfer_On;


    // Indicator Lights
    public Pwr_Avail_Light Ind_APU;
    public Pwr_Avail_Light Ind_GPU;

    public Pwr_Tied_Light Ind_Bus_1;
    public Pwr_Tied_Light Ind_Bus_2;
    public Pwr_Tied_Light Ind_Bus_3;
    public Pwr_Tied_Light Ind_Bus_4;

    public Light_Generic Ind_AC_Tie;
    public Pwr_Tied_Light Ind_DC_Tie;

    public Pwr_Tied_Light Ind_L_DC;
    public Pwr_Tied_Light Ind_R_DC;

    public Light_Generic Ind_Batt;
    public Light_Generic Ind_Emerg;
    public Light_Generic Ind_Xfer;

    // Sounds
    public AudioSource snd_Wap_Init;
    public AudioSource snd_Wap_Loop;
    public AudioSource snd_Switch_Toggle;
    public AudioSource snd_AV_On;
    public AudioSource snd_AV_Off;
    // Panel status
    bool UpdateReq = true;              // In case anything is changed on the default status check switches on first status
    float XferTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatus();
        UpdateIndicators();
    }

    public void UpdateIndicators()
    {
        // Check if GPU is available, and set light
        if (GPU_Avail && GPU_On)
        {
            Ind_GPU.SetMaterial(2);
        }
        else if (GPU_Avail)
        {
            Ind_GPU.SetMaterial(1);
        }
        else
        {
            Ind_GPU.SetMaterial(0);
        }

        // Check if APU is available, and set light
        if (APU_Avail && APU_On)
        {
            Ind_APU.SetMaterial(2);
        }
        else if (APU_Avail)
        {
            Ind_APU.SetMaterial(1);
        }
        else
        {
            Ind_APU.SetMaterial(0);
        }

        if (EmergPower)
        {
            Ind_Emerg.turnOn();
        }
        else
        {
            Ind_Emerg.turnOff();
        }

        bool PowerOnSomewhere = false;


        // If any power source is on run these lights
        if(EmergPower || GPU_On || APU_On)
        {
            if (sw_DC_X_Tie == 1) Ind_DC_Tie.SetMaterial(1);
            else Ind_DC_Tie.SetMaterial(0);

            if (sw_L_Av == 1) Ind_L_DC.SetMaterial(1);
            else Ind_L_DC.SetMaterial(0);

            if (sw_R_Av == 1) Ind_R_DC.SetMaterial(1);
            else Ind_R_DC.SetMaterial(0);

            Ind_Bus_1.SetMaterial(1);
            Ind_Bus_2.SetMaterial(1);
            Ind_Bus_3.SetMaterial(1);
            Ind_Bus_4.SetMaterial(1);
        }
        else
        {
            Ind_DC_Tie.SetMaterial(0);
            Ind_Bus_1.SetMaterial(0);
            Ind_Bus_2.SetMaterial(0);
            Ind_Bus_3.SetMaterial(0);
            Ind_Bus_4.SetMaterial(0);
            Ind_L_DC.SetMaterial(0);
            Ind_R_DC.SetMaterial(0);
        }

    }

    public void UpdateStatus()
    {
        
        if (sw_Battery == 0) // Battery switch is off
        {
            // Turn everything off
            BattOn = false;
            EmergPower = false;
            XferBus = false;
            
            return;
        }

        if (sw_EmergencyPower == 0)     // Set emergency power
        {
            EmergPower = false;
        }
        else
        {
            EmergPower = true;
        }

        if (EmergPower)      // Set xfer bus only if emergency power is avail
        {
            if (sw_XferBus == 1)
            {
                Ind_Xfer.turnOff();
                XferBus = true;
            }
            else
            {
                Ind_Xfer.turnOn();
                XferBus = false;
            }
        }


        if (sw_Battery == 1) // Battery or generator power is applied
        {
            XferTimer += Time.deltaTime;
            if (!XferBus && EmergPower)       // Check if WAPS is initiated
            {
                float InitDuration = snd_Wap_Init.clip.length;
                if (XferTimer < InitDuration)
                {
                    if (!snd_Wap_Init.isPlaying)
                    {
                        snd_Wap_Init.Play();
                        XferTimer = 0.0f;
                    }
                }
                else
                {
                    if (!snd_Wap_Loop.isPlaying) snd_Wap_Loop.Play();
                    float FadeVolume = 1.0f - (XferTimer / 10.0f);
                    if (FadeVolume < 0.1f) FadeVolume = 0.1f;

                    snd_Wap_Loop.volume = FadeVolume;
                }
            }
            else
            {
                if (snd_Wap_Init.isPlaying) snd_Wap_Init.Stop();
                if (snd_Wap_Loop.isPlaying) snd_Wap_Loop.Stop();
            }

            

            if(sw_XferBus == 0)
            {
                XferBus = false;
            }
            else
            {
                XferBus = true;
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
            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_Battery = 0;
            BattOn = false;
            mdl_Batt_Off.SetActive(true);
            mdl_Batt_On.SetActive(false);
            snd_Switch_Toggle.Stop();
        }

    }

    /// <summary>
    /// Cycles emergency power
    /// TODO: Automatically select between auto and on
    /// </summary>
    public void ToggleEmergency()
    {
        if (sw_EmergencyPower == 0)
        {
            if (GPU_On)
            {
                sw_EmergencyPower = 1;

                mdl_Emerg_Off.SetActive(false);
                mdl_Emerg_Auto.SetActive(true);
                mdl_Emerg_On.SetActive(false);

                snd_Switch_Toggle.Play();
            }
            else
            {
                sw_EmergencyPower = 2;

                mdl_Emerg_Off.SetActive(false);
                mdl_Emerg_Auto.SetActive(false);
                mdl_Emerg_On.SetActive(true);

                snd_Switch_Toggle.Play();
            }
        }
        else
        {
            sw_EmergencyPower = 0;

            mdl_Emerg_Off.SetActive(true);
            mdl_Emerg_Auto.SetActive(false);
            mdl_Emerg_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }

    /// <summary>
    /// Cycles left AV bus between off/auto
    /// </summary>
    public void ToggleLeftAVbus()
    {
        if (sw_L_Av == 0)
        {
            sw_L_Av = 1;

            mdl_L_Av_Off.SetActive(false);
            mdl_L_Av_On.SetActive(true);

            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_L_Av = 0;

            mdl_L_Av_Off.SetActive(true);
            mdl_L_Av_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }

    /// <summary>
    /// Cycles left AV bus between off/auto
    /// </summary>
    public void ToggleRightAVbus()
    {
        if (sw_R_Av == 0)
        {
            sw_R_Av = 1;

            mdl_R_Av_Off.SetActive(false);
            mdl_R_Av_On.SetActive(true);

            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_R_Av = 0;

            mdl_R_Av_Off.SetActive(true);
            mdl_R_Av_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }


    /// <summary>
    /// Cycles DC Tie bus between auto/on
    /// </summary>
    public void ToggleDCbus()
    {
        if (sw_DC_X_Tie == 0)
        {
            sw_DC_X_Tie = 1;

            mdl_DC_Tie_Auto.SetActive(false);
            mdl_DC_Tie_On.SetActive(true);

            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_DC_X_Tie = 0;

            mdl_DC_Tie_Auto.SetActive(true);
            mdl_DC_Tie_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }

    /// <summary>
    /// Cycles AC Tie bus between off/auto
    /// </summary>
    public void ToggleACbus()
    {
        if (sw_AC_X_Tie == 0)
        {
            sw_AC_X_Tie = 1;

            mdl_AC_Tie_Off.SetActive(false);
            mdl_AC_Tie_Auto.SetActive(true);

            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_AC_X_Tie = 0;

            mdl_AC_Tie_Off.SetActive(true);
            mdl_AC_Tie_Auto.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }

    /// <summary>
    /// Cycles AC Tie bus between off/auto
    /// </summary>
    public void ToggleExtPower()
    {
        if (sw_GPU == 0)
        {
            if (!GPU_Avail)
            {
                snd_Switch_Toggle.Play();
                return;
            }
            sw_GPU = 1;

            mdl_GPU_Off.SetActive(false);
            mdl_GPU_On.SetActive(true);

            snd_Switch_Toggle.Play();
            snd_AV_On.Play();
        }
        else
        {
            sw_GPU = 0;

            mdl_GPU_Off.SetActive(true);
            mdl_GPU_On.SetActive(false);

            snd_Switch_Toggle.Play();
            snd_AV_Off.Play();
        }
    }

    /// <summary>
    /// Cycles AC Tie bus between off/auto
    /// </summary>
    public void ToggleAPU()
    {
        if (sw_APU == 0)
        {
            if (!APU_Avail)
            {
                snd_Switch_Toggle.Play();
                return;
            }
            sw_APUreset = 1;
            sw_GPU = 1;

            mdl_APU_Reset.SetActive(true);
            mdl_APU_Off.SetActive(false);
            mdl_APU_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_APU = 0;

            mdl_APU_Reset.SetActive(false);
            mdl_APU_Off.SetActive(true);
            mdl_APU_On.SetActive(false);

            snd_Switch_Toggle.Play();
        }
    }

    /// <summary>
    /// Toggle the transfer bus switch
    /// </summary>
    public void ToggleXfer()
    {
        //UpdateReq = true;
        if (sw_XferBus == 0)
        {
            sw_XferBus = 1;
            
                mdl_Xfer_On.SetActive(true);
                mdl_Xfer_Off.SetActive(false);
            snd_Switch_Toggle.Play();
        }
        else
        {
            sw_XferBus = 0;
            mdl_Xfer_Off.SetActive(true);
            mdl_Xfer_On.SetActive(false);
            XferTimer = 0.0f;
            snd_Switch_Toggle.Play();
        }
    }

    public void Set_GPU_On()
    {
        // TODO: Play CMDS sound
        GPU_Avail = true;

    }

    public void Set_GPU_Off()
    {
        // TODO: Play CMDS sound
        GPU_Avail = false;

        sw_GPU = 0;

        mdl_GPU_Off.SetActive(true);
        mdl_GPU_On.SetActive(false);

        snd_Switch_Toggle.Play();
    }
}
