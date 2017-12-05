using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    GameObject manager;
    GameManager gameManager;        // Reference to the Game Manager
    Player player;                  // Reference to character's Player class

    public int MaxSanity = 100;     // This is the main resource
    [SerializeField]
    private int currentSanity;      // We separate current and maxim values because it can be increased during the game
    [SerializeField]
    private int currentState;
    [SerializeField]
    private float deathCooldown = 3f;   // Cooldown time to respawn
    [SerializeField]
    private float hitCooldown = 1f;     // Cooldown time to get hurt
    private float time;
    private float fraction = 3f;


    // Set up references
    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        gameManager = manager.GetComponent<GameManager>();
        player = GetComponentInParent<Player>();
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
                    player.StopPlayer(false);
                }
                break;

            case 0:         // Damage cooldown (otherwise it'd get continuos damage)

                time += Time.smoothDeltaTime;
                if (time >= hitCooldown)
                {
                    time = 0;
                    currentState = 1;
                    player.StopPlayer(false);
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
            if (enemy.AskForLethal())
            {
                currentSanity = 0;
            }
            else
            {
                currentSanity -= enemy.GetAttackPower();
            }
            if (currentSanity <= 0)
            {
                currentState = -1;
                print("Death!");
            }
            else
            {
                currentState = 0;
                print("Hit!");
            }
            player.StopPlayer(true);
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
        currentSanity = MaxSanity;
        currentState = 1;
    }
}
