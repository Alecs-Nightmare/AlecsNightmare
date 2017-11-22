using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Actions/Attack")]
    public class Attack : Action
    {
        public override void Act(FSMController fSM)
        {
            //fSMController.animator.SetBool("detected", true);
            fSM.Material.color = Color.red;
            fSM.Current.stateColor = Color.red;
        }
    }

}
