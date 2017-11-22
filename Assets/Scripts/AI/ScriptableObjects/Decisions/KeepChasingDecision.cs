using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decisions/Keep Chasing Decision")]
    public class KeepChasingDecision : Decision
    {
        public override bool Decide(FSMController fSM)
        {
            return true;
        }
    }

}
