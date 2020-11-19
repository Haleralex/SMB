using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBehaviour : SceneLinkedSMB<EyeBehaviour>
{
    public override void OnSLStateNoTransitionUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnSLStateNoTransitionUpdate(animator, stateInfo, layerIndex);
        if(Input.GetKeyDown(KeyCode.K)){

            m_MonoBehaviour.StartState("HalfParts",0);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            m_MonoBehaviour.ReturnToBeginning();
        }
    }
}
