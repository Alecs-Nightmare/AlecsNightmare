﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrearDestruirAgua1 : MonoBehaviour {

	public GameObject objeto;
	int cantidadObjeto;

	void OnCollisionEnter2D(Collision2D col){	

		if (cantidadObjeto == 0) {
			Instantiate (objeto, transform.position, transform.rotation);
			cantidadObjeto++;
		}
	}

	void OnCollisionExit2D(){	
		float tiempoVida = 10f;	
		Destroy (GameObject.Find("CaerAlAgua(Clone)"),tiempoVida);
		cantidadObjeto--;
	}


}