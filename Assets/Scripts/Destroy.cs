using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    private float fallingSpeed;
    public float minFallingSpeed;
    public float maxFallingSpeed;
	// Use this for initialization
	void Start () {
        fallingSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);
	}
	
	// Update is called once per frame
	void Update () {

        Move();
	}

    public void Move()
    {

        transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime);

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            print("Nos ha golpeado un proyectil!");

        }
        else if (collider.gameObject.CompareTag("AWall") || collider.gameObject.CompareTag("Through"))
        {

            print("LOL");
            Destroy(this.gameObject);

        }
    }
}
