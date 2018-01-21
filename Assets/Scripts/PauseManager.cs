using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public Button[] buttons;
    public Image[] childImages;
    public List<string> scenesWithoutPause;

    bool isPauseActive = false;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        childImages = GetComponentsInChildren<Image>();
    }

    // Use this for initialization
    void Start ()
    {
        ShowPausePanel(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (!scenesWithoutPause.Contains(SceneManager.GetActiveScene().name))
            {
                if (isPauseActive)
                {
                    isPauseActive = false;
                    ShowPausePanel(isPauseActive);
                }
                else
                {
                    isPauseActive = true;
                    ShowPausePanel(isPauseActive);
                }
            }
        }
	}

    public void ShowPausePanel(bool show)
    {
        if(show)
        {
            foreach (Button b in buttons)
            {
                b.gameObject.SetActive(true);
            }

            foreach (Image image in childImages)
            {
                image.canvasRenderer.SetAlpha(1.0f);
            }

        }
        else
        {
            foreach (Button b in buttons)
            {
                b.gameObject.SetActive(false);
            }

            foreach (Image image in childImages)
            {
                image.canvasRenderer.SetAlpha(0.0f);
            }

        }
    }

}

