using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public void ButtonClicked()
    {
        GameManager.instance.LoadSpecificScene("Scene1");
    }
    
}
