using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsDoor : Conditional
{
    public SharedGameObject targetGameObject;
    public SharedGameObject targetDoor;

    public override TaskStatus OnUpdate()
    {
        if (targetGameObject == null || targetGameObject.Value == null)
            return TaskStatus.Failure;
        else
        {
           var door = targetGameObject.Value.GetComponent<Door>();
            if (door == null) return TaskStatus.Failure;
            else
            {
                targetDoor.Value = door.gameObject;
                return TaskStatus.Success;
            }
        }
    }

    public override void OnReset()
    {
        targetGameObject = null;
    }

}
