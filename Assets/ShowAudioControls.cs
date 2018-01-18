using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAudioControls : MonoBehaviour {

    public Image image;
    public float fadeDuration;

    void Start ()
    {
        image.canvasRenderer.SetAlpha(0.0f);	
	}

    public void ShowControls(bool state)
    {
        if (state)
        {
            image.CrossFadeAlpha(1.0f, fadeDuration, true);
        }
        else
        {
            image.CrossFadeAlpha(0.0f, fadeDuration, true);
        }
    }

}
