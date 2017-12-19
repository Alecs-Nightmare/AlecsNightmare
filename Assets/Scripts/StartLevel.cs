using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    public void ButtonClicked()
    {
        //GameManager.instance.LoadSpecificScene("Scene1");
        SceneManager.LoadScene("Loader");
    }
    
}
