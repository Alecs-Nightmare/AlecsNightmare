using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassweatherAnimationController : MonoBehaviour {
    private Animator anim;
    private EnemyMovement _enemyMovement;

    public bool followingPlayer;
    public bool ignoringPlayer;
    public bool dying;
    public bool alertingPlayer;

    // Use this for initialization
    private void Awake()
    {
        anim = GetComponent<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    void Start() {


    }

    enum EnemyState
    {
        idle = 0,
        alerting = 1,
        following = 2,
        dying = 3
    }
    // Update is called once per frame
    void Update() {

        if (followingPlayer)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.following);

        }
        else if (alertingPlayer)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.alerting);
            alertingPlayer = false;
            followingPlayer = true;
            
        }
        else if (dying)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.dying);
        }
        else if (ignoringPlayer)
        {
            anim.SetInteger(AnimatorParameters.estado, (int)EnemyState.idle);
            
        }

    }

    [System.Serializable]
    public class AnimatorParameters
    {
        public static string estado = "estado";
    }

}