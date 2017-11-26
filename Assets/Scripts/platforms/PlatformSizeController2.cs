using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSizeController2 : MonoBehaviour {

    public bool shrinking;
    private float sizePlatformX;
    public float minSize;
    public float startWaitTime;
    private Vector3 temp;
    private float waitTime;

    // Use this for initialization
    void Start () {
        sizePlatformX = GetComponent<Transform>().localScale.x;
        waitTime = startWaitTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (shrinking)
        {
            if (transform.localScale.x > minSize)
            {
                temp = transform.localScale;
                temp.x -= Time.deltaTime;
                transform.localScale = temp;
            }
            else {
                waitTime -= Time.deltaTime;
                if (waitTime < 0) {
                    shrinking = false;
                    waitTime = startWaitTime;
                }
                
            }
        }
        else {
            if (transform.localScale.x < sizePlatformX)
            {
                temp = transform.localScale;
                temp.x += Time.deltaTime;
                transform.localScale = temp;
            }
            else {
                waitTime -= Time.deltaTime;
                if (waitTime < 0){
                    shrinking = true;
                    waitTime = startWaitTime;
                }
                    
            }
        }
	}
}
