using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    public string exposedMusicParam;
    public string exposedSFXParam;

    public AudioMixer mixer;

    public void SetMusicVolume(Slider slider)
    {
        mixer.SetFloat(exposedMusicParam, slider.value);
    }

    public void SetSFXVolume(Slider slider)
    {
        mixer.SetFloat(exposedSFXParam, slider.value);
    }

}
