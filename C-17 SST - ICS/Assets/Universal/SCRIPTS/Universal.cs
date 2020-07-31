using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Repetitive high-use functions go here
/// </summary>
public class Universal : MonoBehaviour {

    /// <summary>
    /// sX(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">X position from left to right</param>
    /// <returns></returns>    
    public float sX(float i)
    {
        i = (float)Screen.width * i;
        return i;
    }


    /// <summary>
    /// sY(0.0 to 1.0) - GUI Screen Width Scaling 
    /// </summary>
    /// <param name="i">Y position from top to bottom</param>
    /// <returns></returns>    
    public float sY(float i)
    {
        i = (float)Screen.height * i;
        return i;
    }
}
