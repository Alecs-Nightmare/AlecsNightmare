using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOL : MonoBehaviour {

    private void Awake()
    {
        
        DontDestroyOnLoad(this.gameObject);

        if (FindObjectsOfType(GetType()).Length > 2)
        {
            Destroy(gameObject);
        }
    }
}
