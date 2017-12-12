using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decisions/IsHeightMax")]
    public class CheckIfHeightIsMax : Decision
    {
        public override bool Decide(FSMController fSM)
        {
            Vector3 pos = fSM.gameObject.transform.position;

            if (pos.y >= fSM.maxHeight)
            {
                return true;
            }

            return false;
        }
    }


}
