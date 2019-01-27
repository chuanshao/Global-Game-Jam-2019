using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yigui : MonoBehaviour
{
    public GameObject m_BtnController;

    private bool m_CanHide;
    private bool m_IsHide;

    private Player m_Player;

    private void Awake()
    {
        m_BtnController.SetActive(false);
        m_CanHide = false;
        m_IsHide = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_BtnController.SetActive(true);
            m_CanHide = true;
            m_Player = collision.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_BtnController.SetActive(false);
            m_CanHide = false;
        }
    }

    private void Update()
    {
        if (m_CanHide)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var sr = m_Player.GetComponent<SpriteRenderer>();
                if (!m_IsHide)
                {
                    sr.enabled = false;
                    m_IsHide = true;
                    m_Player.PlayerCurrenPropState = Player.PlayerPropState.InGuizi;
                }
                else
                {
                    m_IsHide = false;
                    sr.enabled = true;
                    m_Player.PlayerCurrenPropState = Player.PlayerPropState.Normal;
                }
            }
        }
    }



}
