using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{
    public abstract class Singleton<T> : ISingleton where T : Singleton<T> ,new() 
    {
        private static T sInstance;
        private static readonly object mLock = new object();

        /// <summary>
        /// 获取单例
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (mLock)
                {
                    if (sInstance == null)
                    {
                        sInstance = SingletonCreator.CreateSingleton<T>();
                    }
                }

                return sInstance;
            }
        }


        /// <summary>
        /// 单例初始化
        /// </summary>
        public virtual void OnSingletonInit()
        {
            
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public virtual void OnDispose()
        {
            sInstance = null;
        }
    }
}
