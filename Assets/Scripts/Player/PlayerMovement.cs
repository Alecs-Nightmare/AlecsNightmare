using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class PlayerMovement : MonoBehaviour {

    #region Member Variables
    public bool debugMode = true;
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
    [SerializeField]
    bool canEnableUmbrella = true;
    bool active = true;
    bool wallSliding = false;
    int wallDirX;
    Vector2 input;
    Vector3 aimDirection;
    Vector3 velocity;
    Controller2D controller;
    SpriteRenderer spriteRenderer;
    PlayerInput playerInput;
    PlayerStats playerStats;
    private bool able = true;
    //int sanityPoints;   //esto es provisional


    public PlayerMovement()
    {
        UmbrellaUnlocked = false;
    }

    #endregion

    #region Public Properties
    public bool UmbrellaUnlocked { get; set; }
    public Vector3 Velocity { get {return velocity; } }
    public bool CanEnableUmbrella { get {return canEnableUmbrella;}}
    public bool WallSliding { get { return wallSliding; } }
    public Vector3 AimDirection { get { return aimDirection; } }
    public int WallDirX { get { return wallDirX; } }
    public bool debugUmbrella = false;
    #endregion


    public bool countingForAttacking = false;
    public bool countingForProtecting = false;
    private float currentTimeA = 0f;
    private float maxTimeA = 0.5f;
    private float currentTimeP = 0f;
    private float maxTimeP = 0.5f;

    // Set up references
    private void Awake()
    {
        /*controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();*/

        spriteRenderer = GetComponent<SpriteRenderer>();
        controller = GetComponentInParent<Controller2D>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerStats = GetComponentInParent<PlayerStats>();
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
            if (countingForAttacking && !countingForProtecting)
            {
                currentTimeA += Time.deltaTime;
                if (currentTimeA >= maxTimeA)
                {
                    countingForAttacking = false;
                    currentTimeA = 0f;
                    controller.collisions.isAttacking = false;
                    playerStats.SetAction(0);
                }
            }

            if (countingForProtecting && !countingForAttacking)
            {
                currentTimeP += Time.deltaTime;
                if (currentTimeP >= maxTimeP)
                {
                    countingForProtecting = false;
                    currentTimeP = 0f;
                    controller.collisions.isProtecting = false;
                    playerStats.SetAction(0);
                }
            }

            EnableUmbrella();

            input = playerInput.DirectionalInput;

            Protect();
            Attack();
            //Protect();    // I gave priority to Protect() over Attack() -Fieldins

            wallDirX = (controller.collisions.left) ? -1 : 1;

            ResetGravity();

            CheckIfUnlockedUmbrella();

            HorizontalMovement();

            FlipSprite();

            CheckIfCanSoar();

            CheckIfCanEnableUmbrellaAgain();

            HandleWallSliding(wallDirX);

            HandleJumping(wallDirX);

            controller.Move(velocity * Time.deltaTime, input);

            if (playerInput.DirectionalInput.x == 0) velocity.x = 0;
        }
    }

    public void EnableUmbrella()
    {
#if UNITY_EDITOR
        if (debugMode && Input.GetKeyDown(KeyCode.P))
        {
            if (UmbrellaUnlocked)
            {
                UmbrellaUnlocked = false;
            }
            else if (!UmbrellaUnlocked)
            { 
                UmbrellaUnlocked = true;
            }
        }
#endif
    }

    public void Attack()
    {
        if (playerInput.CaptureMouseLeftClick() && !controller.collisions.isAttacking && !controller.collisions.isProtecting && UmbrellaUnlocked)
        {   //si no estaba atacando ni protegiendose, ataca.
            controller.collisions.isAttacking = true;
            playerStats.SetAction(1);
            countingForAttacking = true;
            //Debug.Log("attacking");
        }
    }

    public void Protect()
    {   //si no estaba protegiendose ni atacando, protegese.
        if (playerInput.CaptureMouseRightClick() && !controller.collisions.isProtecting && !controller.collisions.isAttacking && UmbrellaUnlocked && able)
        {
            controller.collisions.isProtecting = true;
            playerStats.SetAction(-1);
            countingForProtecting = true;
            //Debug.Log("protecting");
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
            if (wallSliding && controller.HitTag == "Climbable")
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    //velocity.y = wallJumpClimb.y;
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
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            if (controller.HitTag == "Climbable") //PARA QUE NO FRENE SI ES PLATAFORMA Q SE MEUVE
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
                    //Debug.Log("HOLA");
                    controller.collisions.almostJumping = true;
                    timeToWallUnstick -= Time.deltaTime;
                }
                else if (input.x == wallDirX)
                {
                    controller.collisions.almostJumping = false;
                    timeToWallUnstick = wallStickTime;
                    
                }
            }
            else
            {
                controller.collisions.almostJumping = false;
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    private void HorizontalMovement()
    {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        if (controller.collisions.isSoaring) velocity.y = gravity * Time.deltaTime;
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        


        if (input.x != 0)   //si se mueve horizontalmente
        {
            aimDirection = Mathf.Sign(input.x) == 1 ? Vector3.right : Vector3.left; //constantly check which direction is the _playerMovement looking at          
        }

    }

    private void CheckIfCanEnableUmbrellaAgain()
    {
        if (controller.collisions.isSoaring)    //si  está planeando
        {
            if (controller.collisions.below || controller.collisions.left || controller.collisions.right || playerInput.CaptureJumpInputDown()) //si colisiona en cualquier dirección o se pulsa espacio
            {
                canEnableUmbrella = false;

                controller.collisions.isSoaring = false;
                playerStats.SetAction(0);
            }

            


               
        }
        else //if no está planeando
        {
            //Debug.Log("no soaring");
            gravity = saveGravity;

            if (controller.collisions.below || controller.collisions.left || controller.collisions.right)
            {
                if (controller.collisions.below)
                    controller.collisions.almostJumping = false;
                canEnableUmbrella = true;
            }
        }
    }

    private void CheckIfCanSoar()
    {
        if (!controller.collisions.below && !controller.collisions.left && !controller.collisions.right)
        {
            if (!controller.collisions.isSoaring) //si está en el aire (sin planear)
            {
                if (playerInput.CaptureSoarInput("down") && canEnableUmbrella) //y se pulsa espacio y puede usar el paraguas
                {
                    //Debug.Log("soaring");
                    velocity.y = 0;
                    canEnableUmbrella = false;
                    gravity = gravityWhilePlanning;
                    controller.collisions.isSoaring = true;
                    playerStats.SetAction(-2);
                    // FALTA HACER LA TRANSICIÓN PARA QUE EL ANIMATOR PASE DE DE SOARING A JUMPING !!
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

        controller.collisions.isSoaring = false;
        playerStats.SetAction(0);
    }

    public Vector3 GetAimDirection()
    {
        return aimDirection;
    }

    public void StopPlayer(bool stop)
    {
        active = !stop;
    }

    //TODO revisar propiedades publicas de los sanity points    - Revisado

    public void StopConsumingSanity(bool lockResource)
    {
        able = !lockResource;
        if (lockResource)
        {
            controller.collisions.isSoaring = false;
            // --INSERT MEC MEC SFX HERE--
            print("Can't use more sanity!");
        }
    }
}
