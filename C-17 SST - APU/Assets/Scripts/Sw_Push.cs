using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Super generic switch, abuses texture memory, but runs faster than generating on the fly
/// </summary>
public class Sw_Push : MonoBehaviour {

    Transform t;
    Renderer r;

    public bool isOn = false;
    bool hasChanged = true;
    float timer = 0.0f;

    public Material Mat_Off;
    public Material Mat_On;
    public Material Mat_Upper;
    public Material Mat_Lower;

    public bool splitSwitch = false;
    public bool illumUpper = false;
    public bool illumLower = false;

    Vector3 startPos;

    AudioSource snd_Switched;

    void Start()
    {
        t = GetComponent<Transform>();
        r = GetComponent<Renderer>();
        startPos = t.position;
        snd_Switched = GameObject.Find("Sound - Switch Switched").GetComponent<AudioSource>();
    }


    void Update()
    {
        if(timer >= 0.0f)
        {
            hasChanged = false;
            t.position = new Vector3(startPos.x, startPos.y, startPos.z);
        }
        else
        {
            t.position = new Vector3(startPos.x, startPos.y, startPos.z + 0.2f);
            timer += Time.deltaTime;
        }

        if(illumUpper & illumLower)
        {
            r.material = Mat_On;
        }
        else if (illumUpper)
        {
            r.material = Mat_Upper;
        }
        else if (illumLower && splitSwitch)
        {
            r.material = Mat_Lower;
        }
        else
        {
            r.material = Mat_Off;
        }
    }


    private void OnMouseDown()
    {
        isOn = !isOn;
        hasChanged = true;
        timer = -0.05f;
        snd_Switched.Play();
    }
}
