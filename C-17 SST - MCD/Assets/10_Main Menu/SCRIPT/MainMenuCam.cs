using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCam : MonoBehaviour {

    
    public Transform target;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    Vector3 startPos = new Vector3(0.0f, 1.0f, -10.0f);
    Vector3 targetPos = new Vector3(5.1f, -9.5f, 13.3f);

    bool StartGame = false;

    
	void Update () {
        // Depending on the assigned target, will glide the camera towards an object
        if (StartGame == true)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, startPos, ref velocity, smoothTime);
        }
	}


    public void StartSim()
    {
        StartGame = true;
    }


    public void ResetCam()
    {
        StartGame = false;
    }
}
