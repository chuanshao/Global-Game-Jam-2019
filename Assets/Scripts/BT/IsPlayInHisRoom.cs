using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsPlayInHisRoom : Conditional
{
    public SharedGameObject playerGameObject;
    public SharedVector2 playerRoom;

    public override TaskStatus OnUpdate()
    {
        if (playerGameObject == null || playerGameObject.Value == null)
            return TaskStatus.Failure;
        else
        {
            var player = playerGameObject.Value.GetComponent<Player>();
            if (player == null) return TaskStatus.Failure;
            else
            {
                var pos = player.transform.position;
                var roomPos = playerRoom.Value;
                if (pos.x >= roomPos.x && pos.y >= roomPos.y)
                {
                    return TaskStatus.Success;
                }
                return TaskStatus.Failure;
            }
        }
    }

    public override void OnReset()
    {
        playerGameObject = null;
        playerRoom = null;
    }
}
