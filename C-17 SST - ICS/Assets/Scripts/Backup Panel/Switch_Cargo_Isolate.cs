using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_Cargo_Isolate : MonoBehaviour {

    Transform t;
    Renderer r;

    public bool isOn = false;
    bool hasChanged = true;
    float timer = 0.0f;

    public Material Mat_Iso_On;
    public Material Mat_Iso_Off;

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
        if (hasChanged == false) return;                // No need to poll this, performance tweak

        // TODO: Add sounds
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


        if (isOn == true)
        {
            r.material = Mat_Iso_On;
        }
        else
        {
            
            r.material = Mat_Iso_Off;
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
