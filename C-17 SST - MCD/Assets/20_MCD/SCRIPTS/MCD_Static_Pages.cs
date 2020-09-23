using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a storage container to store all MCD strings, we can't load files at runtime in WebGL
public class MCD_Static_Pages : MonoBehaviour {
    public TextAsset comm_Msg_Summary;
    public TextAsset comm_Main_HF_Off;
    public TextAsset comm_Main_HF_1_On;
    public TextAsset comm_Main_HF_2_On;
    public TextAsset comm_Main_HF_On;
    public TextAsset comm_HF_Off;
    public TextAsset comm_HF_1_On;
    public TextAsset comm_HF_2_On;
    public TextAsset comm_HF_On;
    public TextAsset comm_Index1;
    public TextAsset comm_Index2;
    public TextAsset comm_Maint;

    public TextAsset Msn_EgtOvertemp;
    public TextAsset msn_maint;
    public TextAsset msn_maint_apdms2;
    public TextAsset msn_maint_apdms2_EgtHist;
    public TextAsset msn_maint_apdms2_EgtHist_Eng1;
    public TextAsset msn_maint_apdms2_EgtHist_Eng2;
    public TextAsset msn_maint_apdms2_EgtHist_Eng3;
    public TextAsset msn_maint_apdms2_EgtHist_Eng4;
    public TextAsset msn_maint_apdms2_fault_main;
    public TextAsset msn_maint_apdms2_fault_list;
    public TextAsset msn_maint_apdms2_faults_prop2;
    public TextAsset msn_maint_apdms2_faults_prop2_eng1;
    public TextAsset msn_maint_apdms2_faults_prop2_eng2;
    public TextAsset msn_maint_apdms2_faults_prop2_eng3;
    public TextAsset msn_maint_apdms2_faults_prop2_eng4;

    public TextAsset msn_maint_EFCS_main;
    public TextAsset msn_maint_EFCS_FaultList;
    public TextAsset msn_maint_EFCS_FaultHistory;

    public TextAsset msn_maint_scefcs_main;
    public TextAsset msn_maint_scefcs_FaultList;
    public TextAsset msn_maint_scefcs_FaultHistory;

}
