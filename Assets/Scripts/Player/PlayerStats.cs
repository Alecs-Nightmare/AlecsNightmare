using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    private bool dead;


    // Set up references
    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (dead)
        {
            //this.gameObject.SetActive(false);
            print("You die!");
        }

        // Respawn the player
        if (dead && Input.anyKeyDown)
        {
            this.transform.position = GameManager.instance.Respawn().position;
            //this.gameObject.SetActive(true);
            print("Respawned at " + GameManager.instance.Respawn().position);
            dead = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            // Do we have any HP ??
            // -HP operations-
            // if players dies...
            dead = true;
        }
    }
}
