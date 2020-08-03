using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ICS_AudioScript : MonoBehaviour {

    public bool COM1_Enabled = false;
    public bool COM2_Enabled = false;
    public bool UHF_Enabled = false;
    public bool VHF_Enabled = false;
    public bool HF1_Enabled = false;
    public bool HF2_Enabled = false;

    public bool MarkerBeacon_Enabled = false;
    public bool TACAN_Enabled = false;
    public bool ADF_Enabled = false;

    // Sounds
    AudioSource COM1;
    AudioSource COM2;
    AudioSource UHF;
    AudioSource VHF;
    AudioSource HF1;
    AudioSource HF2;

    AudioSource MarkerBeacon;
    AudioSource TACAN;
    AudioSource ADF;

    // Volume Control
    public float Volume_COM1 = 0.5f;
    public float Volume_COM2 = 0.5f;
    public float Volume_UHF = 0.5f;
    public float Volume_VHF = 0.5f;
    public float Volume_HF1 = 0.5f;
    public float Volume_HF2 = 0.5f;

    public float Volume_MarkerBeacon = 0.5f;
    public float Volume_TACAN = 0.5f;
    public float Volume_ADF = 0.5f;
    
    // Timing and other variables
    float timer = 0.0f;


	// Use this for initialization
	void Start () {
        COM1 = GameObject.Find("~COM1").GetComponent<AudioSource>();
        COM2 = GameObject.Find("~COM2").GetComponent<AudioSource>();
        UHF = GameObject.Find("~UHF").GetComponent<AudioSource>();
        VHF = GameObject.Find("~VHF").GetComponent<AudioSource>();
        HF1 = GameObject.Find("~HF1").GetComponent<AudioSource>();
        HF2 = GameObject.Find("~HF2").GetComponent<AudioSource>();

        MarkerBeacon = GameObject.Find("~MB").GetComponent<AudioSource>();
        TACAN = GameObject.Find("~TACAN").GetComponent<AudioSource>();
        ADF = GameObject.Find("~ADF").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        COM1_Manager();
        COM2_Manager();
        UHF_Manager();
        VHF_Manager();
        HF1_Manager();
        HF2_Manager();
        MarkerBeacon_Manager();
        ADF_Manager();
        TACAN_Manager();
        SetVolumes();
    }

    void SetVolumes()
    {
        COM1.volume = Volume_COM1;
        COM2.volume = Volume_COM2;
        UHF.volume = Volume_UHF;
        VHF.volume = Volume_VHF;
        HF1.volume = Volume_HF1;
        HF2.volume = Volume_HF2;

        MarkerBeacon.volume = Volume_MarkerBeacon;
        TACAN.volume = Volume_TACAN;
        ADF.volume = Volume_ADF;
    }

    void COM1_Manager()
    {
        if (COM1.isPlaying == false && COM1_Enabled == true)
        {
            COM1.time = timer % COM1.clip.length;
            Debug.Log(COM1.time);
            COM1.Play();
        }
        else if (COM1.isPlaying == true && COM1_Enabled == false)
        {
            COM1.Stop();
        }
    }

    void COM2_Manager()
    {
        if (COM2.isPlaying == false && COM2_Enabled == true)
        {
            COM2.time = timer % COM2.clip.length;
            Debug.Log(COM2.time);
            COM2.Play();
        }
        else if (COM2.isPlaying == true && COM2_Enabled == false)
        {
            COM2.Stop();
        }
    }

    void UHF_Manager()
    {
        if (UHF.isPlaying == false && UHF_Enabled == true)
        {
            UHF.time = timer % UHF.clip.length;
            Debug.Log(UHF.time);
            UHF.Play();
        }
        else if (UHF.isPlaying == true && UHF_Enabled == false)
        {
            UHF.Stop();
        }
    }

    void VHF_Manager()
    {
        if (VHF.isPlaying == false && VHF_Enabled == true)
        {
            VHF.time = timer % VHF.clip.length;
            Debug.Log(VHF.time);
            VHF.Play();
        }
        else if (VHF.isPlaying == true && VHF_Enabled == false)
        {
            VHF.Stop();
        }
    }

    void HF1_Manager()
    {
        if (HF1.isPlaying == false && HF1_Enabled == true)
        {
            HF1.time = timer % HF1.clip.length;
            Debug.Log(HF1.time);
            HF1.Play();
        }
        else if (HF1.isPlaying == true && HF1_Enabled == false)
        {
            HF1.Stop();
        }
    }

    void HF2_Manager()
    {
        if (HF2.isPlaying == false && HF2_Enabled == true)
        {
            HF2.time = timer % HF2.clip.length;
            Debug.Log(HF2.time);
            HF2.Play();
        }
        else if (HF2.isPlaying == true && HF2_Enabled == false)
        {
            HF2.Stop();
        }
    }

    void MarkerBeacon_Manager()
    {
        if (MarkerBeacon.isPlaying == false && MarkerBeacon_Enabled == true)
        {
            MarkerBeacon.time = timer % MarkerBeacon.clip.length;
            Debug.Log(MarkerBeacon.time);
            MarkerBeacon.Play();
        }
        else if (MarkerBeacon.isPlaying == true && MarkerBeacon_Enabled == false)
        {
            MarkerBeacon.Stop();
        }
    }

    void TACAN_Manager()
    {
        if (TACAN.isPlaying == false && TACAN_Enabled == true)
        {
            TACAN.time = timer % TACAN.clip.length;
            Debug.Log(TACAN.time);
            TACAN.Play();
        }
        else if (TACAN.isPlaying == true && TACAN_Enabled == false)
        {
            TACAN.Stop();
        }
    }

    void ADF_Manager()
    {
        if (ADF.isPlaying == false && ADF_Enabled == true)
        {
            ADF.time = timer % ADF.clip.length;
            Debug.Log(ADF.time);
            ADF.Play();
        }
        else if (ADF.isPlaying == true && ADF_Enabled == false)
        {
            ADF.Stop();
        }
    }
}
