using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decisions/IsHeightMax")]
    public class CheckIfHeightIsMax : Decision
    {
        public float maxHeight;

        public override bool Decide(FSMController fSM)
        {
            Vector3 pos = fSM.gameObject.transform.position;

            if (pos.y >= maxHeight)
            {
                return true;
            }

            return false;
        }
    }


}
