using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSimpleController : MonoBehaviour {

    PlayerStats playerStats;
    private GameObject player;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player != null)
        {
            if (playerStats.GetState() >= 0)
            {
                this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -2f);
            }
            else
            {
                this.transform.position = new Vector3(player.transform.position.x, this.transform.position.y, -2f);
            }
        }
        else
        {
            ReferencePlayer();
        }
	}

    private void ReferencePlayer()
    {
        print(this+": Locating player...");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.gameObject.GetComponent<PlayerStats>();
        }
    }
}
