using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRatioInfluence : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private GlassweatherAnimationController glAnimContr;

	// Use this for initialization
	private void Awake ()
    {
        glAnimContr = GetComponentInParent<GlassweatherAnimationController>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            enemyMovement.InitialDistanceTraveled = transform.position;
            Debug.Log("entramos en su area de influencia");
            enemyMovement.followPlayer = true;
            enemyMovement.patrolling = false;
            enemyMovement.target = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
<<<<<<< HEAD
            enemyMovement.FinalDistanceTraveled = transform.position;
            Debug.Log(this.transform.position);
=======
            enemyMovement.currentTimeToBackPatrol = 0f;
>>>>>>> da9999affffcdbe6f25e75c48dd795e20a48c49e
            enemyMovement.followPlayer = false;


            enemyMovement.target = null;
            glAnimContr.followingPlayer = false;
            glAnimContr.alertingPlayer = false;
            glAnimContr.ignoringPlayer = true;
            
        }
    }
}
