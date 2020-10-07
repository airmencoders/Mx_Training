using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enviro_Panel_Manager : MonoBehaviour
{
    // Debugging
    public bool Ext_Power;
    public bool APU_Air_Avail;

    // Public GameObjects
    public AudioSource APU_Test_Loop;
    public WAP_Manager WAP;


    public Pwr_Tied_Light Ind_LoopA_Eng_1;
    public Pwr_Tied_Light Ind_LoopA_Eng_2;
    public Pwr_Tied_Light Ind_LoopA_Eng_3;
    public Pwr_Tied_Light Ind_LoopA_Eng_4;
    public Pwr_Tied_Light Ind_LoopA_APU;

    public Pwr_Tied_Light Ind_LoopB_Eng_1;
    public Pwr_Tied_Light Ind_LoopB_Eng_2;
    public Pwr_Tied_Light Ind_LoopB_Eng_3;
    public Pwr_Tied_Light Ind_LoopB_Eng_4;
    public Pwr_Tied_Light Ind_LoopB_APU;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Test_LoopA(int i)
    {
        Ind_LoopA_Eng_1.SetMaterial(i);
        Ind_LoopA_Eng_2.SetMaterial(i);
        Ind_LoopA_Eng_3.SetMaterial(i);
        Ind_LoopA_Eng_4.SetMaterial(i);
        Ind_LoopA_APU.SetMaterial(i);
    }

    public void Test_LoopB(int i)
    {
        Ind_LoopB_Eng_1.SetMaterial(i);
        Ind_LoopB_Eng_2.SetMaterial(i);
        Ind_LoopB_Eng_3.SetMaterial(i);
        Ind_LoopB_Eng_4.SetMaterial(i);
        Ind_LoopB_APU.SetMaterial(i);
    }
}
