using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float amplitude = 5.0f;

    private float startingY;

    void Start()
    {
        startingY = transform.position.y;
    }

	// Update is called once per frame
	void Update ()
	{
	    float newYPosition = startingY + (Mathf.Sin(Time.time)) * amplitude;
        this.transform.position = new Vector3(transform.position.x,newYPosition,transform.position.z);
	}
}
