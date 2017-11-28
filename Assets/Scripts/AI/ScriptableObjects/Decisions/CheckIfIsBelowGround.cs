using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decisions/CheckIfBelowGround")]
    public class CheckIfIsBelowGround : Decision
    {
        public float groundThreshold;

        public override bool Decide(FSMController fSM)
        {
            if (fSM.gameObject.transform.position.y < groundThreshold)
            {
                return true;
            }

            return false;
        }
    }


}
