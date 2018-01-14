using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public bool followPlayer;
    public float enemySpeed;
    public Vector3 enemyVelocity;
    public Transform target;
    public Vector3[] localWaypoints;

    private Vector3 spawnPosition;
    private string whereIsThePlayer;
    private float localScaleX;
    private float invLocalScaleX;
    private GameObject[] bones;
    private GlassweatherAnimationController glAnimContr;
    private HorizontalEnemyController movementController;

    //private Vector3 scaleRight;

    //public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;
    
    public float waitTime;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;
    [Range(0, 2)]
    public float easeAmount;

    // Use this for initialization
    private void Awake()
    {
        glAnimContr = GetComponent<GlassweatherAnimationController>();
        movementController = GetComponent<HorizontalEnemyController>();
        spawnPosition = this.transform.position;
    }

    void Start ()
    {
        localScaleX = this.transform.localScale.x;
        invLocalScaleX = localScaleX * -1;
        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target==null)
            ManageEnemyPatrolMovement();

        if (target != null)
        {
            whereIsThePlayer = target.position.x >= transform.position.x ? "right" : "left";
            ManageEnemyMovement();
        }
	}
    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
    Vector3 CalculateEnemyPatrolMovement()
    {
        /*
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;

        }
        */
        fromWaypointIndex %= globalWaypoints.Length;

        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * enemySpeed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easePercentBetweenWaypoints = Ease(percentBetweenWaypoints);
        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easePercentBetweenWaypoints);
        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {

                this.transform.localScale = new Vector3(this.transform.localScale.x * -1f, this.transform.localScale.y, this.transform.localScale.z);
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
            }

            nextMoveTime = Time.time + waitTime;

        }
        return newPos - transform.position;


    }
    public void ManageEnemyPatrolMovement()
    {
        
        transform.Translate(CalculateEnemyPatrolMovement());

    }

    public void ManageEnemyMovement()
    {
        if (followPlayer)
        {
            //animation
            glAnimContr.alertingPlayer = true;
            glAnimContr.ignoringPlayer = false;

            //movement vector
            Vector3 velocity = (target.transform.position - transform.position).normalized;
            velocity.y = 0;
            velocity.x *= enemySpeed * Time.unscaledDeltaTime;
            enemyVelocity = velocity;
            movementController.Move(velocity);

            //flip
            
        }
        else
        {
            //animation
            glAnimContr.ignoringPlayer = true;
            glAnimContr.followingPlayer = false;
            glAnimContr.alertingPlayer = false;

            this.transform.position = Vector3.MoveTowards(this.transform.position, spawnPosition, (enemySpeed - 1) * Time.deltaTime);
            if (whereIsThePlayer == "right")
                this.transform.localScale = new Vector3(invLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);
            else
                this.transform.localScale = new Vector3(localScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }
        
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(target.position, this.transform.position);
    }
    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.green;
            float size = .3f;
            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
