using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectAI;

namespace ProjectAI
{
    public class FSMController : MonoBehaviour
    {
        public float detectionRadius;
        public LayerMask collisionMask;
        public int maxCollisions;
        public float maxHeight;
        public float minHeight;
        public float moveUpSpeed;
        public float moveDownSpeed;


        [SerializeField] State m_current;
        [SerializeField] State m_remainState;
        [SerializeField] State m_globalState;

        [SerializeField] bool m_isAIActive;
        [SerializeField] bool m_isEntityInRange;

        [SerializeField] Transform chosenTarget;
        [SerializeField] RaycastHit2D[] m_collisions;

        public State Current
        {
            get
            {
                return m_current;
            }

            set
            {
                m_current = value;
            }
        }

        public bool IsEntityInRange
        {
            get
            {
                return m_isEntityInRange;
            }
        }

        public RaycastHit2D[] Collisions
        {
            get
            {
                if (m_collisions == null)
                {
                    m_collisions = new RaycastHit2D[maxCollisions];
                }
                return m_collisions;
            }

            set
            {
                m_collisions = value;
            }
        }

        public Transform ChosenTarget
        {
            get
            {
                return chosenTarget;
            }

            set
            {
                chosenTarget = value;
            }
        }

        private void Awake()
        {
            m_isAIActive = true;
        }


        public void InitializeAI(bool init)
        {
            if (!init)
                m_isAIActive = true;
            else
                m_isAIActive = false;
        }

        public void TransitionToState(State newState)
        {
            if (m_remainState != newState)
            {
                Current.OnExitState(this);
                Current = newState;
                newState.OnEnterState(this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_isAIActive)
                return;

            if (m_globalState != null)
                m_globalState.UpdateState(this);

            Current.UpdateState(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Current.stateColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }

    }

}
