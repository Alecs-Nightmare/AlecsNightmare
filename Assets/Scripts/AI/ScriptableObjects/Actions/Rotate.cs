using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Actions/Rotate")]
    public class Rotate : Action
    {
        public float angle;

        public override void Act(FSMController fSMController)
        {
            fSMController.gameObject.transform.Rotate(Vector3.forward,angle);
        }
    }
}
