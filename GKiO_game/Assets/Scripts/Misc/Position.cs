using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Corrects the position of the GameObject after animation.
/// </summary>
public class Position : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.gameObject.transform.position = animator.transform.position;
    }
}
