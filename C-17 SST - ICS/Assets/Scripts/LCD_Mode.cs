using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCD_Mode : MonoBehaviour {

    bool debugMode = false;                  // Enables LCD Tests
    float debugDelay = 0.25f;                // Delay between switching
    float debugTimer = 0.0f;

    public GameObject LCD_Upper;
    public GameObject LCD_Lower;
    TextMesh LCD_UpperText;
    TextMesh LCD_LowerText;

    public enum type
    {
        radio,
        interphone,
        navigation,
        personel,
        error
    }

    public type currentType = type.radio;
    public int currentPosition = 0;             // NOTE: This is only used for radio modes



    // THESE COULD LIKELY WORK BETTER AS IT'S OWN SUB-Object
    public int radioMode = 0;
    int navMode = 0;
    int psnlMode = 0;
    int errorMode = 0;

    string[] radioModes = new string[]
    {
        "COM1",
        "COM2",
        "UHF ",
        "VHF ",
        "HF 1",
        "HF 2",
        "AER1",
        "AER2"
    };

    public int[] radio_Volumes = new int[]
    {
        5,
        5,
        5,
        5,
        5,
        5,
        5,
        5
    };

    string[] icsModes = new string[]
    {
        "ICS "
    };

    int[] icsVolumes = new int[]
    {
        5
    };

    string[] navModes = new string[]
    {
        "TAC ",
        "MB",
        "NAV1 ",
        "NAV2"
    };

    int[] navVolumes = new int[]
    {
        5,
        5,
        5,
        5
    };

    string[] psnlModes = new string[]
    {
        "HDST",
        "VOX ",
        "????"
    };

    int[] psnlVolumes = new int[]
    {
        5,
        5,
        5
    };

    string[] errorModes = new string[]
    {
        "CHCK\nSW  ",
        "CCU \nFAIL",
        "BKUP\nAUD "
    };

    // Use this for initialization
    void Start () {
        LCD_UpperText = LCD_Upper.GetComponent<TextMesh>();
        LCD_LowerText = LCD_Lower.GetComponent<TextMesh>();

        radioMode = currentPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if (debugMode == true) RunTests();

        switch (currentType)
        {
            case type.radio:
                LCD_UpperText.text = radioModes[radioMode];
                LCD_LowerText.text = "   " + radio_Volumes[radioMode].ToString();
                break;

            case type.interphone:
                LCD_UpperText.text = icsModes[0];
                LCD_LowerText.text = "   " + icsVolumes[0].ToString();
                break;

            case type.navigation:
                LCD_UpperText.text = navModes[navMode];
                LCD_LowerText.text = "   " + navVolumes[navMode].ToString();
                break;

            case type.personel:
                LCD_UpperText.text = psnlModes[psnlMode];
                LCD_LowerText.text = "   " + psnlVolumes[psnlMode].ToString();
                break;

            case type.error:
                LCD_UpperText.text = errorModes[errorMode];
                LCD_LowerText.text = "    ";
                break;

            default:
                break;
        }

	}

    void RunTests()
    {
        debugTimer += Time.deltaTime;
        if (debugTimer < debugDelay) return;

        debugTimer = 0.0f;
        switch (currentType)
        {
            case type.radio:
                radio_Volumes[radioMode] += 1;
                if (radio_Volumes[radioMode] > 9)
                {
                    radio_Volumes[radioMode] = 0;
                    radioMode++;
                    if (radioMode >= radioModes.Length) radioMode = 0;
                    radio_Volumes[radioMode] = 0;
                }
                break;

            case type.interphone:
                icsVolumes[0] += 1;
                if (icsVolumes[radioMode] > 9)
                {
                    icsVolumes[radioMode] = 0;
                }
                break;

            case type.navigation:
                navVolumes[navMode] += 1;
                if (navVolumes[navMode] > 9)
                {
                    navVolumes[navMode] = 0;
                    navMode++;
                    if (navMode >= navModes.Length) navMode = 0;
                    navVolumes[navMode] = 0;
                }
                break;

            case type.personel:
                psnlVolumes[psnlMode] += 1;
                if (psnlVolumes[psnlMode] > 9)
                {
                    psnlVolumes[psnlMode] = 0;
                    psnlMode++;
                    if (psnlMode >= psnlModes.Length) psnlMode = 0;
                    psnlVolumes[psnlMode] = 0;
                }
                break;

            case type.error:
                errorMode++;
                if (errorMode >= errorModes.Length) errorMode = 0;
                break;

            default:
                Debug.Log("ERROR: Invalid mode debugging selected!");
                break;
        }

        
    }
}
