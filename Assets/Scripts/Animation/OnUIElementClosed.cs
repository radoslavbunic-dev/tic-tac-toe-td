using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUIElementClosed : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IUIElement uiElement = animator.GetComponent<IUIElement>();
        if (uiElement != null)
        {
            uiElement.OnClosed();
        }
    }
}
