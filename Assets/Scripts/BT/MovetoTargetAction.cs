using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class MovetoTargetAction : Action
{
    public SharedGameObject movetoGameObject;
    public SharedFloat waitTime = 1;

    private Animator animator;
    private Animator m_QipaoAnimator;
    private PropState propState;

    private float startTime;

    public override void OnStart()
    {
        startTime = Time.time;
        animator = Owner.GetComponent<Animator>();
        m_QipaoAnimator = Owner.GetComponentsInChildren<Animator>()[1];
        propState = movetoGameObject.Value.GetComponent<PropState>();
    }

    public override TaskStatus OnUpdate()
    {
        if (startTime + CalTime() < Time.time)
        {
            SetParam("walking", true);
            SetParam("set", false);
            SetQipaoParam("wenhao", false);
            SetQipaoParam("huiluan", false);
            return TaskStatus.Success;
        }
        else
        {
            TryDoActionWithProp();
            return TaskStatus.Running;
        }
    }

    private float CalTime()
    {
        switch (propState.eProp)
        {
            case PropState.EProp.None:
                return 0;
            case PropState.EProp.Light:
                return 1;
            case PropState.EProp.Shafa:
                return 5;
            case PropState.EProp.Zhanyifu:
                return 1;
            case PropState.EProp.Huaping:
                return 1;
            default:
                break;
        }
        return waitTime.Value;
    }

    private void TryDoActionWithProp()
    {
        switch (propState.eProp)
        {
            case PropState.EProp.None:
                break;
            case PropState.EProp.Shafa:
                SetParam("set",true);
                break;
            case PropState.EProp.Zhanyifu:
                break;
            case PropState.EProp.yigui:
            case PropState.EProp.jingzi:
            case PropState.EProp.bingxiang:
                SetQipaoParam("huiluan", true);
                SetParam("walking", false);
                break;
            case PropState.EProp.Light:
            case PropState.EProp.Huaping:
                SetQipaoParam("wenhao", true);
                SetParam("walking", false);
                break;
            default:
                break;
        }
    }

    private void SetParam(string param, bool isBo)
    {
        if (animator != null) animator.SetBool(param, isBo);
    }

    private void SetQipaoParam(string param, bool isBo)
    {
        if (m_QipaoAnimator != null) m_QipaoAnimator.SetBool(param, isBo);
    }
}
