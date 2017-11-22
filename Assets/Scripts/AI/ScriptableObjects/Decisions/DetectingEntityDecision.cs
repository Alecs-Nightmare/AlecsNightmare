using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/Decision/Detecting Entity Decision")]
    public class DetectingEntityDecision : Decision
    {
        RaycastHit2D[] hits;

        public override bool Decide(FSMController fSMController)
        {
            hits = Physics2D.CircleCastAll(fSMController.transform.position, fSMController.detectionRadius, Vector2.zero);

            if (hits != null)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Player"))
                    {
                        fSMController.ChosenTarget = hits[i].transform;
                        Debug.Log("Detected");
                        return true;
                    }
                }
            }
            return false;
        }
    }

}
