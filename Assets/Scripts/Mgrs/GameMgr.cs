using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmileGame;
using UnityEngine.SceneManagement;

/// <summary>
///  Game Mgr
/// </summary>
public class GameMgr :MonoSingleton<GameMgr>{

    public GameObject m_GameMachine;

    /// <summary>
    /// 游戏结束 因为玩家不在房间发现不在房间里
    /// </summary>
    public void GameOverByFindPlayerIsNotInRoom()
    {
        SceneManager.LoadScene(4);
    }

    public void GameOverSuccess()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// 生成游戏机
    /// </summary>
    public void GenerationGameMachine(Transform parent)
    {
       GameObject go = Instantiate<GameObject>(m_GameMachine);
       go.transform.position = parent.position;
    }

    private void Start()
    {
        FindWhereGameBodyParent();
    }

    private void FindWhereGameBodyParent()
    {
        PlayerDestroy[] pds = FindObjectsOfType<PlayerDestroy>();
        int index = Random.Range(0, pds.Length);
        pds[index].m_HasGameMachine = true;
    }

}
