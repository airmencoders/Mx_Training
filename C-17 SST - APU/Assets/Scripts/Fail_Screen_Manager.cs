using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the student failure screen, provides fading and other effects
/// </summary>
public class Fail_Screen_Manager : MonoBehaviour
{
    
    float Timer = -1.0f;     // Fade/click enable timer
    float ColorToggleTimer = 0.0f;   // Change position/color timer

    // Color Text Variables
    Vector3 startingPos;

    // External Objects
    public Text Text_Title;
    public Text Text_Description;
    public Text Text_Exit;
    public Transform T_Text_Exit;




    // Start is called before the first frame update
    void Start()
    {
        startingPos = T_Text_Exit.position;
    }

    // Update is called once per frame
    void Update()
    {
        float colorAlpha = Timer;
        if (colorAlpha < 0.0f) colorAlpha = 0.0f;
        else if (colorAlpha > 1.0f) colorAlpha = 1.0f;
        
        // Change text fading level
        Text_Title.color = new Vector4(Text_Title.color.r, Text_Title.color.g, Text_Title.color.b, colorAlpha);
        Text_Description.color = new Vector4(Text_Description.color.r, Text_Description.color.g, Text_Description.color.b, colorAlpha);
        

        // Cycle color and position of 
        if(Timer >= 1.0f && ColorToggleTimer > 0.02f)
        {
            
            Text_Exit.color = new Vector4(Random.Range(0.5f, 0.9f), Random.Range(0.5f, 0.9f), Random.Range(0.5f, 0.9f), colorAlpha);
            T_Text_Exit.position = new Vector3(startingPos.x + Random.Range(0.0f, 5.0f), startingPos.y + Random.Range(0.0f, 5.0f), startingPos.z);
            ColorToggleTimer = 0.0f;
        }
        else if (Timer < 1.0f) // Just hide it
        {
            Text_Exit.color = new Vector4(Text_Title.color.r, Text_Title.color.g, Text_Title.color.b, 0.0f);
        }
        
        // Increment Timers
        Timer += Time.deltaTime;
        ColorToggleTimer += Time.deltaTime;
        
    }

    public void RestartSim()
    {
        if (Timer < 1.0f) return;
        SceneManager.LoadScene("APU Simulator");
    }
}
