using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoArena : MonoBehaviour {

    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    void Update(){
		if(Input.GetAxisRaw("Horizontal") != 0){




            this.gameObject.GetComponent<ParticleSystem>().Play(true);

            
            //transform.Rotate (0, 0, 180);
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}

       
        else
        {

            this.gameObject.SetActive(false);


        }
        
	}

}
