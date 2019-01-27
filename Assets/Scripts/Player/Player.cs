using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using SmileGame;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 速度
    /// </summary>
    public float m_Speed = 1;

    /// <summary>
    /// 初始化Player所在Layer
    /// </summary>
    public int m_InitLayer = 2;

    [ShowInInspector]
    private PlayerState m_PlayerState = PlayerState.Idle;
    private Animator m_PlayerAnimator;
    private Rigidbody2D m_PlayerRigidbody;
    private PlayerPropState m_PlayerPropState;

    public enum PlayerState
    {
        Idle,
        Walk_Right,
        Walk_Left,
    }

    /// <summary>
    /// Player 利用道具状态
    /// </summary>
    public enum PlayerPropState
    {
        Normal,
        InDoor,
        InBox,
        InBox_Hide,
        InGuizi,
    }

    public PlayerState PlayerCurrenState
    {
        get { return m_PlayerState; }
    }

    /// <summary>
    /// Player 目前 道具应用状态
    /// </summary>
    public PlayerPropState PlayerCurrenPropState
    {
        get { return m_PlayerPropState; }
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
        UpdateDoorTransfer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var door = collision.gameObject.GetComponent<Door>();
        if (door != null)
        {
            m_PlayerPropState = PlayerPropState.InDoor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var door = collision.gameObject.GetComponent<Door>();
        if (door != null)
        {
            m_PlayerPropState = PlayerPropState.Normal;
        }
    }



    #endregion

    #region private funs

    /// <summary>
    /// door 转移
    /// </summary>
    private void UpdateDoorTransfer()
    {
        if (m_PlayerPropState == PlayerPropState.InDoor)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                m_InitLayer = GetNextDoorIndex();
                MoveToDoorLayerIndex(m_InitLayer);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                m_InitLayer = GetUpDoorIndex();
                MoveToDoorLayerIndex(m_InitLayer);
            }
        }
    }

    private int GetNextDoorIndex()
    {
        int count = GameResMgr.Instance.m_LayerRes.Count;
        if (m_InitLayer - 1 >= 0)
        {
            return m_InitLayer = m_InitLayer - 1;
        }
        else
        {
            return m_InitLayer = count - 1;
        }
    }

    private int GetUpDoorIndex()
    {
        int count = GameResMgr.Instance.m_LayerRes.Count;
        if (m_InitLayer + 1 < count)
        {
            return m_InitLayer = m_InitLayer + 1;
        }
        else
        {
            return m_InitLayer = 0;
        }
    }

    private void MoveToDoorLayerIndex(int index)
    {
        var door = GameResMgr.Instance.m_LayerRes[index].m_Door;
        transform.position = door.transform.position;
    }

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
