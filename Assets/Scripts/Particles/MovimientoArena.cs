using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoArena : MonoBehaviour {

	void Update(){
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Rotate (180, 180, 180);
		}
	}

}
