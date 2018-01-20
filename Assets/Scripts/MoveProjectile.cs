using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{
    [HideInInspector]
    public Vector3 moveDirection;
    public float bulletSpeed = 10f;
    private float invLocalScaleX;
    // Update is called once per frame
    void Start()
    {
        invLocalScaleX = -this.transform.localScale.x;
    }

    void Update()
    {
        
        MoveBullet(moveDirection);
    }

    public void MoveBullet(Vector3 direction)
    {
        if (direction == Vector3.right)
            this.transform.localScale = new Vector3(invLocalScaleX, this.transform.localScale.y, 
                this.transform.localScale.z);
        else
            this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        this.transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //CAUSALE DAÑO Y REPULSION
            Destroy(this.gameObject);
        }
    }
}
