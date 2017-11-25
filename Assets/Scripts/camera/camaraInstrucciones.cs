using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camaraInstrucciones : MonoBehaviour {   
    
    void Update () {       
        //Guardamos en una variable las posiciones en Y de LimiteSuperior y LimiteInferior
        float limiteSuperior = GameObject.Find("LimiteSuperior").transform.position.y;
        float limiteInferior = GameObject.Find("LimiteInferior").transform.position.y;
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //Mientras la posición de la cámara en Y sea inferior a la de LimiteSuperior se puede mover hacia arriba 
            if (transform.position.y < limiteSuperior) {
                //Movemos la cámara 0.15 puntos hacia arriba
                transform.Translate(new Vector3(0, 0.15f, 0));                
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //Mientras la posición de la cámara en Y sea superior a la de LimiteInferior se puede mover hacia abajo
            if (transform.position.y > limiteInferior) { 
                //Movemos la cámara 0.15 puntos hacia abajo
                transform.Translate(new Vector3(0, -0.15f, 0));
            }
        }
    }
}
