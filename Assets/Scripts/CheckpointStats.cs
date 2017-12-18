using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointStats : MonoBehaviour {

    [SerializeField]
    private int number;


    // Set up references
    void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<PlayerStats>().GetState() >= 0)
            {
                print("Check: " + number + "!");
                GameManager.instance.UpdateCurrentCheckNum(number);
            }
        }
    }

    public int GetNumber()
    {
        return number;
    }
}
