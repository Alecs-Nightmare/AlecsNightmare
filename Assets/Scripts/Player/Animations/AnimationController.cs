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
    }

    void Start ()
    {
        _playerMovement = this.GetComponent<PlayerMovement>();
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
        if (controller.collisions.isSoaring && _playerMovement.CanEnableUmbrella && anim.GetBool(AnimatorParameters.armado))
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.planeando);
        }
        else if(!controller.collisions.isSoaring && _playerMovement.Velocity.y < 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
        }
        else if (Mathf.Abs(_playerMovement.Velocity.x) > 0 && controller.collisions.below)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.andando);
        }
        else if (_playerMovement.Velocity.y > 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.saltando);
        }
        else if(m_playerInput.DirectionalInput.x == 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.idle);
        }
        else if(controller.collisions.WallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.escalando);
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
