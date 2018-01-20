using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControls : MonoBehaviour {

    public float fadeDuration;
    public Image image;

    public void ShowControlsPanel(bool state)
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
