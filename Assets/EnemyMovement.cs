using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public bool patrolling;
    public bool backToPatrol;
    public bool followPlayer;

    public bool test;

    public Vector3 enemyVelocity;
    public Transform target;
    public Vector3[] localWaypoints;

    public float enemySpeed = 3f;
    public Vector3 aimDirection;
    public Vector3 lastPatrolMovement;

    public float timeToBackPatrol = 3f;
    public float currentTimeToBackPatrol = -1f;

    public float distanceToWaypointOne;
    public float distanceToWaypointTwo;
    public Vector3 closerWaypoint;

    public bool isOnAWaypoint;

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

    public Vector3 velocity;

    //following player
    Vector3 currentPos;

    // Use this for initialization
    private void Awake()
    {
        glAnimContr = GetComponent<GlassweatherAnimationController>();
        movementController = GetComponent<HorizontalEnemyController>();
        spawnPosition = this.transform.position;
    }

    void Start ()
    {
        patrolling = true;
        aimDirection = Vector3.zero;
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
        //Debug.Log(CheckIfOnAWaypoint());
        Debug.Log(closerWaypoint);
        //if (CheckStartCountingToBackPatrol())
            //CountToBackPatrol();
        UpdateDistanceToWaypoints();
        CheckAimDirection();
        ManageEnemyPatrolMovement();

	}
    public bool CheckStartCountingToBackPatrol()
    {
        return currentTimeToBackPatrol > -1f;
    }
    public void CountToBackPatrol()
    {
        currentTimeToBackPatrol += Time.deltaTime;
        if (currentTimeToBackPatrol >= timeToBackPatrol)
        {
            print("GO!");
            patrolling = true;

            followPlayer = false;
        }
            

    }
    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }
    public void UpdateDistanceToWaypoints()
    {
        distanceToWaypointOne = Vector3.Distance(transform.position, globalWaypoints[0]);
        distanceToWaypointTwo = Vector3.Distance(transform.position, globalWaypoints[1]);
        if (distanceToWaypointOne > distanceToWaypointTwo)
            closerWaypoint = globalWaypoints[1];

        else
            closerWaypoint = globalWaypoints[0];

    }

    public bool CheckIfOnAWaypoint()
    {
        return (transform.position == globalWaypoints[0] || transform.position == globalWaypoints[1]);
    
    }

    public void CheckAimDirection()
    {
        if (velocity.x > 0) // moving right
        {
            //print("right");
            this.transform.localScale = new Vector3(localScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (velocity.x < 0) // moving left
        {
            //print("left");
            this.transform.localScale = new Vector3(invLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }


    }

    Vector3 CalculateEnemyMovement()
    {
        /*
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;

        }
        
        */
        if (patrolling)//lerp entre checkpoint actual y checkpoint siguiente
        {
            Vector3 newPos = Vector3.zero;
            fromWaypointIndex %= globalWaypoints.Length;

            int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
            float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
            percentBetweenWaypoints += Time.deltaTime * enemySpeed / distanceBetweenWaypoints;
            percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
            float easePercentBetweenWaypoints = Ease(percentBetweenWaypoints);

            newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easePercentBetweenWaypoints);
            lastPatrolMovement = globalWaypoints[fromWaypointIndex];

            if (percentBetweenWaypoints >= 1)
            {
                percentBetweenWaypoints = 0;
                fromWaypointIndex++;

                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {


                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }

                nextMoveTime = Time.time + waitTime;

            }
            velocity = newPos - transform.position;
            return velocity;
        }
        
        
        else if (followPlayer)//following
        {

            //animation
            glAnimContr.alertingPlayer = true;
            glAnimContr.ignoringPlayer = false;
            velocity = (target.transform.position - transform.position).normalized;
            velocity.y = 0;
            velocity.x *= enemySpeed * Time.unscaledDeltaTime;
            return velocity;
            
      


        }
        return velocity;




        


    }
    public void ManageEnemyPatrolMovement()
    {
        
        movementController.Move(CalculateEnemyMovement());

    }

    public void ManageEnemyMovement()
    {
        if (followPlayer)
        {
            //animation
            glAnimContr.alertingPlayer = true;
            glAnimContr.ignoringPlayer = false;

            //movement vector
            velocity = (target.transform.position - transform.position).normalized;
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
