using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockUmbrella : MonoBehaviour {

    //public GameObject player;

    // Set up references
    /*
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    */

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            
            col.GetComponent<Player>().UmbrellaUnlocked = true;

        }
    }
}
