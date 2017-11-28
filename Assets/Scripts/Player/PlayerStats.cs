using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int MaxSanity = 100;      // This is the main resource
    [SerializeField]
    private int currentSanity;              // We separate current and maxim values because it can be increased during the game
    [SerializeField]
    private int currentState;
    [SerializeField]
    private float cooldown = 3f;             // Cooldown time to respawn (damage cooldown is a fraction of this)
    private float time;
    private float fraction = 3f;


    // Set up references
    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        Respawn();
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch(currentState)
        {
            case -1:        // Dead (waiting for respawn)

                time += Time.smoothDeltaTime;
                // -deactivate player's movement controller class after refactoring-
                if (time >= cooldown)
                {
                    time = 0;
                    Respawn();  // -for the moment we manage this here-
                }
                break;

            case 0:         // Damage cooldown (otherwise it'd get continuos damage)

                time += Time.smoothDeltaTime;
                if (time >= cooldown/fraction)
                {
                    time = 0;
                    currentState = 1;
                }
                break;

            case 1:         // Player is active

                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (currentState == 1 && col.gameObject.tag == "Enemy")
        {
            EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
            currentSanity -= enemy.GetAttackPower();
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
        }
    }

    // Resets and respawns the player
    public void Respawn()
    {
        // -here we should play some sound effect, particles, etc-
        // --this part only works when loaded with the Loader--
        //this.transform.position = GameManager.instance.Respawn().position;
        //print("Respawned at " + GameManager.instance.Respawn().position);
        // --
        currentSanity = MaxSanity;
        currentState = 1;
    }
}
