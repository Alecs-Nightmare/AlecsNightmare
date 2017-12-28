using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    public bool followPlayer;
    public Transform target;
    public float enemySpeed;
    private Vector3 spawnPosition;
    private string whereIsThePlayer;
    private float localScaleX;
    private float invLocalScaleX;
    private GameObject[] bones;
    private GlassweatherAnimationController glAnimContr;

    // Use this for initialization
    private void Awake()
    {
        glAnimContr = GetComponent<GlassweatherAnimationController>();
        spawnPosition = this.transform.position;
        

    }
    void Start () {

        localScaleX = this.transform.localScale.x;
        invLocalScaleX = localScaleX * -1;

        
	}
	
	// Update is called once per frame
	void Update () {

        whereIsThePlayer = target.position.x >= transform.position.x ? "right" : "left";
        ManageEnemyMovement();
       
	}

    public void ManageEnemyMovement()
    {
        if (followPlayer)
        {
            glAnimContr.alertingPlayer = true;
            glAnimContr.ignoringPlayer = false;
            print("follow player");
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.position.x, this.transform.position.y, 0), enemySpeed * Time.deltaTime);
            if (whereIsThePlayer == "right")
                this.transform.localScale = new Vector3(localScaleX, this.transform.localScale.y, this.transform.localScale.z);
            else
                this.transform.localScale = new Vector3(invLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);

        }
        else
        {
            glAnimContr.ignoringPlayer = true;
            glAnimContr.followingPlayer = false;
            glAnimContr.alertingPlayer = false;
            this.transform.position = Vector3.MoveTowards(this.transform.position, spawnPosition, (enemySpeed - 1) * Time.deltaTime);
            if (whereIsThePlayer == "right")
                this.transform.localScale = new Vector3(invLocalScaleX, this.transform.localScale.y, this.transform.localScale.z);
            else
                this.transform.localScale = new Vector3(localScaleX, this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    public float DistanceToPlayer()
    {
        return Vector2.Distance(target.position, this.transform.position);
    }
}
