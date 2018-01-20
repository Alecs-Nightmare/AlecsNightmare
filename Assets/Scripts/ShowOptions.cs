using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOptions : MonoBehaviour
{
    public Image controlsImage;
    public Image audioImage;
    public Image imageToHide;

    public Button controls;
    public Button audioButton;
    public float fadeDuration = 0.75f;

    private void Start()
    {
        controls.gameObject.SetActive(false);
        audioButton.gameObject.SetActive(false);
        controlsImage.canvasRenderer.SetAlpha(0.0f);
    }

    public void ShowPanel(bool state)
    {
        if (state)
        {
            this.GetComponent<Button>().interactable = false;
            audioButton.gameObject.SetActive(true);
            controls.gameObject.SetActive(true);
            imageToHide.CrossFadeAlpha(0.0f, fadeDuration, true);
            controlsImage.CrossFadeAlpha(1.0f, fadeDuration, true);
            audioImage.CrossFadeAlpha(0.0f, fadeDuration, true);
        }
        else
        {
            this.GetComponent<Button>().interactable = true;
            controls.gameObject.SetActive(false);
            audioButton.gameObject.SetActive(false);
            imageToHide.CrossFadeAlpha(1.0f, fadeDuration, true);
            controlsImage.CrossFadeAlpha(0.0f, fadeDuration, true);
            audioImage.CrossFadeAlpha(0.0f, fadeDuration, true);
        }

    }
}
