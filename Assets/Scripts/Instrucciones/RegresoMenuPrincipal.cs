using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegresoMenuPrincipal : MonoBehaviour {

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene("Menu con instrucciones");
        }
    }
}
