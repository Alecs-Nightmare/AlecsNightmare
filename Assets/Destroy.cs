using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public float bulletSpeed = 10f;
    GameObject WineDjinn;
	// Use this for initialization
	void Start () {
        WineDjinn = GameObject.Find("WineDjinn");
	}
	
	// Update is called once per frame
	void Update () {
        MoveBullet(WineDjinn.GetComponent<WineDjinnController>().aimDirection);
	}
    
    public void MoveBullet(Vector3 direction)
    {
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
