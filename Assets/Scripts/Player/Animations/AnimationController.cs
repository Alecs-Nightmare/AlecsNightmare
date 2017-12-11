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
        planeando = 5
    }

    void Update()
    {
        

        Debug.Log(_playerMovement.WallSliding);
        //Debug.Log(_playerMovement.Velocity.x);
        if (controller.collisions.isSoaring && _playerMovement.CanEnableUmbrella && anim.GetBool(AnimatorParameters.armado))
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.planeando);
        }
        else if (_playerMovement.WallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.escalando);
            if (_playerMovement.AimDirection.x != _playerMovement.WallDirX)
            {
                anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
            }
        }
        else if(!controller.collisions.isSoaring && _playerMovement.Velocity.y < 0 && !controller.collisions.below)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
        }
      
        
       
        else if (Mathf.Abs(m_playerInput.DirectionalInput.x) > 0 && controller.collisions.below)
        {
            Debug.Log("andando");
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.andando);
        }

        else if (m_playerInput.DirectionalInput.x == 0 && controller.collisions.below)
        {

            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.idle);
        }
        
        else if (_playerMovement.Velocity.y > 0)
        {
            Debug.Log("saltando");
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
