using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class EnterDoorAction : Action
{
    public SharedGameObject doorGameObject;

    private Door m_Door;
    private Mama mama;

    public override void OnStart()
    {
        mama = Owner.GetComponent<Mama>();
        if (doorGameObject != null && doorGameObject.Value != null)
        {
            m_Door = doorGameObject.Value.GetComponent<Door>();
        }
    }

    public override TaskStatus OnUpdate()
    {
        Random.InitState ((int)Time.time);
        float r = Random.Range(-1, 1);
        if (r >= 0)
        {
            m_Door.OpenDoor();
            mama.DoEnterDoor();
            return TaskStatus.Success; //结束任务
        }
        else
        {
            return TaskStatus.Failure; //继续任务
        }
    }

    public override void OnReset()
    {
        doorGameObject = null;
        m_Door = null;
    }
}
