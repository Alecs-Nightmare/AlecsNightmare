using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    [SerializeField]
    private int attackPower = 10;
    [SerializeField]
    private int hitPoints = 100;
    [SerializeField]
    private int recover = 25;   // Sanity to recover when defeated
    [SerializeField]
    private float bouncingFactor = 2;
    [SerializeField]
    private bool isLethal;      // Set true for enemies than one-hit kill you
    [SerializeField]
    private bool isToucheable;  // Set true for enemies that can be jumped above
    private bool dead;
    [SerializeField]
    private float deathDelay = 1f;
    private float timer = 0f;

    // Set up references
    void Awake()
    {

    }

    // Use this for initialization
    void Start ()
    {
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (dead)
        {
            timer += Time.smoothDeltaTime;
            this.transform.localScale -= new Vector3(Time.smoothDeltaTime, Time.smoothDeltaTime, 0);
            this.transform.Rotate(new Vector3(0f, 0f, 50f) * timer);
            if (timer >= deathDelay)
            {
                // --INSERT 'POP' SFX HERE--
                print(this+" has been destroyed.");
                Object.Destroy(this.gameObject);
            }
        }
	}
    
    /*void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            //print("Hit!");
            // opposite force pull?
            // -apply repulsion force on the player-
        }
    }*/

    public int GetAttackPower()
    {
        return attackPower;
    }

    public bool AskIfToucheable()
    {
        return isToucheable;
    }

    public bool AskForLethal()
    {
        return isLethal;
    }

    public float GetBouncingFactor()
    {
        return bouncingFactor;
    }

    public int Hit(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            // --deactivate AI/movement scripts here--
            dead = true;
            return recover;
        }
        else
        {
            return 0;
        }
    }
}
