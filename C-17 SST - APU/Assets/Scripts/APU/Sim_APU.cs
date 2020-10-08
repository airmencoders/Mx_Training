using System;       // Used to get time/date
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sim_APU : MonoBehaviour
{
    // Debugging
    public bool Debug_External_Power = true;
    public bool Debug_Fuel_Pressure = false;

    // Public Objects
    public TextMeshPro APU_RPM;
    public TextMeshPro APU_EGT;

    public Pwr_Tied_Light Ind_RPM_High;
    public Pwr_Tied_Light Ind_EGT_High;
    public Pwr_Tied_Light Ind_Low_FuelPress;
    public Pwr_Tied_Light Ind_Autoshutdown;
    public Pwr_Tied_Light Ind_Armed;
    public Pwr_Tied_Light Ind_Discharge;

    public GameObject Mdl_APU_Off;
    public GameObject Mdl_APU_Run;
    public GameObject Mdl_APU_Start;

    public WAP_Manager wap;
    public Panel_Electric elec_manager;

    // Switches
    int sw_APU_Mode_Toggle = 0;
    float sw_APU_Start_Timer = 0.0f;

    bool sw_Auto_Shutdown_Override = false;
    public bool sw_Run = false;
    public bool sw_Start = false;
    bool sw_offArm = false;
    bool sw_Ext_Disch = false;

    // Status
    public float RPM = 0.0f;
    float RPM_Target = 0.0f;
    float EGT = 0.0f;
    float EGT_Target = 0.0f;
    bool ind_High = false;
    bool ind_Hot = false;

    // Startup sequence variables
    bool status_StartSeq = false;
    public int status_StartSeq_Step = 0;
    float seqTimer = 0.0f;

    bool fault_Overspeed = false;
    bool fault_Fire = false;
    bool status_OnFire = false;
    float timer_OnFire = 0.0f;


    // EGT calcs
    float Temp_Ambient = 30.0f;
    float Temp_Target = 30.0f;

    float Temp_Load_Elec = 20.0f;
    float[] Temp_Load_Packs = new float[3]
    {
        0.0f,
        80.0f,
        160.0f
    };
    
    float[] chart_EGT_Norm = new float[23]      // use linear interpolation, keep it simple
    {
        0,
        0,
        0,
        0,
        650,
        720,
        710,
        700,
        770,
        760,
        750,
        720,
        690,
        650,
        605,
        550,
        500,
        450,
        430,
        400,
        372,
        372,
        372
    };

    float[] chart_EGT_Overspeed = new float[23]    // We'll use this for manifold fail as well due to loss of output pressure 
    {
        0,
        0,
        0,
        0,
        650,
        720,
        710,
        700,
        770,
        760,
        750,
        720,
        690,
        650,
        605,
        550,
        500,
        450,
        430,
        440,
        432,
        422,
        402
    };

    float[] chart_EGT_Fire = new float[23]      // APU is on fire
    {
        999,
        950,
        870,
        800,
        650,
        720,
        710,
        700,
        770,
        760,
        750,
        720,
        690,
        650,
        605,
        600,
        610,
        610,
        650,
        670,
        692,
        662,
        612
    };

// External Objects
    public TextMeshPro disp_EGT;
    public TextMeshPro disp_RPM;

    public AudioSource APU_Start;
    public AudioSource APU_Loop;
    public AudioSource APU_End;

    

    // Start is called before the first frame update
    void Start()
    {
        GetFuzzy();     // Randomize a few things to make things more "exciting"
        if(UnityEngine.Random.Range(0.0f, 1.0f) < 0.2f) sw_Auto_Shutdown_Override = true;
        
        chart_EGT_Overspeed = chart_EGT_Norm;
        chart_EGT_Fire = chart_EGT_Norm;
    }

    // Update is called once per frame
    void Update()
    {
        CalcPhysics();
        
        //if (!Debug_External_Power) return;
        UpdateIndicators();
        

        if(sw_offArm || !sw_Run)
        {
            RPM_Target = 0.0f;
            status_StartSeq = false;
            status_StartSeq_Step = 0;
            
            return;
        }

        if(sw_Run)
        {
            if (sw_Start) status_StartSeq = true;
            return;
        }
    }


    /// <summary>
    /// Updates indicator lights
    /// </summary>
    void UpdateIndicators()
    {
        //Sets the main toggle switch position
        if(sw_APU_Mode_Toggle == 0)
        {
            Mdl_APU_Off.SetActive(true);
            Mdl_APU_Run.SetActive(false);
            Mdl_APU_Start.SetActive(false);
        }
        else if(sw_APU_Mode_Toggle == 1)
        {
            Mdl_APU_Off.SetActive(false);
            Mdl_APU_Run.SetActive(true);
            Mdl_APU_Start.SetActive(false);
        }
        else if(sw_APU_Mode_Toggle == 2 && sw_APU_Start_Timer > 0.0f)
        {
            Mdl_APU_Off.SetActive(false);
            Mdl_APU_Run.SetActive(false);
            Mdl_APU_Start.SetActive(true);
            sw_APU_Start_Timer -= Time.deltaTime;
        }
        else
        {
            sw_APU_Mode_Toggle = 1;
        }

        // Sets all indicators & displays
        if (Debug_External_Power)
        {
            disp_EGT.text = Mathf.Round(EGT).ToString();
            disp_RPM.text = Mathf.Round(RPM).ToString();

            if(sw_APU_Mode_Toggle >= 1)
            {
                if (Debug_Fuel_Pressure) Ind_Low_FuelPress.SetMaterial(0);
                else Ind_Low_FuelPress.SetMaterial(1);

                if (sw_Auto_Shutdown_Override) Ind_Autoshutdown.SetMaterial(1);
                else Ind_Autoshutdown.SetMaterial(0);

                if (sw_Ext_Disch) Ind_Discharge.SetMaterial(1);
                else Ind_Discharge.SetMaterial(0);

                if (sw_offArm) Ind_Armed.SetMaterial(1);
                else Ind_Armed.SetMaterial(0);
            }
            else
            {
                Ind_Low_FuelPress.SetMaterial(0);
                Ind_Autoshutdown.SetMaterial(0);
                Ind_Discharge.SetMaterial(0);
                Ind_Armed.SetMaterial(0);
            }
        }
        else // No power to panel, disable indicators. TODO: Check if fire switches tie to battery bus instead of mode switch
        {
            Ind_Low_FuelPress.SetMaterial(0);
            Ind_Autoshutdown.SetMaterial(0);
            Ind_Discharge.SetMaterial(0);
            Ind_Armed.SetMaterial(0);
            
        }
    }

    /// <summary>
    /// Calculate acceleration/temp change rates
    /// </summary>
    void CalcPhysics()
    {
        int chartReference = Mathf.FloorToInt(RPM / 5.0f);

        EGT_Target = chart_EGT_Norm[chartReference];

        if(status_StartSeq)
        {
            if(sw_APU_Mode_Toggle == 0 && status_StartSeq_Step == 5)
            {
                status_StartSeq_Step = 6;

            }
            
            if(RPM < 0.1f && status_StartSeq_Step == 0)      // Check if we at a low enough RPM to start
            {
                status_StartSeq_Step = 1;
            }
            /*
            else                // ABORT start sequence
            {
                status_StartSeq_Step = 0;
                status_StartSeq = false;
                RPM_Target = 0.0f;
            }*/

            if (status_StartSeq_Step == 1)       // Set light flash timer
            {
                seqTimer = 0.0f;
                status_StartSeq_Step = 2;
                Debug.Log("Start initiated");
            }
            else if (status_StartSeq_Step == 2) // Flash lights
            {
                if (seqTimer < 0.5f)
                {
                    Ind_EGT_High.SetMaterial(0);
                    Ind_RPM_High.SetMaterial(0);
                }
                else if (seqTimer < 1.0f)
                {
                    Ind_EGT_High.SetMaterial(1);
                    Ind_RPM_High.SetMaterial(1);
                }
                else if (seqTimer < 1.5f)
                {
                    Ind_EGT_High.SetMaterial(0);
                    Ind_RPM_High.SetMaterial(0);
                }
                else if (seqTimer < 2.0f)
                {
                    status_StartSeq_Step = 4;
                }

                seqTimer += Time.deltaTime;
            }
            else if (status_StartSeq_Step == 3)
            {
                // BIT COMPLETE, 1 sec delay
            }

            else if (status_StartSeq_Step == 4)
            {
                if (APU_Start.isPlaying == false) APU_Start.Play();
                RPM_Target = 100.0f;
                wap.AddCue("APU ");
                status_StartSeq_Step = 5;   // Enter running sequence
                
            }
            else if (status_StartSeq_Step == 5)
            {
                if (APU_Start.isPlaying == false && APU_Loop.isPlaying == false) APU_Loop.Play();
            }
            else if (status_StartSeq_Step == 6)
            {
                Debug.Log("Shutdown Initated via mode switch");
                if (RPM > 15.0f)
                {
                    APU_Start.Stop();
                    APU_Loop.Stop();
                    APU_End.Play();
                }
                else
                {
                    APU_Start.Stop();
                    APU_Loop.Stop();
                    APU_End.Stop();
                }
                seqTimer = 0.0f;
                status_StartSeq_Step = 7;

            }
            else if (status_StartSeq_Step == 7)
            {
                seqTimer += Time.deltaTime;
                if (seqTimer > 1.0f) status_StartSeq_Step = 8;
            }
            else if (status_StartSeq_Step == 8)
            {
                sw_Run = false;
                sw_Start = false;
            }






            }

        if (RPM > RPM_Target)
        {
            RPM -= Time.deltaTime * 30.0f;
            
            if (RPM < 0.0f) RPM = 0.0f;
        } 
        else
        {
            RPM += Time.deltaTime * 5.0f;
        }

        float EGT_AdjTarget = EGT_Target + Temp_Ambient;
        if (elec_manager.sw_APU == 2) EGT_AdjTarget += 25.0f;

        if (EGT > EGT_AdjTarget)
        {
            EGT -= Time.deltaTime * 100.0f;

            if (EGT < EGT_AdjTarget) EGT = EGT_AdjTarget;
        }
        else
        {
            EGT += Time.deltaTime * 250.0f;
        }
    }

    /// <summary>
    /// Adds a EGT fuzz factor, date only for now
    /// </summary>
    void GetFuzzy()
    {
        switch (DateTime.Now.Month)
        {
            case 1:
                Temp_Ambient = UnityEngine.Random.Range(0.0f, 10.0f);
                break;
            case 2:
                Temp_Ambient = UnityEngine.Random.Range(5.0f, 15.0f);
                break;
            case 3:
                Temp_Ambient = UnityEngine.Random.Range(15.0f, 25.0f);
                break;
            case 4:
                Temp_Ambient = UnityEngine.Random.Range(25.0f, 35.0f);
                break;
            case 5:
                Temp_Ambient = UnityEngine.Random.Range(35.0f, 40.0f);
                break;
            case 6:
                Temp_Ambient = UnityEngine.Random.Range(31.0f, 36.0f);
                break;
            case 7:
                Temp_Ambient = UnityEngine.Random.Range(35.0f, 40.0f);
                break;
            case 8:
                Temp_Ambient = UnityEngine.Random.Range(31.0f, 39.0f);
                break;
            case 9:
                Temp_Ambient = UnityEngine.Random.Range(25.0f, 37.0f);
                break;
            case 10:
                Temp_Ambient = UnityEngine.Random.Range(15.0f, 30.0f);
                break;
            case 11:
                Temp_Ambient = UnityEngine.Random.Range(15.0f, 25.0f);
                break;
            case 12:
                Temp_Ambient = UnityEngine.Random.Range(0.0f, 15.0f);
                break;
            default:
                Debug.Log("Invalid month for setting ambient temp!");
                break;
        }

    }

    #region Public_Switches
    /// <summary>
    /// Move switch down
    /// </summary>
    public void APU_Sw_Down()
    {
        if (sw_APU_Mode_Toggle == 0)
        {
            sw_APU_Mode_Toggle = 1;
            sw_Run = true;
            sw_Start = false;
        }
        else if (sw_APU_Mode_Toggle == 1)
        {
            sw_APU_Mode_Toggle = 2;
            sw_APU_Start_Timer = 0.5f;
            sw_Start = true;
        }
        else
        {
            sw_APU_Mode_Toggle = 2;
            sw_APU_Start_Timer = 0.5f;
            
        }

    }

    /// <summary>
    /// Move switch up
    /// </summary>
    public void APU_Sw_Up()
    {
        if (sw_APU_Mode_Toggle > 0) sw_APU_Mode_Toggle--;
        else return;

        if (sw_APU_Mode_Toggle == 0)
        {
            sw_Run = false;
            sw_Start = false;
            return;
        }
        


    }

    public void toggleAutoShutDown()
    {
        sw_Auto_Shutdown_Override = !sw_Auto_Shutdown_Override;
    }

    public void toggleOffArm()
    {
        sw_offArm = !sw_offArm;
    }

    public void dischargeExtinguisher()
    {
        sw_Ext_Disch = true;
    }
    #endregion
}
