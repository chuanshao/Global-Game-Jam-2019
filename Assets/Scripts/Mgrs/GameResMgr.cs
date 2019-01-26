using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmileGame;

public class GameResMgr : MonoSingleton<GameResMgr>
{
    public List<LayerRes> m_LayerRes;

    /// <summary>
    /// 楼层资源
    /// </summary>
    [System.Serializable]
    public class LayerRes
    {
        public int m_LayerIndex;
        public Door m_Door;
        public List<GameObject> m_CamMovetoObjs;
    }

}
