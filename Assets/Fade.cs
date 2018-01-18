using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float duration = 0.3f;

	void Start ()
    {
        this.GetComponent<Image>().canvasRenderer.SetAlpha(0.0f);
	}
	
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
}
