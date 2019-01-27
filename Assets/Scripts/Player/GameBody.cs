using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBody : MonoBehaviour
{
    public GameObject m_BtnController;

    private bool m_CanPickup;

    private void Awake()
    {
        m_BtnController.SetActive(false);
        m_CanPickup = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_BtnController.SetActive(true);
            m_CanPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_BtnController.SetActive(false);
            m_CanPickup = false;
        }
    }

    private void Update()
    {
        if (m_CanPickup)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameMgr.Instance.GameOverSuccess();
            }
        }
    }




}
