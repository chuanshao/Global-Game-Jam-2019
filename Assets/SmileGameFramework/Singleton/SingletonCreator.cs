using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame {

    /// <summary>
    /// 单例构造函数
    /// </summary>
    public static class SingletonCreator
    {
        /// <summary>
        /// 构建一个指定类型的单例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateSingleton<T>() where T : class,ISingleton,new()
        {
            var retInstance = new T();
            retInstance.OnSingletonInit();
            return retInstance;
        }

    }
}
