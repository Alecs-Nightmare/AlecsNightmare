using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decisions/CheckIfBelowGround")]
    public class CheckIfIsBelowGround : Decision
    {
    
        public override bool Decide(FSMController fSM)
        {
            if (fSM.gameObject.transform.position.y < fSM.minHeight)
            {
                return true;
            }

            return false;
        }
    }
}
