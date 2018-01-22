using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    GameObject manager;
    GameManager gameManager;        // Reference to the Game Manager
    PlayerMovement playerMovement;  // Reference to character's Player class
    Animator animator;              // Reference to the player's animator

    //public int MaxSanity = 100;     // This is the main resource
    //[SerializeField]
    //private int currentSanity;      // We separate current and maxim values because it can be increased during the game
    [SerializeField]
    private int currentState;
    [SerializeField]
    private float deathCooldown = 2.66f;   // Cooldown time to respawn
    [SerializeField]
    private float hitCooldown = 0.33f;     // Cooldown time to get hurt
    [SerializeField]
    private float repulsionForce = 20f;
    private float time;
    private float gravity = 9.8f;
    private bool bouncing = false;
    private Vector3 respawnMargin = new Vector3(0f, 2f, 0f);
    [SerializeField]
    private int action = 0;         // -2 --> Soaring / -1 --> Defending / 0 --> Idle / 1 --> Charging / 2 --> Full load
    [SerializeField]
    private int sanityCost = 1;
    [SerializeField]
    private int sanityThreshold = 1;
    [SerializeField]
    private float tickTime = 0.33f;
    private float tickTimer = 0f;
    [SerializeField]
    private float maxLoadTime = 5f;
    [SerializeField]
    private float loadTimer = 0f;
    [SerializeField]
    private float specialAttVelocity = 1f;
    [SerializeField]
    private int weakAttPower = 0;
    [SerializeField]
    private int mediumAttPower = 100;
    [SerializeField]
    private int strongAttPower = 100;
    [SerializeField]
    private int regularAttPower = 100;
    [SerializeField]
    private GameObject magicMissile;
    //private Parti
    [SerializeField]
    private GameObject blockingParticles;


    // Set up references
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        gameManager = manager.GetComponent<GameManager>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        animator = GetComponentInParent<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        ResetStats();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // CHECK ACTION and increase timers
        if (action != 0)
        {
            if (action < 2)
            {
                tickTimer += Time.smoothDeltaTime;
                while (tickTimer >= tickTime)
                {
                    tickTimer -= tickTime;
                    // if can be substracted more sanity
                    if (gameManager.CheckSanity(sanityCost, sanityThreshold))
                    {
                        gameManager.SubstractSanity(sanityCost);
                    }
                    // if can't be substracted
                    else
                    {
                        // CALL PLAYER MOVEMENT TO STOP THE ACTION HERE
                        if (action < 0)
                        {
                            action = 0;
                            playerMovement.StopConsumingSanity(true);
                        }
                        else if (action == 1)
                        {
                            action = 2;
                            // --INSERT SEMI LOAD SFX HERE--
                            print("Max possible load has been reached!");
                        }
                        break;
                    }
                }
                if (action == 1)
                {
                    loadTimer += Time.smoothDeltaTime;
                    if (loadTimer > maxLoadTime)
                    {
                        loadTimer = maxLoadTime;
                        action = 2;
                        // --INSERT MAX LOAD SFX HERE--
                        print("Max load has been reached!");
                    }
                }
            }
        }

        switch (currentState)
        {
            // DEAD (waiting for respawn)
            case -1:

                time += Time.smoothDeltaTime;
                this.transform.Rotate(new Vector3(0f, 0f, 50f) * time);
                // -deactivate player's movement controller class after refactoring-
                if (time >= deathCooldown)
                {
                    if (gameManager.GetLifes() > 0)
                    {
                        time = 0;
                        ResetStats();  // -for the moment we manage this here-
                        gameManager.DeathRoutine(false);
                        animator.enabled = true;
                        playerMovement.enabled = true;
                        print("Respawned!");
                        // --INSERT REBIRTH SFX HERE--
                    }
                    else
                    {
                        gameManager.GameOver();
                    }
                }
                break;

            // DAMAGE COOLDOWN (otherwise it'd get continuos damage every frame)
            case 0:

                time += Time.smoothDeltaTime;
                if (time >= hitCooldown)
                {
                    time = 0;
                    currentState = 1;
                    if (GetComponent<Rigidbody2D>().gravityScale > 0f)
                    {
                        GetComponent<Rigidbody2D>().gravityScale = 0f;
                        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    }
                    if (!GetComponent<Collider2D>().isTrigger) { GetComponent<Collider2D>().isTrigger = true; }
                    if (!playerMovement.enabled) { playerMovement.enabled = true; }
                    print("Movement restaured!");
                    // Check if we are on an enemy
                    Collider2D collider = SelectInnerCollider();
                    if (collider != null) { CheckCollider(collider); }
                }
                break;

            // PLAYER IS ACTIVE
            case 1:

                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        CheckCollider(col);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (bouncing)
        {
            // IF THE COLLIDER IS A COLLECTABLE
            if (col.gameObject.layer == 12)
            {
                if (col.gameObject.tag == "Chips")
                {
                    gameManager.AddChips();
                    print("Takes a chip!");
                }
                else if (col.gameObject.tag == "Burger")
                {
                    gameManager.RecoverSanity(-1);
                    playerMovement.StopConsumingSanity(false);
                    print("Takes a burger!");
                }
                col.gameObject.SetActive(false);
            }

            // IF COLLIDES WITH THE SCENARIO
            else if (col.gameObject.layer == 9)
            {
                GetComponent<Rigidbody2D>().gravityScale = 0f;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Collider2D>().isTrigger = true;
                playerMovement.enabled = true;
                bouncing = false;
                // --INSERT CRASHING SFX HERE--
                print("Crashes with the scenario!");
            }
        }
    }

    private Collider2D SelectInnerCollider()
    {
        print("Looking for enemies after cooldown...");
        Collider2D target = null;
        Collider2D[] withinColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 1f);
        foreach (Collider2D col in withinColliders)
        {
            print(col.name);
            if (col.gameObject.tag == "Enemy")
            {
                int attack = 0;
                EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
                if (enemy.AskForLethal())
                {
                    return col;
                }
                else if (enemy.GetAttackPower() >= attack)
                {
                    target = col;
                }
            }
        }
        return target;
    }

    private void CheckCollider(Collider2D col)
    {
        if (currentState == 1 && col.gameObject.tag == "Enemy")
        {
            EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();

            // JUMPS OVER A TOUCHABLE ENEMY
            if (enemy.AskIfToucheable() && this.transform.position.y > col.transform.position.y + col.bounds.size.y/3)
            {
                // Adds sanity
                gameManager.RecoverSanity(enemy.Hit(regularAttPower));
                playerMovement.StopConsumingSanity(false);

                // Bounce!
                GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, repulsionForce*enemy.GetBouncingFactor()), ForceMode2D.Impulse);
                GetComponent<Rigidbody2D>().gravityScale = gravity;
                bouncing = true;
                // --INSERT ENEMY HIT SFX HERE--
                print("Attack!");
            }

            // TAKES DAMAGE FROM ENEMY
            else
            {
                // LETHAL ENEMY
                if (enemy.AskForLethal() && !enemy.AskIfVolatile())   // if enemy object is a static lethal we shouldn't knock back!
                {
                    gameManager.SubstractSanity(gameManager.GetCurrentSanity());
                    GetComponent<Rigidbody2D>().gravityScale = gravity/8;   // reduced gravity for water!
                }

                // NON LETHAL ENEMY
                else
                {
                    // Check if protecting
                    bool takesDamage = true;
                    if (action == -1)
                    {
                        if (playerMovement.GetAimDirection().x > 0 && this.transform.position.x < col.transform.position.x)
                        {
                            takesDamage = false;
                            // --INSERT BLOCKING SFX HERE--
                            print("Blocks " + col.name + "!");
                        }
                        else if (playerMovement.GetAimDirection().x < 0 && this.transform.position.x > col.transform.position.x)
                        {
                            takesDamage = false;
                            // --INSERT BLOCKING SFX HERE--
                            print("Blocks " + col.name + "!");
                        }
                    }

                    // knock back!
                    if (!takesDamage && enemy.AskIfVolatile())
                    {
                        print("Knock back is prevented when protecting against projectiles.");
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().AddForce(repulsionForce * (new Vector2(this.transform.position.x - col.transform.position.x, 1f)), ForceMode2D.Impulse);
                        GetComponent<Rigidbody2D>().gravityScale = gravity;
                    }

                    // Substracts sanity
                    if (takesDamage)
                    {
                        if (enemy.AskForLethal()) { gameManager.SubstractSanity(gameManager.GetCurrentSanity()); }
                        else { gameManager.SubstractSanity(enemy.GetAttackPower()); }
                    }
                }

                // Check if Player dies
                if (gameManager.GetCurrentSanity() <= 0)     // if it is dead, fall!
                {
                    currentState = -1;
                    animator.enabled = false;
                    gameManager.DeathRoutine(true);
                    Time.timeScale = 0.66f;
                    print("Death!");
                }

                // If Player doesn't die, gets knocked back!
                else
                {
                    GetComponent<Collider2D>().isTrigger = false;
                    bouncing = true;
                    currentState = 0;
                    // --INSERT DAMAGE SFX HERE--
                    print("Hit!");
                }
                playerMovement.enabled = false;
            }
        }

        // IF THE COLLIDER IS A COLLECTABLE
        else if ((currentState == 1 || bouncing) && col.gameObject.layer == 12)
        {
            if (col.gameObject.tag == "Chips")
            {
                gameManager.AddChips();
                print("Takes a chip!");
            }
            else if (col.gameObject.tag == "Burger")
            {
                gameManager.RecoverSanity(-1);
                playerMovement.StopConsumingSanity(false);
                print("Takes a burger!");
            }
            col.gameObject.SetActive(false);
        }

        // IF COLLIDES WITH THE SCENARIO
        else if (bouncing && col.gameObject.layer == 9)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //GetComponent<Collider2D>().isTrigger = true;
            //playerMovement.enabled = true;
            bouncing = false;

            // --INSERT CRASHING SFX HERE--
            print("Crashes with the scenario!");
        }
    }

    // Resets and respawns the player
    public void ResetStats()
    {
        /* here we should play some sound effect, particles, etc.
        this part only works when loaded with the Loader
        this.transform.position = GameManager.instance.Respawn().position;
        print("Respawned at " + GameManager.instance.Respawn().position);
        */
        this.transform.position = gameManager.ResetPlayer().position + respawnMargin;
        this.transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        currentState = 1;
        SetAction(0);
        playerMovement.StopConsumingSanity(false);
        GetComponent<Collider2D>().isTrigger = true;
        Time.timeScale = 1f;
    }

    public int GetState()
    {
        return currentState;
    }

    // This is not the best way to handle subactions but will work for now
    public void SetAction(int state)
    {
        if (state >= -2 && state <= 1)  // check if parameters are valid
        {
            if (action != state)        // if parameter won't update the action, ignore
            {
                if (state < 0)          // soaring and protecting (under zero) override attack loading
                {
                    tickTimer = 0;
                    loadTimer = 0;
                    print("Action timers have been reseted.");
                    action = state;
                }
                else if (action != 2)   // if max load (2) has been reached, ignore
                {
                    action = state;
                }
            }
        }
        else
        {
            print("ERROR: invalid parameters!");
        }
    }
}
