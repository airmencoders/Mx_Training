using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All this does is scroll the terrain on the loading screen
/// </summary>
public class ScrollingTerrain : MonoBehaviour {

    public Material terrainTex;
    float texOffset = 0.0f;

    
	// Update is called once per frame
	void Update () {
        
        texOffset -= Time.deltaTime * 0.15f;
        terrainTex.SetTextureOffset("_MainTex", new Vector2(texOffset, -0.01f));
    }
}
