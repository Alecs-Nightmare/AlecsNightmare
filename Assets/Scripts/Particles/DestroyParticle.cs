using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

	void OnCollisionEnter2D(){
		Destroy (gameObject);
	}
}
