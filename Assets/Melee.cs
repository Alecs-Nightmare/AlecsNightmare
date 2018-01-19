using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour {
    public WineDjinnAnimationController WDAnimContr;

    // Use this for initialization
    private void Awake()
    {
        WDAnimContr = GetComponentInParent<WineDjinnAnimationController>();
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WDAnimContr.shooting = false;
            WDAnimContr.melee = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WDAnimContr.preshooting = true;
            WDAnimContr.melee = false;
        }
    }
}
