using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : RaycastController 
{
	#region member variables
	[Header("Controller Options")]
    public float maxClimbAngle = 80f;
    public float maxDescendAngle = 75f;
    public int sanityToAdd = 3;

    private PlayerStats playerStats;

    [HideInInspector]
    public bool _standingOnPlatform;

	[HideInInspector]public PlayerMovement RefPlayerMovement;
	[HideInInspector]public CollisionInfo collisions;
	#endregion

	#region Public Properties
	public Vector2 PlayerInput { get; private set; }
    public string HitTag { get; private set; }

    #endregion

    #region MonoBehaviours Messages
	public override void Awake ()
	{
		base.Awake ();
		RefPlayerMovement = GetComponent<PlayerMovement>();
	    playerStats = GetComponent<PlayerStats>();
        collisions = new CollisionInfo();
	}

    public override void Start()
    {
        base.Start();
        collisions.faceDirection = 1;
    }
    #endregion

    #region Controller Methods

    
    public void Move(Vector3 velocity, bool standingOnPlatform)
    {
        _standingOnPlatform = standingOnPlatform;
        //Debug.Log(_standingOnPlatform);
        Move(velocity, Vector2.zero, standingOnPlatform);
    }
            
    public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

		UpdateCollisionsInfo (ref velocity, standingOnPlatform,input);
        HorizontalCollisions(ref velocity);
        
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
            
        transform.Translate(velocity);               
    }

	void UpdateCollisionsInfo (ref Vector3 velocity, bool standingOnPlatform, Vector2 input)
	{
		PlayerInput = input;
		collisions.velocityOld = velocity;

		if (collisions.below) {
			collisions.descending = false;
			collisions.ascending = false;
		}
		if (collisions.below || (!collisions.left && !collisions.right)) {
			HitTag = "";
		}
		if (velocity.x != 0) {
			collisions.faceDirection = (int)Mathf.Sign (velocity.x);
		}
		if (velocity.y < 0 && !standingOnPlatform)//added
		 {
 
			collisions.descending = true;
			CheckIfDescendingSlope (ref velocity);
		}
		else
			if (velocity.y > 0 && !standingOnPlatform)//added
			 {
				collisions.ascending = true;
			}
		if (standingOnPlatform) {
			collisions.below = true;
		}
	}

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = collisions.faceDirection;
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Mathf.Abs(velocity.x) < skinWidth)
        {

            rayLength = 2 * skinWidth;
        }
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                HitTag = hit.collider.tag;
                
               if (hit.collider.CompareTag("collectable"))
               {
                    //playerStats.CurrentSanity += sanityToAdd;
                    Destroy(hit.collider.gameObject);
                    continue;
                }
               
                if (hit.distance == 0)
                    continue; 

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                    
                }
                     
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) //wall
                {

                    //change walljumping properties depending which wall we are colliding with
                    collisions.TouchAWall = hit.collider.tag;
                    RefPlayerMovement.SetJumpBetweenWalls(collisions.TouchAWall);

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance; //we gotta change our ray length due to if there is a two different height blocks our object has to be able to collide with bpth

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {

                if (hit.collider.CompareTag("Through"))
                {
                    if (directionY == 1 || hit.distance ==0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (PlayerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.1f);
                        continue;
                    }


                }
                else if (hit.collider.CompareTag("collectable"))
                {
                    //playerStats.CurrentSanity += sanityToAdd;
                    Destroy(hit.collider.gameObject);
                    continue;
                }
               
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; //we gotta change our ray length due to if there is a two different height blocks our object has to be able to collide with bpth
                
                

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
        if (collisions.climbingSlope){
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit){

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }
            
   void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)   
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

   void CheckIfDescendingSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        DescendSlope (velocity, directionX, hit);
    }

	void DescendSlope (Vector3 velocity, float directionX, RaycastHit2D hit)
	{
		if (hit) 
		{
			float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign (hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) {
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }
    #endregion
   

}
