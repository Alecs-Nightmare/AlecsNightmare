using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
    
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float minJumpHeight = 1;
    public float maxJumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public float maxDashTime = 1f;
    public float dashForce = 50.0f;
    public float dashStoppingSpeed = 0.1f;
    public float gravityWhilePlanning = -20f;
    public float constantvelocityYFalling = -2.5f;
    public float moveSpeed;
    public Vector2 input;
    public bool wallSliding;

    float maxJumpVelocity;
    float minJumpVelocity;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    float saveGravity;
    float gravity = -20f;
    float timeToWallUnstick;
    float jumpsToPlane = 2f;
    float currentJumps;
    float currentDashTime;
    float accelRatePerSec;
    float saveVelocityY;
    bool canEnableUmbrella = true;
    bool umbrellaUnlocked = false;
    [SerializeField] bool active = true;
    Vector3 aimDirection;
    Vector3 velocity;
    Controller2D controller;
    SpriteRenderer spriteRenderer;

    private int sanityPoints;   //esto es provisional

    //esto es provisional
    public bool UmbrellaUnlocked
    {
        get
        {
            return umbrellaUnlocked;
        }

        set
        {
            umbrellaUnlocked = value;
        }
    }

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start ()
    {
        currentJumps = 0f;
        jumpsToPlane = 2f;
        aimDirection = Vector3.right;
        currentDashTime = maxDashTime;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + " Jump Velocity: " + maxJumpVelocity);
        saveGravity = gravity;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (active)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            float targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            if (aimDirection == Vector3.right)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            if (!controller.collisions.below && !controller.collisions.left && !controller.collisions.right)
            {


                if (!controller.collisions.isPlanning && velocity.y < 0) //si está en el aire (sin planear)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && canEnableUmbrella)//y se pulsa espacio y puede usar el paraguas
                    {
                        velocity.y = 0;

                        gravity = gravityWhilePlanning;

                        currentJumps = 0f;
                        //print("activado");
                        controller.collisions.isPlanning = true;
                        return;
                    }
                    //si no pulsa espacio o no se puede usar el paraguas

                }
            }

            if (controller.collisions.isPlanning)//si  está planeando
            {


                if (controller.collisions.below || controller.collisions.left || controller.collisions.right || Input.GetKeyDown(KeyCode.Space))//si colisiona en cualquier dirección o se pulsa espacio
                {

                    canEnableUmbrella = false;

                    controller.collisions.isPlanning = false;

                }
            }
            else //if no está planeando
            {

                gravity = saveGravity;

                if (controller.collisions.below || controller.collisions.left || controller.collisions.right)
                {
                    //gravity = saveGravity;
                    currentJumps = 0f;
                    canEnableUmbrella = true;
                }
            }


            if (input.x != 0)//si se mueve horizontalmente
            {
                aimDirection = Mathf.Sign(input.x) == 1 ? Vector3.right : Vector3.left;//constantly check which direction is the player looking at          

            }

            wallSliding = false;

            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
            {
                if (controller.getHitTag() != "Through")//PARA QUE NO FRENE SI ES PLATAFORMA Q SE MEUVE
                {
                    wallSliding = true;
                    if (velocity.y < -wallSlideSpeedMax)
                    {
                        velocity.y = -wallSlideSpeedMax;
                    }
                }



                if (timeToWallUnstick > 0)
                {

                    velocityXSmoothing = 0;
                    velocity.x = 0;
                    if (input.x != wallDirX && input.x != 0)
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }




                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }

            if (Input.GetKeyDown(KeyCode.C))//start dash
            {
                currentDashTime = 0f;
                controller.collisions.isDashing = true;

            }

            if (currentDashTime < maxDashTime)//dash still activated?
            {
                //velocity.x += ((direction == 1) ? dashForce : -dashForce);
                velocity.x += aimDirection.x * dashForce;
                velocity.y = 0f;
                currentDashTime += dashStoppingSpeed;



            }
            else //dash is finished
            {
                controller.collisions.isDashing = false;
                //velocity.x = 0f;
            }


            if (Input.GetKeyDown(KeyCode.Space))//si pulsa espacio
            {
                currentJumps++;

                if (currentJumps == jumpsToPlane && canEnableUmbrella) //si puede planear: planea
                {
                    controller.collisions.isPlanning = true;
                    gravity = gravityWhilePlanning;
                    currentJumps = 0f;
                }


                if (wallSliding && controller.getHitTag() != "Through")
                {

                    if (wallDirX == input.x)
                    {
                        velocity.x = -wallDirX * wallJumpClimb.x;
                        velocity.y = wallJumpClimb.y;

                    }
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else
                    {
                        velocity.x = -wallDirX * wallLeap.x;
                        velocity.y = wallLeap.y;
                    }
                }
                if (controller.collisions.below)//si estaba en tierra: salta
                {
                    velocity.y = maxJumpVelocity;
                }


            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (velocity.y > minJumpVelocity)
                {
                    velocity.y = minJumpVelocity;
                }

            }

            if (!controller.collisions.isDashing)
            {

                if (controller.collisions.isPlanning) //added
                {
                    if (controller.collisions.descending)
                    {
                        velocity.y = constantvelocityYFalling;
                    }
                    else//if ascending
                    {
                        velocity.y += gravity * Time.deltaTime;
                    }
                }

                else//added
                {
                    velocity.y += gravity * Time.deltaTime;
                }

            }

            controller.Move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            if (input.x == 0)
            {
                velocity.x = 0;
            }
            if (!UmbrellaUnlocked)
            {
                canEnableUmbrella = false;
            }
        }
    }

    public Vector3 getVelocity() {
        return velocity;
    }
    public float getCurrentJump()
    {
        return currentJumps;
    }
    public bool getCanEnableUmbrella()
    {
        return canEnableUmbrella;
    }
    public float getJumpsToPlane()
    {
        return jumpsToPlane;
    }

    public void SetJumpBetweenWalls(string type)
    {

        if (type == "Climbable")
        {

            //print("LOL2");

            wallJumpClimb.x = 5;
            wallJumpClimb.y = 17;
        }
        
        else
        {
            //print("LOL");
            //changing properties...
            wallJumpClimb.x = 4;
            wallJumpClimb.y = 10;
        }

    }

    //provisional
    public void setSanityPoints(int amount)
    {

        sanityPoints += amount;

    }
    public int getSanityPoints()
    {

        return sanityPoints;
    }
    //provisional

    public void StopPlayer(bool stop)
    {
        active = !stop;
    }
}
