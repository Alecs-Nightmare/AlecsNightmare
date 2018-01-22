using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class EnemyStats : MonoBehaviour {

    [SerializeField]
    private int attackPower = 10;
    [SerializeField]
    private int hitPoints = 100;
    [SerializeField]
    private int recover = 25;       // Sanity to recover when defeated
    [SerializeField]
    private float bouncingFactor = 2;
    [SerializeField]
    private bool isLethal;          // Set true for enemies than one-hit kill you
    [SerializeField]
    private bool isToucheable;      // Set true for enemies that can be jumped above
    [SerializeField]
    private bool isDestroyable;     // Set true for enemies that can be killed
    [SerializeField]
    private bool isVolatile;        // Set true for enemies that are destroyed when they impact with the player (projectiles)
    private bool dead;
    [SerializeField]
    private bool itFades = false;
    [SerializeField]
    private float liveTime = 0f;    // Time to stay for decaying enemies (set to zero or less for infinite)
    [SerializeField]
    private float deathDelay = 1f;
    private float timer = 0f;
    SpriteMeshInstance[] sprites;

    // Set up references
    void Awake()
    {
        sprites = GetComponentsInChildren<SpriteMeshInstance>();    // NO LAS REFERENCIA Y NO PUEDO PERDER MÁS TIEMPO CON ESTO
    }

    // Use this for initialization
    void Start ()
    {
        dead = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (liveTime > 0)
        {
            liveTime -= Time.smoothDeltaTime;
            if (liveTime <= 0)
            {
                dead = true;
            }
        }

        else if (dead)
        {
            timer += Time.smoothDeltaTime;
            if (itFades)
            {
                foreach (SpriteMeshInstance sprite in sprites)
                {
                    //sprite.color = new Color(0f, 0f, 0f, 0f);
                }
                if (timer >= deathDelay)
                {
                    // --INSERT DISAPPEARING SFX HERE--
                    // --INSERT DISAPPEARING PARTICLES HERE--
                    print(this + " has disappeared.");
                    Object.Destroy(this.gameObject);
                }
            }
            else
            {
                this.transform.localScale -= new Vector3(Time.smoothDeltaTime, Time.smoothDeltaTime, 0);
                this.transform.Rotate(new Vector3(0f, 0f, 50f) * timer);
                if (timer >= deathDelay)
                {
                    // --INSERT 'POP' SFX HERE--
                    // --INSERT DEATH PARTICLES HERE--
                    print(this + " has been destroyed.");
                    Object.Destroy(this.gameObject);
                }
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
        if (isVolatile || itFades) { dead = true; }
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

    public bool AskIfVolatile()
    {
        return isVolatile;
    }

    public bool AskIfFades()
    {
        return itFades;
    }

    public float GetBouncingFactor()
    {
        return bouncingFactor;
    }

    public int Hit(int damage)
    {
        if (isDestroyable)
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
        else
        {
            return 0;
        }
    }
}
