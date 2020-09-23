using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the foreground fade object automatically based on menu selection
/// </summary>
public class Fader : MonoBehaviour {

    /*
     * float fadeAmount = 1.0f;
    const float desiredFade = 0.5f;

    
	void Update ()
    {
        // Polling local UI to see what menus is loaded
        GameObject localUI = GameObject.Find("!Local_UI");
        MainMenu_UI localUI_Access = localUI.GetComponent<MainMenu_UI>();

        // Check if we are on the main menus, all other menus are faded
        if (localUI_Access.currentMenu == MainMenu_UI.Menu.main) RemoveFade();
        else FadeToBlack();
    }

    
    private void RemoveFade()
    {
        if (fadeAmount <= desiredFade) return;          // Checking if we are done fading in to halt counter, if this goes negative it will perform additive alpha blending

        Renderer fadeObject = this.GetComponent<Renderer>();
        fadeAmount -= Time.deltaTime * 0.5f;

        if (fadeAmount < desiredFade)
        {
            fadeAmount = desiredFade;
        }

        fadeObject.material.color = new Color(1.0f, 1.0f, 1.0f, fadeAmount);
    }


    private void FadeToBlack()
    {
        if (fadeAmount >= 1.0f) return;                 

        Renderer fadeObject = this.GetComponent<Renderer>();
        fadeAmount += Time.deltaTime * 0.5f;

        if (fadeAmount > 1.0f)
        {
            fadeAmount = 1.0f;
        }

        fadeObject.material.color = new Color(1.0f, 1.0f, 1.0f, fadeAmount);
    }

    */
}
