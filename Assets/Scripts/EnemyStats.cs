using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    [SerializeField]
    private int attackPower = 10;
    [SerializeField]
    private int hitPoints = 100;
    [SerializeField]
    private bool isLethal;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "PlayerMovement")
        {
            //print("Hit!");
            // opposite force pull?
            // -apply repulsion force on the _playerMovement-
        }
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public bool AskForLethal()
    {
        return isLethal;
    }
}
