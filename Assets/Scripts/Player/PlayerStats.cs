using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    GameObject manager;
    GameManager gameManager;        // Reference to the Game Manager
    PlayerMovement playerMovement;  // Reference to character's Player class

    public int MaxSanity = 100;     // This is the main resource
    [SerializeField]
    private int currentSanity;      // We separate current and maxim values because it can be increased during the game
    [SerializeField]
    private int currentState;
    [SerializeField]
    private float deathCooldown = 2.66f;   // Cooldown time to respawn
    [SerializeField]
    private float hitCooldown = 0.33f;     // Cooldown time to get hurt
    [SerializeField]
    private float repulsionForce = 20f;
    private float time;


    // Set up references
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        gameManager = manager.GetComponent<GameManager>();
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Use this for initialization
    void Start ()
    {
        ResetStats();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch(currentState)
        {
            case -1:        // Dead (waiting for respawn)

                time += Time.smoothDeltaTime;
                // -deactivate player's movement controller class after refactoring-
                if (time >= deathCooldown)
                {
                    time = 0;
                    ResetStats();  // -for the moment we manage this here-
                    //player.StopPlayer(false);
                    playerMovement.enabled = true;
                    print("Respawned!");
                    // --INSERT REBIRTH SFX HERE--
                }
                break;

            case 0:         // Damage cooldown (otherwise it'd get continuos damage)

                time += Time.smoothDeltaTime;
                if (time >= hitCooldown)
                {
                    time = 0;
                    currentState = 1;
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    GetComponent<Collider2D>().isTrigger = true;
                    //player.StopPlayer(false);
                    playerMovement.enabled = true;
                }
                break;

            case 1:         // Player is active

                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (currentState == 1 && col.gameObject.tag == "Enemy")
        {
            EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
            if (enemy.AskForLethal())   // if enemy object is lethal we shouldn't knock back!
            {
                currentSanity = 0;
            }
            else
            {
                // knock back!
                GetComponent<Rigidbody2D>().AddForce(repulsionForce*(new Vector2 (this.transform.position.x - col.transform.position.x, 1f)), ForceMode2D.Impulse);

                currentSanity -= enemy.GetAttackPower();
            }
            if (currentSanity <= 0)
            {
                currentState = -1;

                // fall!
                GetComponent<Rigidbody2D>().gravityScale = 2f;

                print("Death!");
                // --INSERT DEATH SFX HERE--
            }
            else
            {
                currentState = 0;
                GetComponent<Collider2D>().isTrigger = false;
                print("Hit!");
                // --INSERT DAMAGE SFX HERE--
            }
            //player.StopPlayer(true);
            playerMovement.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (currentState == 0 && col.gameObject.layer == 9)  // Crashes with the scenario!
        {
            time = hitCooldown;
            print("Crashes with the scenario!");
            // --INSERT CRASHING SFX HERE--
        }
    }

    // Resets and respawns the player
    public void ResetStats()
    {
        /* here we should play some sound effect, particles, etc-
        this part only works when loaded with the Loader
        this.transform.position = GameManager.instance.Respawn().position;
        print("Respawned at " + GameManager.instance.Respawn().position);
        */
        this.transform.position = gameManager.GetRespawnTransform().position;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
        currentSanity = MaxSanity;
        currentState = 1;
        GetComponent<Collider2D>().isTrigger = true;
    }

    public int GetState()
    {
        return currentState;
    }
}
