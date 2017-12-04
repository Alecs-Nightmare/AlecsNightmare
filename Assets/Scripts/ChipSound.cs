using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSound : MonoBehaviour {

    public AudioClip chipPickClip;
    public AudioSource audioSource;
    public GameEvent OnCollectablePickUpEvent;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = chipPickClip;
    }

    public void PlaySound()
    {
        audioSource.Play(0);
    }
}
