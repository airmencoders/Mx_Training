using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICS_Mic_Switch : MonoBehaviour {

    public bool MicOn = false;


    // Render Stuff
    public Material textureOn;
    public Material textureOff;

    Renderer r;

    ICS_Panel ics_Panel;

    // Transfor Stuff
    Transform t;
    Vector3 ogPosition;
    float offset = 0.05f;

    float timer = 1.0f;

    // Use this for initialization
    void Start () {
        r = this.GetComponent<Renderer>();
        t = this.GetComponent<Transform>();
        ogPosition = t.position;

        ics_Panel = GameObject.Find("!ICS Controller").GetComponent<ICS_Panel>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (MicOn == true)
        {
            r.material = textureOn;
        } else
        {
            r.material = textureOff;
        }

        if(timer < 0.1f)
        {
            t.position = new Vector3(ogPosition.x, ogPosition.y, ogPosition.z + offset);
        }
        else
        {
            t.position = new Vector3(ogPosition.x, ogPosition.y, ogPosition.z);
        }
	}

    private void OnMouseDown()
    {
        if (timer < 0.2f) return;
        ics_Panel.AllMicsOff();
        MicOn = true;
        timer = 0.0f;
    }
}
