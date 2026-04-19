using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUIWindowClosed : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIWindow uiWindow = animator.GetComponent<UIWindow>();
        if (uiWindow != null)
        {
            uiWindow.OnClosed();
        }
    }
}
