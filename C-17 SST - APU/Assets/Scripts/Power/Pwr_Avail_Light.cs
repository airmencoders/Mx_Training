using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to set lighting for GPU & APU availability
/// </summary>
public class Pwr_Avail_Light : MonoBehaviour
{
    // Start is called before the first frame update

    public Material[] mat = new Material[3];

    public void SetMaterial(int DesiredMat)
    {
        this.GetComponent<Renderer>().material.CopyPropertiesFromMaterial(mat[DesiredMat]);
    }
}
