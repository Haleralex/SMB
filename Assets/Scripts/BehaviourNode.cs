using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class BehaviourNode : SceneLinkedSMB<StartStateBehaviour>
    {
        
        public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
            foreach(var e in Transitions)
            {
                if (Input.GetKeyDown(e._keyCode))
                {
                    m_MonoBehaviour.StartState(e._nameAimState);
                }
            }
        }
    }
}

