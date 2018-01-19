using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineDjinnController : MonoBehaviour {

    public WineDjinnAnimationController WDAnimController;
    public Vector3 aimDirection;
    public bool playerRight;
    public GameObject target;
    public GameObject bullet;
    private float localScaleX;
    private float invLocalScaleX;

    public float TimeCadency = 1f;
    public float currentTimeCadency = 0f;
    // Use this for initialization
    private void Awake()
    {
        WDAnimController = GetComponent<WineDjinnAnimationController>();
    }
    void Start () {
        localScaleX = transform.localScale.x;
        invLocalScaleX = -localScaleX;
	}
	
	// Update is called once per frame
	void Update () {
        //CheckWhereIsPlayer();
        setAimDirection();
        ManageDjinnWine();
	}
    public void CheckWhereIsPlayer()
    {
        if (transform.position.x > target.transform.position.x)
        {
            playerRight = false;
           
        }
        else
        {
            playerRight = true;
        }
    }
    public void setAimDirection()
    {
        if (playerRight)
        {
            print("AIM TO THE RIGHT");
            aimDirection = Vector3.right;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }

        else
        {
            print("AIM TO THE LEFT");
            aimDirection = Vector3.left;
            transform.localScale = new Vector3(invLocalScaleX, transform.localScale.y, transform.localScale.z);
        }
              
    }
    public void ManageDjinnWine()
    {
        
        if (target != null)
        {
            CheckWhereIsPlayer();
            if (WDAnimController.melee)
            {
                print("HOSTION!");
                //daño al enemigo y repulsion melee.
            }
            //if (WDAnimController.shooting && !WDAnimController.melee)
            if (currentTimeCadency >= TimeCadency)
            {
                ShootTarget();
                currentTimeCadency = 0f;
            }
            else
            {
                currentTimeCadency += Time.deltaTime;
            }
                
        }
    }
    public void ShootTarget()
    {
        
        Instantiate(bullet, new Vector3(this.transform.position.x + 1f, this.transform.position.y - 1f, this.transform.position.z), this.transform.rotation);
        

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WDAnimController.preshooting = true;
            WDAnimController.idle = false;
            target = collision.gameObject;
            CheckWhereIsPlayer();
            
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            WDAnimController.shooting = false;
            WDAnimController.preshooting = false;
            WDAnimController.postshooting = true;
            
            target = null;
        }
    }
}
