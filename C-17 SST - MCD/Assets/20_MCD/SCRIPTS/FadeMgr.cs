using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMgr : MonoBehaviour {

    // NOT USED

    public bool debugMode = true;
    public float fadeSpeed = 1.0f;

    float fadeAmt = 1.0f;
    bool fade = false;

    SpriteRenderer r;
    
	// Use this for initialization
	void Start () {
        r = this.GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
		if (fade == false)
        {
            if (fadeAmt > 0.0f)
            {
                fadeAmt -= Time.deltaTime;
                if (fadeAmt < 0.0f) fadeAmt = 0.0f;
            }
        }
        else
        {
            if (fadeAmt < 1.0f)
            {
                fadeAmt += Time.deltaTime;
                if (fadeAmt > 1.0f) fadeAmt = 1.0f;
            }
        }
        r.color = new Vector4(1.0f, 1.0f, 1.0f, fadeAmt);
        
	}

    private void OnGUI()
    {
        if (debugMode == true)
        {
            if (GUI.Button(new Rect(sX(0.0f), sY(0.0f), sX(1.0f), sY(0.4f)), "Fade to Black"))
            {
                fade = true;
            }

            if (GUI.Button(new Rect(sX(0.0f), sY(0.6f), sX(1.0f), sY(0.4f)), "Fade off"))
            {
                fade = false;
            }
        }
    }

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
