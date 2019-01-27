using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmileGame;

public class PlayerDestroy : MonoBehaviour
{
    public Sprite sprite_Normal;
    public Sprite sprite_Destroy;

    public GameObject m_BtnController;
    public float m_AddSpeedToMama = 0.2f;

    private EState currentState = EState.Normal;
    private SpriteRenderer m_SR;
    private bool m_CanDestroy;

    public enum EState
    {
        Normal,
        Destroy,
    }

    private void Awake()
    {
        m_SR = GetComponent<SpriteRenderer>();
        ChangedState(EState.Normal);
        m_BtnController.SetActive(false);
        m_CanDestroy = false;
    }

    public void ChangedState(EState s)
    {
        currentState = s;
        switch (s)
        {
            case EState.Normal:
                SetSripte(sprite_Normal);
                break;
            case EState.Destroy:
                SetSripte(sprite_Destroy);
                break;
            default:
                break;
        }
    }

    private void SetSripte(Sprite s)
    {
        m_SR.sprite = s;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag) && currentState != EState.Destroy)
        {
            m_BtnController.SetActive(true);
            m_CanDestroy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_BtnController.SetActive(false);
            m_CanDestroy = false;
        }
    }

    private void Update()
    {
        if (m_CanDestroy && currentState != EState.Destroy)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DestroyProp();
            }
        }
    }


    /// <summary>
    ///  破坏道具
    /// </summary>
    public void DestroyProp()
    {
        ChangedState(EState.Destroy);
        m_BtnController.SetActive(false);
        m_CanDestroy = false;
        EventHelper.Send(Mama.Event_Destory_Obj, m_AddSpeedToMama);
    }


}
