using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    Animator anim;
    Controller2D controller;
    Vector3 velocity;
    Player player;
    bool canEnableUmbrella = true;
    AnimatorStateInfo stateinfo;
    Animatorparameters parameters;
    int saltoArmadoStateHash = Animator.StringToHash("Base Layer.AlexJumpArmed");
    int saltoDesrmadoStateHash = Animator.StringToHash("Base Layer.AlexJumpDesArmed");
    /*int idleStateHash = Animator.StringToHash("Base Layer.Idle");
    int correrStateHash = Animator.StringToHash("Base Layer.Correr Desarmado");
    int saltoStateHash = Animator.StringToHash("Base Layer.Salto");*/

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    void Start () {
        parameters = this.GetComponent<Animatorparameters>();
        anim = this.GetComponent<Animator>();
        player = this.GetComponent<Player>();
    }


    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!controller.collisions.below && !controller.collisions.left && !controller.collisions.right)
        {


            if (!controller.collisions.isPlanning && player.getVelocity().y != 0) //si está en el aire (sin planear)
            {
                if (Input.GetKeyDown(KeyCode.Space) && player.getCanEnableUmbrella())//y se pulsa espacio y puede usar el paraguas
                {

                    //activar animacion paraguas (planeo)
                    anim.SetBool(parameters.getEnTierra(), false);
                    anim.SetBool(parameters.getCorrer(), false);
                    anim.SetBool(parameters.getSaltar(), false);
                    anim.SetBool(parameters.getPlanear(), true);
                    anim.SetBool(parameters.getPared(), false);

                }
                //si no pulsa espacio o no se puede usar el paraguas
                else
                {
                    stateinfo = anim.GetCurrentAnimatorStateInfo(0);
                    if (stateinfo.fullPathHash != saltoArmadoStateHash && stateinfo.fullPathHash != saltoDesrmadoStateHash)//si no está saltando (simplemente cae)
                    {
                        anim.SetBool(parameters.getEnTierra(), false);
                        anim.SetBool(parameters.getPlanear(), false);
                        anim.SetBool(parameters.getSaltar(), false);
                        anim.SetBool(parameters.getCorrer(), false);
                        anim.SetBool(parameters.getPared(), false);

                    }
                    else {
                        print("salta");
                        anim.SetBool(parameters.getSaltar(), true);
                    }

                }
            }
        }
        if (controller.collisions.isPlanning)//si  está planeando
        {


            if (controller.collisions.below || controller.collisions.left || controller.collisions.right || Input.GetKeyDown(KeyCode.Space))//si colisiona en cualquier dirección o se pulsa espacio
            {

                if (controller.collisions.below)//si toca el suelo
                {
                    anim.SetBool(parameters.getEnTierra(), true);
                    anim.SetBool(parameters.getPlanear(), false);
                }
                else
                {
                    if (controller.collisions.left || controller.collisions.right) {//si se ha chocado con una pared
                        //activar animación trepar
                        anim.SetBool(parameters.getPared(), true);
                        anim.SetBool(parameters.getPlanear(), false);
                        anim.SetBool(parameters.getSaltar(), false);
                        anim.SetBool(parameters.getEnTierra(), false);
                    }
                    else {//si se ha pulsado espacio
                        //activar animación caída
                        anim.SetBool(parameters.getPlanear(), false);
                        anim.SetBool(parameters.getEnTierra(), false);
                        anim.SetBool(parameters.getPared(), false);
                    }
                    
                }

            }
        }

        /*else //si no está planeando
        {
            if (controller.collisions.below)//si toca el suelo
            {
                anim.SetBool(parameters.enTierra, true);
                anim.SetBool(parameters.saltar, false);
                anim.SetBool(parameters.planear, false);
            }
        }*/

        if (horizontal != 0)//si se mueve horizontalmente
        {
            if (controller.collisions.below)//si está tocando suelo activar animación corriendo
            {
                anim.SetBool(parameters.getCorrer(), true);
                anim.SetBool(parameters.getEnTierra(), true);
                anim.SetBool(parameters.getSaltar(), false);

                if (controller.collisions.right || controller.collisions.left)
                {//si toca pared activar animación pared
                    anim.SetBool(parameters.getPared(), true);
                }
                else {
                    anim.SetBool(parameters.getPared(), false);
                }
            }
            else
            {//si no está tocando suelo
                anim.SetBool(parameters.getCorrer(), false);
                anim.SetBool(parameters.getEnTierra(), false);

                if (controller.collisions.right || controller.collisions.left)
                {//si toca pared activar animación pared
                    anim.SetBool(parameters.getPared(), true);
                    anim.SetBool(parameters.getSaltar(), false);
                    
                }
                else {
                    anim.SetBool(parameters.getPared(), false);
                }
            }

        }

        else
        {//si no se mueve horizontalmente
            if (controller.collisions.below)//si está tocando suelo activar animación idle
            {
                anim.SetBool(parameters.getCorrer(), false);
                anim.SetBool(parameters.getEnTierra(), true);
                anim.SetBool(parameters.getSaltar(), false);
                anim.SetBool(parameters.getPlanear(), false);
            }

            else
            {//si no toca suelo
                anim.SetBool(parameters.getEnTierra(), false);

                if (controller.collisions.right || controller.collisions.left)
                {//si toca pared activar animación pared
                    anim.SetBool(parameters.getPared(), true);
                    if (player.getVelocity().y < 0)
                    {//si esta cayendo
                        anim.SetBool(parameters.getSaltar(), false);
                    }

                }
                else
                {
                    anim.SetBool(parameters.getPared(), false);
                }
            }
        }

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)//Si está en la pared
        {
            //activar animación pared
            anim.SetBool(parameters.getPared(), true);
            anim.SetBool(parameters.getEnTierra(), false);
            anim.SetBool(parameters.getSaltar(), false);
            anim.SetBool(parameters.getPlanear(), false);
            anim.SetBool(parameters.getCorrer(), false);
        }

        if (Input.GetKeyDown(KeyCode.Space))//si pulsa espacio
        {

            if (player.getCurrentJump() == player.getJumpsToPlane() && player.getCanEnableUmbrella()) //si puede planear: planea
            {
                print("planea");
                //activar animación paraguas
                anim.SetBool(parameters.getPlanear(), true);
                anim.SetBool(parameters.getEnTierra(), false);
                anim.SetBool(parameters.getCorrer(), false);
                anim.SetBool(parameters.getSaltar(), false);
                anim.SetBool(parameters.getPared(), false);

            }

            else if (controller.collisions.below)//si estaba en tierra: salta
            {
                //activar animación salto
                anim.SetBool(parameters.getSaltar(), true);
                anim.SetBool(parameters.getEnTierra(), false);
                anim.SetBool(parameters.getPlanear(), false);
                anim.SetBool(parameters.getCorrer(), false);
            }
            else if (controller.collisions.left || controller.collisions.right) {//si estaba en la pared: salta
                anim.SetBool(parameters.getSaltar(), true);
                anim.SetBool(parameters.getEnTierra(), false);
                anim.SetBool(parameters.getPlanear(), false);
                anim.SetBool(parameters.getCorrer(), false);
                anim.SetBool(parameters.getPared(), false);
            }


        }

    }
}
