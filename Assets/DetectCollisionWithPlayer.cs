﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionWithPlayer : MonoBehaviour {


    public InstantiateBackgrounds refInstantiateBackgroundsClass;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            refInstantiateBackgroundsClass.changeFrontBackgrounds();


        }
    }



}
