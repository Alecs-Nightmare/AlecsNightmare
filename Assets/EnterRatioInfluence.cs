using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRatioInfluence : MonoBehaviour {
    private EnemyMovement enemyMovement;
	// Use this for initialization
	void Start () {

        enemyMovement = GetComponentInParent<EnemyMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyMovement.followPlayer = true;
            enemyMovement.target = collision.gameObject.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("lo hace");
            enemyMovement.followPlayer = false;
            //enemyMovement.target = null;
        }
    }
}
