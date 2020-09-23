using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCD_Help : MonoBehaviour {

    GameObject gHintSpikey;
    GameObject gHintText;
    Transform tHintSpikey;
    Transform tHintText;
    
    float timeOff = 0.0f;
    public bool wasOn = false;
    bool showHint = false;
    public bool hideHint = false;

    float t = 0.0f;

    Vector3 spikeyPos;
    Vector3 spikeyDest;
    Vector3 spikeyHide = new Vector3(12.6f, 10.0f, 0.0f);
    Vector3 spikeyShow = new Vector3(6.6f, 4.65f, 0.0f);
    float spikeyTravel = 0.0f;

    Vector3 hintPos;
    Vector3 hintDest;
    Vector3 hintHide = new Vector3(12.6f, 5.5f, 0.0f);
    Vector3 hintShow = new Vector3(6.6f, 5.0f, 0.0f);
    float hintTravel = 0.0f;


    // Use this for initialization
    void Start () {
        gHintSpikey = GameObject.Find("Hint_Background");
        gHintText = GameObject.Find("Hint_Text");

        tHintSpikey = gHintSpikey.GetComponent<Transform>();
        tHintText = gHintText.GetComponent<Transform>();

        spikeyPos = spikeyHide;
        hintPos = hintHide;
    }

    // Update is called once per frame
    void Update() {
        if(hideHint == true)
        {
            tHintSpikey.localPosition = spikeyHide;
            tHintText.localPosition = hintHide;
            return;
        }
        tHintSpikey.localPosition = Vector3.Lerp(spikeyHide, spikeyShow, t*2);
        tHintText.localPosition = Vector3.Lerp(hintHide, hintShow, t);



        if (showHint == true)
        {
            t += Time.deltaTime;
            
        }

        if (timeOff > 30.0f) {
            showHint = true;
            wasOn = true;
        }
        else if (wasOn == true) return;

        timeOff += Time.deltaTime;

	}


}
