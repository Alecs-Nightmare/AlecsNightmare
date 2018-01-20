using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void FadeToBlack(bool toBlack)
    {
        if (toBlack)
            this.GetComponent<Image>().CrossFadeAlpha(1.0f, duration, true);
        else
            this.GetComponent<Image>().CrossFadeAlpha(0.0f, duration, true);
    }

    public void FadeToBlack(bool toBlack, float duration)
    {
        if (toBlack)
            this.GetComponent<Image>().CrossFadeAlpha(1.0f, duration, true);
        else
            this.GetComponent<Image>().CrossFadeAlpha(0.0f, duration, true);
    }
    */

    public void FadeToBlack(bool toBlack, float duration)
    {
        if (toBlack)    // Going to black
        {
            image.canvasRenderer.SetAlpha(0f);
            image.CrossFadeAlpha(1f, duration, true);
        }
        else            // Going to transparent
        {
            image.canvasRenderer.SetAlpha(1f);
            image.CrossFadeAlpha(0f, duration, true);
        }
    }
}
