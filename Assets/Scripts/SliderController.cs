using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    public Button[] buttons;
    private Slider slider;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void ShowButtons(bool show)
    {
        if (show)
        {
            foreach (Button button in buttons)
            {
                button.interactable = true;
            }
        }
        else
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
    }

    public void IncreaseVolume(float increaseValue)
    {
        if(slider.value + increaseValue <= 0)
            slider.value += increaseValue;
        else
            slider.value = 0;

    }

    public void DecreaseValue(float decreaseValue)
    {
        slider.value -= decreaseValue;
        if (slider.value <= -30)
        {
            slider.value = -30;
        }
    }
}
