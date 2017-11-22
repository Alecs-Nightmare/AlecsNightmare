using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(fileName = "Chase", menuName = "AI/Actions/Chase")] 
    public class Chase : Action
    {
        public override void Act(FSMController fSMController)
        {
            Debug.Log("Chasing");
        }
    }

}
