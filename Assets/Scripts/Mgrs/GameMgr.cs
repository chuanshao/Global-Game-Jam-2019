using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmileGame;
using UnityEngine.SceneManagement;

/// <summary>
///  Game Mgr
/// </summary>
public class GameMgr :MonoSingleton<GameMgr>{

    /// <summary>
    /// 游戏结束 因为玩家不在房间发现不在房间里
    /// </summary>
    public void GameOverByFindPlayerIsNotInRoom()
    {
        SceneManager.LoadScene(4);
    }

}
