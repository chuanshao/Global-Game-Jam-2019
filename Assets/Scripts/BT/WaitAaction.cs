using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using TooltipAttribute = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

public class WaitAaction : Action
{
    [Tooltip("The amount of time to wait")]
    public SharedFloat waitTime = 1;
    [Tooltip("Should the wait be randomized?")]
    public SharedBool randomWait = false;
    [Tooltip("The minimum wait time if random wait is enabled")]
    public SharedFloat randomWaitMin = 1;
    [Tooltip("The maximum wait time if random wait is enabled")]
    public SharedFloat randomWaitMax = 1;

    public SharedString animatorParam;

    // The time to wait
    private float waitDuration;
    // The time that the task started to wait.
    private float startTime;
    // Remember the time that the task is paused so the time paused doesn't contribute to the wait time.
    private float pauseTime;

    private Animator m_QipaoAnimator;

    public override void OnStart()
    {
        m_QipaoAnimator = Owner.GetComponentsInChildren<Animator>()[1];

        // Remember the start time.
        startTime = Time.time;
        if (randomWait.Value)
        {
            waitDuration = Random.Range(randomWaitMin.Value, randomWaitMax.Value);
        }
        else
        {
            waitDuration = waitTime.Value;
        }
    }

    public override TaskStatus OnUpdate()
    {
        // The task is done waiting if the time waitDuration has elapsed since the task was started.
        if (startTime + waitDuration < Time.time)
        {
            if (m_QipaoAnimator != null)
            {
                m_QipaoAnimator.SetBool(animatorParam.Value, false);
            }
            return TaskStatus.Success;
        }
        if (m_QipaoAnimator != null)
        {
            m_QipaoAnimator.SetBool(animatorParam.Value, true);
        }
        // Otherwise we are still waiting.
        return TaskStatus.Running;
    }

    public override void OnPause(bool paused)
    {
        if (paused)
        {
            // Remember the time that the behavior was paused.
            pauseTime = Time.time;
        }
        else
        {
            // Add the difference between Time.time and pauseTime to figure out a new start time.
            startTime += (Time.time - pauseTime);
        }
    }

    public override void OnReset()
    {
        // Reset the public properties back to their original values
        waitTime = 1;
        randomWait = false;
        randomWaitMin = 1;
        randomWaitMax = 1;
    }
}
