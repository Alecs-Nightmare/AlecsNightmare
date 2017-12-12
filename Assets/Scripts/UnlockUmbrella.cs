using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockUmbrella : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.GetComponent<PlayerMovement>().UmbrellaUnlocked = true;
            Destroy(this.gameObject);
        }
    }
}
