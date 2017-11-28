using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Actions/MoveDown")]
    public class MoveDown : Action
    {
        public float moveSpeed;

        public override void Act(FSMController fSMController)
        {
            fSMController.gameObject.transform.Translate(new Vector3(0,moveSpeed * Time.deltaTime,0),Space.World);
        }
    }


}
