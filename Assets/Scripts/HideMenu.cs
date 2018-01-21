using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideMenu : MonoBehaviour
{
    public Image[] childImages;

    private void Awake()
    {
        childImages = GetComponentsInChildren<Image>();
    }
    // Use this for initialization
    void Start ()
    {
        GetComponent<UnityEngine.UI.Image>().canvasRenderer.SetAlpha(0.0f);

        foreach (Image image in childImages)
        {
            image.canvasRenderer.SetAlpha(0.0f);
        }


	}
	
    public void ShowMenu(bool show)
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
