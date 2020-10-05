using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Aft_LM_Manager : MonoBehaviour
{
    public GameObject mdl_Fire_Control_Off;
    public GameObject mdl_Fire_Control_Arm;
    public GameObject mdl_Fire_Control_Norm;

    public Button btn_Fire_Control_Off;
    public Button btn_Fire_Control_Arm;
    public Button btn_Fire_Control_Norm;

    bool Update_Fire_Control = true;
    int sw_Fire_Control = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f) sw_Fire_Control = 0;       // Random chance of switch being in wrong position
    }

    // Update is called once per frame
    void Update()
    {
        if (Update_Fire_Control) Move_Fire_Control();
    }

    /// <summary>
    /// Moves the switch if an update is needed
    /// </summary>
    void Move_Fire_Control()
    {
        if(sw_Fire_Control == 0)
        {
            mdl_Fire_Control_Off.SetActive(true);
            mdl_Fire_Control_Arm.SetActive(false);
            mdl_Fire_Control_Norm.SetActive(false);

            ColorBlock colors = btn_Fire_Control_Off.colors;
            colors.normalColor = new Color32(0,255,0,60);
            btn_Fire_Control_Off.colors = colors;

            colors = btn_Fire_Control_Arm.colors;
            colors.normalColor = new Color32(255, 255, 0, 60);
            btn_Fire_Control_Arm.colors = colors;

            colors = btn_Fire_Control_Norm.colors;
            colors.normalColor = new Color32(255, 255, 0, 90);
            btn_Fire_Control_Norm.colors = colors;
        }
        else if (sw_Fire_Control == 1)
        {
            mdl_Fire_Control_Off.SetActive(false);
            mdl_Fire_Control_Arm.SetActive(true);
            mdl_Fire_Control_Norm.SetActive(false);

            ColorBlock colors = btn_Fire_Control_Off.colors;
            colors.normalColor = new Color32(255, 255, 0, 60);
            btn_Fire_Control_Off.colors = colors;

            colors = btn_Fire_Control_Arm.colors;
            colors.normalColor = new Color32(0, 255, 0, 60);
            btn_Fire_Control_Arm.colors = colors;

            colors = btn_Fire_Control_Norm.colors;
            colors.normalColor = new Color32(255, 255, 0, 90);
            btn_Fire_Control_Norm.colors = colors;
        }
        else
        {
            mdl_Fire_Control_Off.SetActive(false);
            mdl_Fire_Control_Arm.SetActive(false);
            mdl_Fire_Control_Norm.SetActive(true);

            ColorBlock colors = btn_Fire_Control_Off.colors;
            colors.normalColor = new Color32(255, 255, 0, 60);
            btn_Fire_Control_Off.colors = colors;

            colors = btn_Fire_Control_Arm.colors;
            colors.normalColor = new Color32(255, 255, 0, 60);
            btn_Fire_Control_Arm.colors = colors;

            colors = btn_Fire_Control_Norm.colors;
            colors.normalColor = new Color32(0, 255, 0, 90);
            btn_Fire_Control_Norm.colors = colors;
        }

        Update_Fire_Control = false;
    }

    public void Set_FireControl(int i)
    {
        Update_Fire_Control = true;
        sw_Fire_Control = i;
    }
}
