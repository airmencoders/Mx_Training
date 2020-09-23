using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Just animates the loading text
/// </summary>
public class LoadingAnimation : MonoBehaviour
{
    private float timer = 0.0f;
    Text LoadingText;

    private void Start()
    {
        LoadingText = this.GetComponent<Text>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            LoadingText.text = "LOADING...";
            timer = 0.0f;
        }
        else if (timer > 0.75f)
        {
            LoadingText.text = "LOADING..";
        }
        else if (timer > 0.5f) 
        { 
            LoadingText.text = "LOADING.";
        }
        else if (timer > 0.25f)
        {
            LoadingText.text = "LOADING";
        }

    }
}
