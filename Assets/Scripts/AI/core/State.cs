using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    [CreateAssetMenu(menuName = "AI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transtions;
        [SerializeField]public Action[] onEnterActions;
        [SerializeField]public Action[] onExitActions;
        public Color stateColor;

        public void UpdateState(FSMController fSMController)
        {
            DoActions(fSMController,actions);
            CheckTranstions(fSMController);
        }

        public void OnExitState(FSMController fSMController)
        {
            DoActions(fSMController, onExitActions);
        }

        public void OnEnterState(FSMController fSMController)
        {
            DoActions(fSMController, onEnterActions);
        }

        private void DoActions(FSMController fSMController, Action[] actions)
        {
            if(actions != null)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    actions[i].Act(fSMController);
                }
            }
        }

        private void CheckTranstions(FSMController fSMController)
        {
            for (int i = 0; i < transtions.Length; i++)
            {
                bool decision = transtions[i].decision.Decide(fSMController);

                if (decision)
                    fSMController.TransitionToState(transtions[i].trueState);
                else
                    fSMController.TransitionToState(transtions[i].falseState);
            }
        }

    }
}

