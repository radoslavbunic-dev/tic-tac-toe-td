using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUIPopupClosed : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIPopup uiWindow = animator.GetComponent<UIPopup>();
        if (uiWindow != null)
        {
            uiWindow.OnClosed();
        }
    }
}
