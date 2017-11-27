using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    Animator anim;
    AnimatorStateInfo stateinfo;
    int idleStateHash = Animator.StringToHash("Base Layer.Idle");
    int correrStateHash = Animator.StringToHash("Base Layer.Correr Desarmado");
    int saltoStateHash = Animator.StringToHash("Base Layer.Salto");

    void Start () {
        anim = this.GetComponent<Animator>();
	}
	
	
	void Update () {
        float horizontal = Input.GetAxisRaw("Horizontal");
        stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        
        if (horizontal != 0)//Si el personaje se mueve horizontalmente
        {
            anim.SetBool("corriendo", true);

        }
        else {
            anim.SetBool("corriendo", false);
        }

        if ((stateinfo.fullPathHash == correrStateHash || stateinfo.fullPathHash == idleStateHash))
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                anim.SetTrigger("Salto");
            }

        }
    }
}
