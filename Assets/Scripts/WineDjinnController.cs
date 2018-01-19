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
        //Debug.Log(aimDirection);
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
            
            aimDirection = Vector3.right;
            transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
        }

        else
        {
    
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
                Shoot();
                currentTimeCadency = 0f;
            }
            else
            {
                currentTimeCadency += Time.deltaTime;
            }
                
        }
    }
    public void Shoot()
    {
        
        GameObject aux;
        Instantiate(aux = bullet, this.transform.position, Quaternion.identity);
        aux.GetComponent<MoveProjectile>().moveDirection = aimDirection;



        Instantiate(aux = bullet, 
            aimDirection == Vector3.right ? 
            new Vector3(this.transform.position.x - 1f, this.transform.position.y - 0.5f, this.transform.position.z) :
            new Vector3(this.transform.position.x + 1f, this.transform.position.y - 0.5f, this.transform.position.z)
            , Quaternion.identity);
        aux.GetComponent<MoveProjectile>().moveDirection = aimDirection;

        /*
        Instantiate(bullet, new Vector3(this.transform.position.x + 0f, this.transform.position.y + 0f, 
            this.transform.position.z), aimDirection == Vector3.left ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(0f, 180f, 0f));
        /*
        Instantiate(bullet, new Vector3(this.transform.position.x + 1f, this.transform.position.y -0.5f,
            this.transform.position.z), aimDirection == Vector3.left ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(0f, 180f, 0f));
        */

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
