using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class Sim_APU : MonoBehaviour
{
    // Switches
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

    bool status_StartSeq = false;
    int status_StartSeq_Step = 0;

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
    public GameObject light_High;
    public GameObject light_Hot;
    public TextMeshPro disp_EGT;
    public TextMeshPro disp_RPM;

    public AudioSource APU_Start;
    public AudioSource APU_Loop;
    public AudioSource APU_End;

    // Start is called before the first frame update
    void Start()
    {
        chart_EGT_Overspeed = chart_EGT_Norm;
        chart_EGT_Fire = chart_EGT_Norm;
    }

    // Update is called once per frame
    void Update()
    {
        if(sw_offArm || !sw_Run)
        {
            RPM_Target = 0.0f;
            status_StartSeq = false;
            status_StartSeq_Step = 0;
            CalcPhysics();
            return;
        }

        if(sw_Run)
        {
            disp_RPM.text = Mathf.Round(RPM).ToString();
            disp_EGT.text = Mathf.Round(EGT).ToString();

            if (sw_Start) status_StartSeq = true;
            CalcPhysics();
            return;
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
            if(RPM < 0.1f)      // Check if we at a low enough RPM to start
            {
                status_StartSeq_Step = 4;
            }
            /*
            else                // ABORT start sequence
            {
                status_StartSeq_Step = 0;
                status_StartSeq = false;
                RPM_Target = 0.0f;
            }*/

            if(status_StartSeq_Step == 1)
            {
                // START TIMER
            }
            else if (status_StartSeq_Step == 2)
            {
                // FLASH LIGHTS
            }
            else if (status_StartSeq_Step == 3)
            {
                // BIT COMPLETE, 1 sec delay
            }

            else if (status_StartSeq_Step == 4)
            {
                if (APU_Start.isPlaying == false) APU_Start.Play();
                RPM_Target = 100.0f;
                status_StartSeq_Step = 5;
            }
            else if (status_StartSeq_Step == 5)
            {
                if (APU_Start.isPlaying == false && APU_Loop.isPlaying == false) APU_Loop.Play();
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

        if (EGT > EGT_Target)
        {
            EGT -= Time.deltaTime * 100.0f;

            if (EGT < 0.0f) EGT = 0.0f;
        }
        else
        {
            EGT += Time.deltaTime * 250.0f;
        }
    }

    /// <summary>
    /// Adds a EGT fuzz factor, for APU wear, pack, and generator variation
    /// </summary>
    void GetFuzzy()
    {

    }
}
