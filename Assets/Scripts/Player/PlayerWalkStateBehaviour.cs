using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkStateBehaviour : StateMachineBehaviour
{
    public EWalkDir m_Dir = EWalkDir.Right;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        var palyer = animator.transform;
        var scale = palyer.localScale;
        if (m_Dir == EWalkDir.Right)
        {
            scale.x = 1 * Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = -1 * Mathf.Abs(scale.x);
        }
        palyer.localScale = scale;
    }


    public enum EWalkDir
    {
        Right,
        Left,
    }

}
