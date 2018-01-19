using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemyController : RaycastController
{


    public LayerMask playerMask;

    [HideInInspector] public CollisionInfo collisions;
    

    public override void Awake()
    {
        base.Awake();
        collisions = new CollisionInfo();
    }

    public override void Start()
    {
        base.Start();
        collisions.faceDirection = 1;
    }
    public void FixedUpdate()
    {
        
    }
    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        UpdateCollisionsInfo(ref velocity);
        HorizontalCollisions(ref velocity);
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, playerMask | collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                if (hit.distance == 0)
                    continue;

                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance; //we gotta change our ray length due to if there is a two different height blocks our object has to be able to collide with bpth

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
              
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength,playerMask | collisionMask );
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; //we gotta change our ray length due to if there is a two different height blocks our object has to be able to collide with bpth
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateCollisionsInfo(ref Vector3 velocity)
    {
        collisions.velocityOld = velocity;


        if (velocity.x != 0)
        {
            collisions.faceDirection = (int)Mathf.Sign(velocity.x);
        }
    }



    // Update is called once per frame
    void Update () {
		
	}
}
