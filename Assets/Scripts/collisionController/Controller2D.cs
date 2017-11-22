using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(BoxCollider2D))]
public class Controller2D : RaycastController {

    public float maxClimbAngle = 80f;
    public float maxDescendAngle = 75f;
    public CollisionInfo collisions;

    private Vector2 playerInput;
    private string hitTag;
    public Player refPlayer;
    public Vector2 PlayerInput { get { return playerInput; } }


    // Use this for initialization
    public override void Start()
    {
        refPlayer = GetComponent<Player>();
        
        base.Start();
        collisions.faceDirection = 1;
    }

    public void Move(Vector3 velocity, bool standingOnPlatform)
    {
        Move(velocity, Vector2.zero, standingOnPlatform);
    }
            
    public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false)
    {
        if (collisions.below)//added
        {
            collisions.descending = false;
            collisions.ascending = false;
        }

        if (collisions.descending)//added                
        {
            //print("descending!!");
        }
        if (collisions.ascending)//added
        {
            //print("ascending!!");
        }

        UpdateRaycastOrigins();
        collisions.Reset();

        collisions.velocityOld = velocity;
        playerInput = input;

        if (collisions.below || (!collisions.left && !collisions.right))
        {
            hitTag = "";
            
        }

        if (velocity.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }

        if (velocity.y < 0 && !standingOnPlatform)//added
        {
            collisions.descending = true;
            DescendSlope(ref velocity);
        }
        else if (velocity.y > 0 && !standingOnPlatform)//added
        {
            collisions.ascending = true;
        }
        
        HorizontalCollisions(ref velocity);
        
            
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }
            
        transform.Translate(velocity);
        if (standingOnPlatform)
        {
            collisions.below = true;
        }

        //check if we are grounded

        /*
        if (collisions.below)
        {
            if (collisions.isPlanning)
            {
                collisions.isPlanning = false;
            }
        }
        */
                         
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


                hitTag = hit.collider.tag;
                
               if (hit.collider.CompareTag("collectable"))
                {
                    refPlayer.setSanityPoints(+3);
                    print(refPlayer.getSanityPoints());
                    Destroy(hit.collider.gameObject);
                    continue;
                }

                //print(hitTag);
                if (hit.distance == 0)
                {
                   
                    continue;
                    
                    
                }

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
                    refPlayer.SetJumpBetweenWalls(collisions.TouchAWall);
                    //print(collisions.TouchAWall);                      
                    //change... end

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
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", 0.1f);
                        continue;
                    }


                }
                else if (hit.collider.CompareTag("collectable"))
                {
                    refPlayer.setSanityPoints(+3);
                    print(refPlayer.getSanityPoints());
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

   void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            


            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle!=0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;

                    }
                }
                 
            }
        }



    }


 


    public string getHitTag()
    {
        return hitTag;
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }
   
    public struct CollisionInfo
    {
        public string TouchAWall;

        public bool above, below;
        public bool left, right;

        public float slopeAngle, slopeAngleOld;
        public bool climbingSlope;
        public bool descendingSlope;

        public bool ascending;//added
        public bool descending;//added


        public bool isDashing;

        public bool isPlanning;
        //public bool grounded;            

        public Vector3 velocityOld;
        public int faceDirection;
        public bool fallingThroughPlatform;
               
        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            ascending = false;//added
            descending = false;//added
            //grounded = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
            TouchAWall = "";
        }
    }
}
