using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleTrigger : MonoBehaviour {

	void OnTriggerEnter2D(){
		Destroy (gameObject);
	}
}
