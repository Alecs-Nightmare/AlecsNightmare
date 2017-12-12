using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventCheckpoint : MonoBehaviour {

    private Animator anim;
    private string activado = "activado"; 

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            anim.SetTrigger(activado);
        }
    }
}
