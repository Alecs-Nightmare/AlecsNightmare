using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private Animator anim;
    private Controller2D controller;
    private PlayerMovement _playerMovement;
    private PlayerInput m_playerInput;
    

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        anim = this.GetComponent<Animator>();
        m_playerInput = GetComponent<PlayerInput>();
        _playerMovement = this.GetComponent<PlayerMovement>();
    }

    void Start ()
    {
        
    }

    enum PlayerState
    {
        idle = 0,
        andando = 1,
        saltando = 2,
        cayendo = 3,
        escalando = 4,
        planeando = 5,
        preparadoSalto = 6,
        atacando = 7,
        protegiendo = 8
    }

    void Update()
    {
        /*
        if (controller.collisions.isAttacking)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.atacando);
            //controller.collisions.isAttacking = false;
        }
        else if (controller.collisions.isProtecting)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.protegiendo);
            //controller.collisions.isProtecting = false;
        }
        */
        if (controller.collisions.isAttacking && !controller.collisions.isSoaring && !_playerMovement.WallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.atacando);   
        }
        else if (controller.collisions.isProtecting && !controller.collisions.isSoaring && !_playerMovement.WallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.protegiendo);
        }
        else if (!controller.collisions.isSoaring && _playerMovement.Velocity.y < 0 && !controller.collisions.below && !controller._standingOnPlatform && !_playerMovement.WallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
        }
        else if (controller.collisions.isSoaring)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.planeando);
            
        }
        else if (_playerMovement.WallSliding && controller.collisions.almostJumping)
        {
            //print("PENE DELGADO");  // wat
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.preparadoSalto);
        }
        else if (_playerMovement.WallSliding && !controller.collisions.almostJumping)
        {
            //print("PENE GORDO");    // dejad las babosas
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.escalando);
            if (_playerMovement.AimDirection.x != _playerMovement.WallDirX && !controller.collisions.isSoaring)
            {
                anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
            }
        }
       
        else if (Mathf.Abs(m_playerInput.DirectionalInput.x) > 0 && controller.collisions.below)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.andando);
        }

        else if (m_playerInput.DirectionalInput.x == 0 && controller.collisions.below && !controller.collisions.isAttacking && !controller.collisions.isProtecting)
        {

            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.idle);
        }
        
        else if (_playerMovement.Velocity.y > 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.saltando);
        }


        //if umbrella unlocked
        if (_playerMovement.UmbrellaUnlocked)
        {
            anim.SetBool(AnimatorParameters.armado, true);
        }
        else
        {
            anim.SetBool(AnimatorParameters.armado, false);
        }
    }

    [System.Serializable]
    public class AnimatorParameters
    {
        public static string estado = "estado";
        public static string armado = "armado";
    }
}
