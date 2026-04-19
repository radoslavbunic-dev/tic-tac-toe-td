using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUIBackgroundClosed : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIBackground uiWindow = animator.GetComponent<UIBackground>();
        if (uiWindow != null)
        {
            uiWindow.OnClosed();
        }
    }
}
