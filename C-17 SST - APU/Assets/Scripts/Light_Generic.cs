using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Super generic toggle indicator
/// </summary>
public class Light_Generic : MonoBehaviour {

    Renderer r;

    public bool isOn = false;
    public bool hasChanged = false;

    public Material Mat_Off;
    public Material Mat_On;

    void Start()
    {
        r = GetComponent<Renderer>();
        if(isOn) r.material = Mat_On;           // Option to have light on immediately, there if needed.
    }


    void Update()
    {
        if (!hasChanged) return;
        
        if(isOn)
        {
            r.material = Mat_On;
        }
        else 
        { 
            r.material = Mat_Off;
        }

        hasChanged = false;
    }

    /// <summary>
    /// Turns light on
    /// </summary>
    public void turnOn()
    {
        if (isOn) return;       // Don't attempt to cycle texture if it's on already
        isOn = true;
        hasChanged = true;      // Queue to cycle texture
    }

    /// <summary>
    /// Turns light off
    /// </summary>
    public void turnOff()
    {
        if (!isOn) return;      // Don't attempt to cycle texture if it's off already
        isOn = false;
        hasChanged = true;      // Queue to cycle texture
    }
}
