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
            enemyMovement.followPlayer = true;
            enemyMovement.target = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyMovement.followPlayer = false;
            enemyMovement.target = null;
            glAnimContr.followingPlayer = false;
            glAnimContr.alertingPlayer = false;
            glAnimContr.ignoringPlayer = true;
            
        }
    }
}
