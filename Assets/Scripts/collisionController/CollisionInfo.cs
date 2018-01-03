using UnityEngine;

public class CollisionInfo
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
    public bool isSoaring;
    public Vector3 velocityOld;
    public int faceDirection;
    public bool fallingThroughPlatform;

    public void Reset()
    {
        above = below = false;
        left = right = false;
        climbingSlope = false;
        descendingSlope = false;
        ascending = false;
        descending = false;
        slopeAngleOld = slopeAngle;
        slopeAngle = 0;
        TouchAWall = "";
        //WallSliding = false;
    }
}