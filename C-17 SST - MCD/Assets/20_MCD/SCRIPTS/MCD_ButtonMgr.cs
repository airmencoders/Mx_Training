using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to manage button animations, and shading if it is active
/// </summary>
public class MCD_ButtonMgr : MonoBehaviour {

    GameObject sndOK;
    GameObject sndFAIL;

    AudioSource s_sndOK;
    AudioSource s_sndFAIL;

    Renderer r;         // Used to get button's render info
    Transform t;
    bool btnOn = true;

    float timer = 0.0f;
    float dist = 0.5f;

    Vector3 startPos;
    

    // Use this for initialization
	void Start () {
        r = this.GetComponent<Renderer>();
        t = this.GetComponent<Transform>();
        startPos = t.localPosition;
        sndOK = GameObject.Find("clickOK");
        sndFAIL = GameObject.Find("clickFail");

        s_sndOK = sndOK.GetComponent<AudioSource>();
        s_sndFAIL = sndFAIL.GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {
        if (timer >= 0.0f)
        {
            t.localPosition = new Vector3(startPos.x, startPos.y, -2.70f);
            return;
        }
        else
        {
            t.localPosition = new Vector3(startPos.x, startPos.y, -2.04f);
        }
        timer += Time.deltaTime;

    }

    public void Push()
    {
        if (btnOn == false)
        {
            if (s_sndFAIL.isPlaying == false) s_sndFAIL.Play();
            timer = -0.15f;
            return;
        }
        else
        {
            if (s_sndOK.isPlaying == false) s_sndOK.Play();
            timer = -0.15f;
            return;
        }
        
        
    }

    public void Off()
    {
        btnOn = false;
        r.material.color = new Vector4(0.25f, 0.25f, 0.25f, 1.0f);
    }

    public void On()
    {
        btnOn = true;
        r.material.color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
