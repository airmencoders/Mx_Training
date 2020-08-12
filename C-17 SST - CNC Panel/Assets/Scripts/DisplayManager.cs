using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public TextMesh txt_CurrentMode;
    public TextMesh txt_StandbyFreq;
    public TextMesh txt_ActiveFreq;
    public TextMesh txt_LeftSelect;
    public TextMesh txt_RightSelect;
    public TextMesh txt_1MS;
    public TextMesh txt_2MS;
    public TextMesh txt_3MS;
    public TextMesh txt_4MS;
    public TextMesh txt_5MS;
    public TextMesh txt_6MS;

    public enum mode
    {
        COM,
        UHF,
        VHF,
        HF,
        NAV,
        TAC,
        ADF,
        IFF
    }

    public mode CurrentMode = mode.COM;
    
    // COM Radios 1 & 2
    public int Com_RadioSelect = 1;
    public int HF_RadioSelect = 1;
    
    float[] freq_COM = new float[4]
    {
        200.0f,     // COM 1
        200.0f,
        200.0f,     // COM 2
        200.0f
    };

    int Nav_RadioSelect = 1;


    float[] freq_UHF = new float[2]
    {
        200.0f,
        200.0f
    };

    float[] freq_VHF = new float[2]
    {
        120.0f,
        120.0f
    };

    
    float[] freq_HF = new float[4]
    {
        1500.0f,
        1500.0f,
        2500.0f,
        2500.0f
    };

    float[] freq_NAV = new float[2]
    {
        200.0f,
        200.0f
    };
    float[] freq_TAC = new float[2]
    {
        200.0f,
        200.0f
    };
    float[] freq_ADF = new float[2]
    {
        200.0f,
        200.0f
    };
    float[] freq_IFF = new float[4]
    {
        1234.0f,        // Mode 3
        4321.0f,        // Mode 3
        12.0F,          // Mode 1
        21.0f           // Mode 1
    };


    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentMode == mode.COM)
        {
            if (Com_RadioSelect == 1)
            {
                txt_CurrentMode.text = "COM1";
                txt_StandbyFreq.text = freq_COM[0].ToString();
                txt_ActiveFreq.text = freq_COM[1].ToString();
            }
            else
            {
                txt_CurrentMode.text = "COM2";
                txt_StandbyFreq.text = freq_COM[2].ToString();
                txt_ActiveFreq.text = freq_COM[3].ToString();
            }

        }
        else if (CurrentMode == mode.UHF)
        {
            txt_CurrentMode.text = "UHF";
            txt_StandbyFreq.text = freq_UHF[1].ToString();
            txt_ActiveFreq.text = freq_UHF[2].ToString();
        }
        else if (CurrentMode == mode.VHF)
        {
            txt_CurrentMode.text = "VHF";
            txt_StandbyFreq.text = freq_VHF[1].ToString();
            txt_ActiveFreq.text = freq_VHF[2].ToString();
        }
        else if (CurrentMode == mode.HF)
        {
            if (HF_RadioSelect == 1)
            {
                txt_CurrentMode.text = "HF1";
                txt_StandbyFreq.text = freq_HF[0].ToString();
                txt_ActiveFreq.text = freq_HF[1].ToString();
            }
            else
            {
                txt_CurrentMode.text = "HF2";
                txt_StandbyFreq.text = freq_HF[2].ToString();
                txt_ActiveFreq.text = freq_HF[3].ToString();
            }
        }
        else if (CurrentMode == mode.NAV)
        {
            if (Nav_RadioSelect == 1)
            {
                txt_CurrentMode.text = "NAV1";
                txt_StandbyFreq.text = freq_NAV[1].ToString();
                txt_ActiveFreq.text = freq_NAV[2].ToString();
            }
            else
            {
                txt_CurrentMode.text = "NAV2";
                txt_StandbyFreq.text = freq_NAV[3].ToString();
                txt_ActiveFreq.text = freq_NAV[4].ToString();
            }
        }
        else if (CurrentMode == mode.TAC)
        {

        }
        else if (CurrentMode == mode.ADF)
        {

        }
        else if (CurrentMode == mode.IFF)
        {

        }
    }

    public void Chg_Mode(mode DesiredMode)
    {
        CurrentMode = DesiredMode;
    }
}
