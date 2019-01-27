using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using SmileGame;

public class Mama : MonoBehaviour
{
    /// <summary>
    /// 妈妈初始化所在层数
    /// </summary>
    public int m_InitLayer = 0;
    public int m_MaxLayer = 2;
    public float m_MamaSpeed = 2;
    public MamaState mamaState = MamaState.Walking;
    public Player m_Player;
    private BehaviorTree m_MamaBT;
    private Transform m_CurrentTarget;
    private SpriteRenderer m_MamaSR;

    public static string Event_Destory_Obj = "Event_Destory_Obj";

    public enum MamaState
    {
        Walking,
        EnterDoor,
        FindPlayerNotInHisRoom,
    }

    private void Awake()
    {
        m_MamaBT = GetComponent<BehaviorTree>();
        m_MamaSR = GetComponent<SpriteRenderer>();
        m_MamaBT.OnBehaviorEnd += DoBTEnd;
    }


    private void Start()
    {
        RandomMovetoTarget();
        ChangedMamaSpeed();
    }

    private void OnEnable()
    {
        EventHelper.Listen(Event_Destory_Obj, AddSpeed);
    }

    private void OnDisable()
    {
        EventHelper.Remove(Event_Destory_Obj, AddSpeed);
    }



    private void Update()
    {

    }

    private void AddSpeed(object[] args)
    {
        if (args != null && args.Length > 0)
        {
            m_MamaSpeed += (float)args[0];
            ChangedMamaSpeed();
        }
    }

    public void ChangedMamaSpeed()
    {
        SharedFloat sf = m_MamaSpeed;
        m_MamaBT.SetVariable("Speed", sf);
    }


    private void RandomMovetoTarget()
    {
        if (GameResMgr.IsSingletonDestory) return;
        m_CurrentTarget = RandomTraget(m_CurrentTarget);
        if (m_CurrentTarget != null)
        {
            SharedGameObject sgoPlayer = m_Player.gameObject;
            m_MamaBT.SetVariable("SeePlayerObj", sgoPlayer);

            SharedGameObject sgo = m_CurrentTarget.gameObject;
            m_MamaBT.SetVariable("MoveTo", sgo);
            SharedFloat sf = Mathf.Sign(m_CurrentTarget.position.x - transform.position.x) * 90;
            m_MamaBT.SetVariable("AngleOff2D", sf);
            List<GameObject> canSeeObjs = new List<GameObject>();
            canSeeObjs.Add(GameResMgr.Instance.m_LayerRes[m_InitLayer].m_Door.gameObject);
            SharedGameObjectList sgl = canSeeObjs;
            m_MamaBT.SetVariable("SeeObjs", sgl);
            TryFlipSR(m_CurrentTarget);
        }
    }

    private void TryFlipSR(Transform moveToTrans)
    {
        m_MamaSR.flipX = moveToTrans.position.x < transform.position.x;
    }

    private Transform RandomTraget(Transform currentTarget)
    {
        GameResMgr.LayerRes res = GameResMgr.Instance.m_LayerRes[m_InitLayer];
        if (res != null && res.m_CamMovetoObjs != null && res.m_CamMovetoObjs.Count > 0)
        {
            int targetIndex = Random.Range(0, res.m_CamMovetoObjs.Count);
            if (res.m_CamMovetoObjs[targetIndex] == null)
            {
                currentTarget = RandomTraget(currentTarget);
            }
             else if (res.m_CamMovetoObjs[targetIndex] != currentTarget)
            {
                currentTarget = res.m_CamMovetoObjs[targetIndex].transform;
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
        if (mamaState == MamaState.Walking)
        {
            m_MamaBT.Start();
            RandomMovetoTarget();
        }
    }

    /// <summary>
    /// 随机楼层
    /// </summary>
    private int RandomLayer(int currentLayer)
    {
        var l = Random.Range(0, m_MaxLayer + 1);
        if (l != currentLayer)
        {
            return m_InitLayer = l;
        }
        else
        {
          return  RandomLayer(currentLayer);
        }
    }

    #region API

    public void DoEnterDoor()
    {
        mamaState = MamaState.EnterDoor;
        RandomLayer(m_InitLayer);
    }

    public void ToNextDoor(GameObject doorGameObj)
    {
        Door nextDoor = GameResMgr.Instance.m_LayerRes[m_InitLayer].m_Door;
        nextDoor.OpenDoor();
        transform.position = nextDoor.transform.position;
        mamaState = MamaState.Walking;

        if (doorGameObj != null)
        {
            var door = doorGameObj.GetComponent<Door>();
            door.CloseDoor();
        }

    }

    public void FindPlayerNotInHisRoom()
    {
        LOG.Log("不在房间 游戏结束");
        mamaState = MamaState.FindPlayerNotInHisRoom;
        GameMgr.Instance.GameOverByFindPlayerIsNotInRoom();
    }

    public void FindPlayerInHisRoom()
    {
        LOG.Log("在房间里，好好学习哈");
    }

    #endregion

}
