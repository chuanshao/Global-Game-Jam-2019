using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{
    /// <summary>
    /// 单例接口
    /// </summary>
    public interface ISingleton
    {
        /// <summary>
        /// 单例初始化
        /// </summary>
        void OnSingletonInit();

        /// <summary>
        /// 单例销毁
        /// </summary>
        void OnDispose();

    }
}
