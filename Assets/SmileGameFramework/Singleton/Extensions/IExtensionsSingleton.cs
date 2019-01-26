using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame {
    /// <summary>
    /// 单例扩展接口
    /// </summary>
    public static class IExtensionsSingleton {

        /// <summary>
        /// 销毁单例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="singleton"></param>
        public static void Dispose(this ISingleton singleton)
        {
            if (singleton != null)
            {
                singleton.OnDispose();
            }
        }

    }
}
