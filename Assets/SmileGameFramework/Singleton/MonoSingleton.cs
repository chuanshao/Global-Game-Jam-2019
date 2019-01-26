using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{
    public abstract class MonoSingleton<T> : MonoBehaviour , ISingleton where T : MonoSingleton<T>
    {
        public bool m_DontDestroyOnLoad = true;
        private static T sInstance;
        private static readonly object mLock = new object();
        private static bool s_IsDestory = false;
        private bool m_IsInit = false;

        /// <summary>
        /// 单例是否已经销毁
        /// </summary>
        public static bool IsSingletonDestory
        {
            get
            {
                return s_IsDestory;
            }
        }

        /// <summary>
        /// 单例是否初始化
        /// </summary>
        public bool IsInit
        {
            get { return m_IsInit; }
            set { m_IsInit = value; }
        }

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
                        sInstance = FindObjectOfType<T>();
                        if (sInstance == null)
                        {
                            sInstance = MonoSingletonCreator.CreateSingleton<T>();
                            s_IsDestory = false;
                        }
                    }
                }

                return sInstance;
            }
        }

        public virtual void Awake()
        {
            if (m_DontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            if (sInstance == null)
            {
                s_IsDestory = false;
                sInstance = this as T;
                if (!IsInit)
                {
                    sInstance.OnSingletonInit();
                    IsInit = true;
                }
            }
            else
            {
                s_IsDestory = true;
                sInstance = null;
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 程序退出时，就不再使用单例
        /// </summary>
        public virtual void OnApplicationQuit()
        {
            s_IsDestory = true;
            sInstance = null;
        }

        /// <summary>
        /// 单例初始化回调函数
        /// </summary>
        public virtual void OnSingletonInit()
        {
            
        }

        /// <summary>
        /// 单例销毁时，回调函数 单例销毁请调用扩展方法 Dispose()
        /// </summary>
        public virtual void OnDispose()
        {
            s_IsDestory = true;
            sInstance = null;
            Destroy(gameObject);
        }
    }
}
