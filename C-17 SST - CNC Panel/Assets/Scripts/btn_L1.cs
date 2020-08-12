﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles animation and actions of desirted button
/// </summary>
public class btn_L1 : MonoBehaviour
{
    // Button Animation
    Vector3 startPos;
    Vector3 pushedPos;
    float pushTimerLimit = 0.20f;
    float pushTimer = 1.0f;

    // External objects
    DisplayManager displayManager;
    AudioSource ClickSound;

    void Start()
    {
        // Find objects
        displayManager = GameObject.Find("CNC Panel Manager").GetComponent<DisplayManager>();
        ClickSound = GameObject.Find("Snd_Click").GetComponent<AudioSource>();
        
        // Set animation positions
        startPos = this.transform.localPosition;
        pushedPos = startPos;
        pushedPos.x = pushedPos.x + 0.05f;
    }

    private void Update()
    {
        // Tracking time since last press
        pushTimer += Time.deltaTime;
        
        // Transform button based on timer
        if(pushTimer < pushTimerLimit)
        {
            this.transform.localPosition = pushedPos;
        }
        else
        {
            this.transform.localPosition = startPos;
        }
    }

    
    void OnMouseDown()
    {
        Debug.Log("L1 Pressed");
        pushTimer = 0.0f;           // Reset timer

        ClickSound.Play();          // Play sound
        

        // Check if we are already looking at the COM radio, and if so swap to next COM radio instead
        if (displayManager.CurrentMode == DisplayManager.mode.COM)
        {
            displayManager.Com_RadioSelect++;
            if (displayManager.Com_RadioSelect > 2) displayManager.Com_RadioSelect = 1;

        }
        else
        {
            displayManager.CurrentMode = DisplayManager.mode.COM;
        }
    }
}
