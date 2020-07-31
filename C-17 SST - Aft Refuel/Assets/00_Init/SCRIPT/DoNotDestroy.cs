using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour {

	/// <summary>
    /// Script that only sets object to do not destroy
    /// </summary>
	void Start () {
        DontDestroyOnLoad(this);                    // Keep this forever!	
    }
}
