using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to set TIED indicator lights
/// </summary>
public class Pwr_Tied_Light : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] mat = new Material[2];

    public void SetMaterial(int DesiredMat)
    {
        this.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(mat[DesiredMat]);
    }
}
