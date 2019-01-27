using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorState m_DoorState = DoorState.Close;
    public Sprite m_DoorClosed_S;
    public Sprite m_DoorOpen_S;

    public GameObject m_TipGameObj;

    private SpriteRenderer m_Door_SR;
    

    public enum DoorState
    {
        Open,
        Close,
    }

    private void Awake()
    {
        m_Door_SR = GetComponent<SpriteRenderer>();
        m_TipGameObj.SetActive(false);
    }

    public void OpenDoor()
    {
        m_Door_SR.sprite = m_DoorOpen_S;
        m_DoorState = DoorState.Open;
    }

    public void CloseDoor()
    {
        m_Door_SR.sprite = m_DoorClosed_S;
        m_DoorState = DoorState.Close;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_TipGameObj.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ("Player".Equals(collision.gameObject.tag))
        {
            m_TipGameObj.SetActive(false);
        }
    }

}
