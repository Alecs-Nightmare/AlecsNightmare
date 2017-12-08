using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    Animator anim;
    Controller2D controller;
    Player player;

    private void Awake()
    {
        controller = GetComponent<Controller2D>();
        anim = this.GetComponent<Animator>();
    }

    void Start ()
    {
        player = this.GetComponent<Player>();
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
        if (controller.collisions.isSoaring && player.getCanEnableUmbrella() && anim.GetBool(AnimatorParameters.armado))
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.planeando);
        }
        else if(!controller.collisions.isSoaring && player.getVelocity().y < 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.cayendo);
        }
        else if (Mathf.Abs(player.getVelocity().x) > 0 && controller.collisions.below)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.andando);
        }
        else if (player.getVelocity().y > 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.saltando);
        }
        else if(player.input == Vector2.zero && player.getVelocity().x == 0)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.idle);
        }
        else if(player.wallSliding)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)PlayerState.escalando);
        }


        //if umbrella unlocked
        if (player.UmbrellaUnlocked)
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
