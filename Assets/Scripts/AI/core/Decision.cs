using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(FSMController fSM);
    }
}