using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowChildren : MonoBehaviour {

    public Image[] childImages;
    public Slider slider;

    private void Awake()
    {
        childImages = GetComponentsInChildren<Image>();
    }

    public void ShowImageChildren(bool show)
    {
        if (show)
        {
            GetComponent<UnityEngine.UI.Image>().canvasRenderer.SetAlpha(0.0f);

            foreach (Image image in childImages)
            {
                image.canvasRenderer.SetAlpha(1.0f);
            }
        }
        else
        {
            GetComponent<UnityEngine.UI.Image>().canvasRenderer.SetAlpha(0.0f);

            foreach (Image image in childImages)
            {
                image.canvasRenderer.SetAlpha(0.0f);
            }
        }
    }
}
