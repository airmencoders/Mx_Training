using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manages the parachute object's prefab
/// </summary>
public class Choot : MonoBehaviour {

    public Transform sprite;
    const float speedScale = 0.1f;

    Vector3 StartVel = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 CurrentVel = new Vector3(0.0f, -0.01f, 0.0f);
    float xAccel = 1.0f * speedScale;
    float yAccel = -0.1f;

    Vector3 StartScale = new Vector3(0.1f, 0.25f, 1.0f);
    Vector3 Scale = new Vector3(0.1f, 0.25f, 1.0f);
    Vector3 ScaleTgt = new Vector3(0.25f, 0.25f, 1.0f);

    float timer = 0.0f;                                     // Track chute time alive used using a 1 second delay currently via Lerp


    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);                 // This prevents the chute from being killed during an ASync load on slow computers.
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;                            // Increment timer

        Scale = Vector3.Lerp(StartScale, ScaleTgt, timer);  // Make it look like the chute is deploying

        // Handle chute motion
        CurrentVel.x = Mathf.Lerp(StartVel.x, xAccel, timer);   
        CurrentVel.y *= 1.0f - (yAccel * Time.deltaTime);

        // Rotate sprite as it slows down
        sprite.localEulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(-90.0f, -0.0f, timer));

        // Update position
        sprite.position += CurrentVel;
        sprite.localScale = Scale;

        // Kill this once 5 seconds have elapsed
        if (timer > 5.0f) GameObject.Destroy(this.gameObject);
    }

}
