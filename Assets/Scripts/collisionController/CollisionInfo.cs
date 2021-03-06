﻿using UnityEngine;

public class CollisionInfo
{
    public string TouchAWall;

    public bool isAttacking;
    public bool isProtecting;
    public bool above, below;
    public bool left, right;
    public float slopeAngle, slopeAngleOld;
    public bool climbingSlope;
    public bool descendingSlope;
    public bool ascending;//added
    public bool descending;//added
    public bool isDashing;
    public bool isSoaring;
    public Vector3 velocityOld;
    public int faceDirection;
    public bool fallingThroughPlatform;
    public bool almostJumping;

    public void Reset()
    {
        above = below = false;
        left = right = false;
        climbingSlope = false;
        descendingSlope = false;
        ascending = false;
        descending = false;
        //almostJumping = false;
        slopeAngleOld = slopeAngle;
        slopeAngle = 0;
        TouchAWall = "";
        //WallSliding = false;
    }
}