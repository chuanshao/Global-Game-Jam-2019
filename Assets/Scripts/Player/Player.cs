using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 速度
    /// </summary>
    public float m_Speed = 1;

    [ShowInInspector]
    private PlayerState m_PlayerState = PlayerState.Idle;
    private Animator m_PlayerAnimator;
    private Rigidbody2D m_PlayerRigidbody;

    public enum PlayerState
    {
        Idle,
        Walk_Right,
        Walk_Left,
        EnterDoor,
    }

    #region unity api 

    private void Awake()
    {
        m_PlayerAnimator = GetComponent<Animator>();
        m_PlayerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontal = GameInput.Horizontal; //控制左右移动 
        float vertical = GameInput.Vertical;//控制下蹲和进入门

        if (horizontal > 0)
        {
            m_PlayerState = PlayerState.Walk_Right;
        }
        else if (horizontal < 0)
        {
            m_PlayerState = PlayerState.Walk_Left;
        }
        else
        {
            m_PlayerState = PlayerState.Idle;
        }
        UpdatePlayerMove(m_PlayerState,horizontal);

        UpdateAnimatorParams();
    }

    #endregion

    #region private funs

    private void UpdatePlayerMove(PlayerState state, float moveDir)
    {
        var oldPos = m_PlayerRigidbody.position;
        oldPos.x += moveDir * m_Speed;
        m_PlayerRigidbody.MovePosition(oldPos);
    }

    /// <summary>
    ///   更新动画数据 
    /// </summary>
    private void UpdateAnimatorParams()
    {
        m_PlayerAnimator.SetBool("walk_left", m_PlayerState == PlayerState.Walk_Left);
        m_PlayerAnimator.SetBool("walk_right", m_PlayerState == PlayerState.Walk_Right);
    }

    #endregion
}
