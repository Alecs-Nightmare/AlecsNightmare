using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonChairAnimationController : MonoBehaviour
{
    private Animator anim;
    private AttackPlayer ChairController;

    public bool idle;
    public bool shooting;
    public bool melee;
    public bool preshooting;
    public bool postshooting;

    // Use this for initialization
    private void Awake()
    {
        anim = GetComponent<Animator>();
        ChairController = GetComponent<AttackPlayer>();
    }

    void Start()
    {
        idle = true;

    }

    enum EnemyState
    {
        idle = 0,
        preshooting = 1,
        shooting = 2,
        postshooting = 3,
        melee = 4
    }
    // Update is called once per frame
    void Update()
    {

        if (idle)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.idle);

        }
        else if (melee)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.melee);

        }
        else if (shooting)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.shooting);
        }
        else if (preshooting)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.preshooting);
            preshooting = false;
            shooting = true;



        }

        else if (postshooting)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.postshooting);
            idle = true;
        }


    }

    [System.Serializable]
    public class AnimatorParameters
    {
        public static string estado = "estado";
    }

}