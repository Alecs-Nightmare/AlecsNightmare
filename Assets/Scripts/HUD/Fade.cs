using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float predefinedDuration;

    Image image;
    
    // Set up references
    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start ()
    {
        image.canvasRenderer.SetAlpha(0f);
	}

    
    public void FadeToBlack(bool toBlack)
    {
        if (toBlack)
            this.GetComponent<Image>().CrossFadeAlpha(1.0f, predefinedDuration, true);
        else
            this.GetComponent<Image>().CrossFadeAlpha(0.0f, predefinedDuration, true);
    }


    public void FadeToBlack(bool toBlack, float duration)
    {
        if (toBlack)    // Going to black
        {
            image.CrossFadeAlpha(1.0f, duration, true);
        }
        else            // Going to transparent
        {
            image.canvasRenderer.SetAlpha(0.01f);
            image.CrossFadeAlpha(0.0f, duration, false);
            Debug.Log("transparent");
        }
    }
}
