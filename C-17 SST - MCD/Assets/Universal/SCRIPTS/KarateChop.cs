using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarateChop : MonoBehaviour {

    Transform Sprite_Hand;

    Vector3 startPos = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 endPos = new Vector3(0.0f, -20.0f, 0.0f);
    bool isChopping = true;
    float timer = 0.0f;

    // Use this for initialization
    void Start () {
        startPos = this.transform.localPosition;
        endPos = this.transform.localPosition;
        endPos.y -= 30.0f;
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime * 2.0f;


        if (!isChopping) return;

        this.transform.localPosition = Vector3.Lerp(startPos, endPos, timer);
        if (timer > 1.0f) GameObject.Destroy(this.gameObject);

	}

    public void Chop()
    {
        timer = 0.0f;
        isChopping = true;
    }
}
