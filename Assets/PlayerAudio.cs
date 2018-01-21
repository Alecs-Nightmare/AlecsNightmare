using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : MonoBehaviour
{
    PlayerMovement controller;
    AudioSource audioSource;

    public AudioClip[] clips;

    private void Start()
    {
        controller = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

	
	// Update is called once per frame
	void Update ()
    {
        //salto
        if (controller.Velocity.y > 0)
        {
            audioSource.clip = clips[0];

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioSource.clip);
            }
        }	
	}
}
