﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionDetection : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerMovement")
        {
            Debug.Log(("Hit PlayerMovement"));
        }
    }
}
