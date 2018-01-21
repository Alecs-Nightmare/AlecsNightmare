using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    public Button SubmitButton;
    public Button CancelButton;
    public float fadeDuration = 1.0f;

    private void Start()
    {
        SubmitButton.image.canvasRenderer.SetAlpha(0.0f);
        CancelButton.image.canvasRenderer.SetAlpha(0.0f);
    }

    public void ActivateLevelSelection(bool state)
    {
        if (state)
        {
            SubmitButton.gameObject.SetActive(true);
            CancelButton.gameObject.SetActive(true);
            SubmitButton.image.CrossFadeAlpha(1, fadeDuration, true);
            CancelButton.image.CrossFadeAlpha(1, fadeDuration, true);
        }
        else
        {
            SubmitButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
            SubmitButton.image.CrossFadeAlpha(0, fadeDuration, true);
            CancelButton.image.CrossFadeAlpha(0, fadeDuration, true);
        }

    }

    public void BackToStartScreen()
    {
        SubmitButton.gameObject.SetActive(false);
        CancelButton.gameObject.SetActive(false);
        SubmitButton.image.CrossFadeAlpha(0, fadeDuration, true);
        CancelButton.image.CrossFadeAlpha(0, fadeDuration, true);
    }
}
