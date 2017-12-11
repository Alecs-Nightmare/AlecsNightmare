using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {
    
    #region Member Variables
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float minJumpHeight = 1;
    public float maxJumpHeight = 4;
    public float timeToJumpApex = 0.4f;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public float gravityWhilePlanning = -20f;
    public float constantvelocityYFalling = -2.5f;
    public float moveSpeed;

    float maxJumpVelocity;
    float minJumpVelocity;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    float saveGravity;
    float gravity = -20f;
    float timeToWallUnstick;
    float accelRatePerSec;
    bool canEnableUmbrella = true;
    bool active = true;
    Vector2 input;
    Vector3 aimDirection;
    Vector3 velocity;
    Controller2D controller;
    SpriteRenderer spriteRenderer;
    PlayerInput playerInput;
    int sanityPoints;   //esto es provisional

    public PlayerMovement()
    {
        UmbrellaUnlocked = false;
    }

    #endregion

    #region Public Properties
    public bool UmbrellaUnlocked { get; set; }

    public Vector3 Velocity { get {return velocity; } }
    public bool CanEnableUmbrella { get {return canEnableUmbrella;}}
    #endregion

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Use this for initialization
    void Start ()
    {
        InitializeMembers();
    }

    void Update () 
    {
        if (active)
        {
            input = playerInput.DirectionalInput;
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            ResetGravity();

            CheckIfUnlockedUmbrella();

            HorizontalMovement();

            FlipSprite();

            CheckIfCanSoar();

            CheckIfCanEnableUmbrellaAgain();

            HandleWallSliding(wallDirX);

            HandleJumping(wallDirX);

            controller.Move(velocity * Time.deltaTime, input);
        }
    }

    private void CheckIfUnlockedUmbrella()
    {
        if (!UmbrellaUnlocked)
        {
            canEnableUmbrella = false;
        }
    }

    private void ResetGravity()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    private void HandleJumping(int wallDirX)
    {
        if (playerInput.CaptureJumpInputDown())
        {
            if (controller.collisions.WallSliding && controller.HitTag != "Through")
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
            if (controller.collisions.below)
            {
                print("jump");
                velocity.y = maxJumpVelocity;
            }
        }

        if (playerInput.CaptureJumpInputUp())
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }

        }
    }

    private void HandleWallSliding(int wallDirX)
    {
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            if (controller.HitTag != "Through") //PARA QUE NO FRENE SI ES PLATAFORMA Q SE MEUVE
            {
                controller.collisions.WallSliding = true;
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
    }

    private void HorizontalMovement()
    {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (input.x != 0)//si se mueve horizontalmente
        {
            aimDirection = Mathf.Sign(input.x) == 1 ? Vector3.right : Vector3.left;//constantly check which direction is the _playerMovement looking at          
        }

    }

    private void CheckIfCanEnableUmbrellaAgain()
    {
        if (controller.collisions.isSoaring) //si  está planeando
        {
            if (controller.collisions.below || controller.collisions.left || controller.collisions.right ||
                playerInput.CaptureSoarInput()) //si colisiona en cualquier dirección o se pulsa espacio
            {
                canEnableUmbrella = false;

                controller.collisions.isSoaring = false;
            }
        }
        else //if no está planeando
        {
            gravity = saveGravity;

            if (controller.collisions.below || controller.collisions.left || controller.collisions.right)
            {
                canEnableUmbrella = true;
            }
        }
    }

    private void CheckIfCanSoar()
    {
        if (!controller.collisions.below && !controller.collisions.left && !controller.collisions.right)
        {
            if (!controller.collisions.isSoaring && velocity.y < 0) //si está en el aire (sin planear)
            {
                if (playerInput.CaptureSoarInput() && canEnableUmbrella) //y se pulsa espacio y puede usar el paraguas
                {
                    velocity.y = 0;
                    gravity = gravityWhilePlanning;
                    controller.collisions.isSoaring = true;
                    return;
                }
            }
        }
    }

    private void FlipSprite()
    {
        if (aimDirection == Vector3.right)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void SetJumpBetweenWalls(string type)
    {

        if (type == "Climbable")
        {
            wallJumpClimb.x = 5;
            wallJumpClimb.y = 17;
        }
        
        else
        {
            wallJumpClimb.x = 4;
            wallJumpClimb.y = 10;
        }

    }

    private void InitializeMembers()
    {
        aimDirection = Vector3.right;
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        saveGravity = gravity;
    }

    //TODO revisar propiedades publicas de los sanity points

    public void StopPlayer(bool stop)
    {
        active = !stop;
    }
}
