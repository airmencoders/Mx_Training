using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for attaching multiple colliders to a switch. Maybe there is a better way.
/// </summary>
public class SwCollider : MonoBehaviour {

    public bool clicked = false;

    private void OnMouseDown()
    {
        clicked = true;   
    }

}
