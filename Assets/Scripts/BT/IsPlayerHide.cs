using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class IsPlayerHide : Conditional
{
    public SharedGameObject playerGameObject;

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
                if (player.PlayerCurrenPropState == Player.PlayerPropState.InBox_Hide || player.PlayerCurrenPropState == Player.PlayerPropState.InGuizi)
                {
                    return TaskStatus.Failure;
                }

                return TaskStatus.Success;
            }
        }
    }

    public override void OnReset()
    {
        playerGameObject = null;
    }
}
