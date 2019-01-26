using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using SmileGame;

public class Mama : MonoBehaviour
{
    public List<Transform> m_Targets;
    private BehaviorTree m_MamaBT;
    private Transform m_CurrentTarget;
    private SpriteRenderer m_MamaSR;

    private void Awake()
    {
        m_MamaBT = GetComponent<BehaviorTree>();
        m_MamaSR = GetComponent<SpriteRenderer>();
        m_MamaBT.OnBehaviorEnd += DoBTEnd;
    }

    private void OnEnable()
    {
        RandomMovetoTarget();
    }

    private void RandomMovetoTarget()
    {
        m_CurrentTarget = RandomTraget(m_CurrentTarget);
        if (m_CurrentTarget != null)
        {
            SharedGameObject sgo = m_CurrentTarget.gameObject;
            m_MamaBT.SetVariable("MoveTo", sgo);
            TryFlipSR(m_CurrentTarget);
        }
    }

    private void TryFlipSR(Transform moveToTrans)
    {
        m_MamaSR.flipX = moveToTrans.position.x < transform.position.x;
    }

    private Transform RandomTraget(Transform currentTarget)
    {
        if (m_Targets != null && m_Targets.Count > 0)
        {
            int targetIndex = Random.Range(0, m_Targets.Count);
            if (m_Targets[targetIndex] == null)
            {
                currentTarget = RandomTraget(currentTarget);
            }
             else if (m_Targets[targetIndex] != currentTarget)
            {
                currentTarget = m_Targets[targetIndex];
            }
            else
            {
                currentTarget = RandomTraget(currentTarget);
            }
        }

        return currentTarget;
    }

    private void DoBTEnd(Behavior behavior)
    {
        LOG.Log("End");
        m_MamaBT.Start();
        RandomMovetoTarget();
    }

}
