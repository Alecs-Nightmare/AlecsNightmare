using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Actions/MoveUp")]
    public class MoveUp : Action
    {
        public float moveSpeed;

        public override void Act(FSMController fSMController)
        {
            Vector3 goPosition = fSMController.gameObject.transform.position;
            float deltaMovementY = moveSpeed * Time.deltaTime;

            fSMController.transform.position = new Vector3(goPosition.x,deltaMovementY + goPosition.y,goPosition.z);
        }
    }
}

