/********************************************************************
	created:	2018/06/11  9:14
	filename: 	SGActionComponent.cs
	author:		Chuanshao
	
	purpose:	SmileGame Action 主要节点更新组件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{

    /// <summary>
    /// SmileGame Action 补间动画 Mono 组件，一般为自动挂载
    /// </summary>
    public class ActionComponent : MonoSingleton<ActionComponent>
    {
        #region 私有字段

        private float mUnScaleTime;
        private float mUnScalDeltaTime; 

        #endregion

        #region 属性

        /// <summary>
        /// 组件是否初始化
        /// </summary>
        public bool IsComponentInit
        {
            get;
            private set;
        }

        /// <summary>
        /// 配置设置
        /// </summary>
        public ActionConfig Config
        {
            get;
            set;
        }

        #endregion

        #region Unity API

        private void Start()
        {
            mUnScaleTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            mUnScalDeltaTime = Time.realtimeSinceStartup - mUnScaleTime;
            float time = 0;
            if (Config.IgnoreUnitySacleTime)
            {
                if (Config.UseSmoothTime && mUnScalDeltaTime > Config.MaxSmoothTime) mUnScalDeltaTime = Config.MaxSmoothTime;
                time = mUnScalDeltaTime * Config.ScaleTime;
            }
            else
            {
                time = Config.UseSmoothTime ? Time.smoothDeltaTime : Time.deltaTime * Config.ScaleTime;
            }
            ActionTweenManager.Instance.Update(time);
            mUnScaleTime = Time.realtimeSinceStartup;
        }

        #endregion

        #region Public API

        /// <summary>
        /// 初始化配置
        /// </summary>
        public void Init(ActionConfig config)
        {
            if (config != null)
            {
                Config = config;
                IsComponentInit = true;
            }
        }

        #endregion

        #region Override Callback

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            if (!IsComponentInit)
            {
                //还未进行初始化话，自我初始化开始
                ActionConfig config = ResourcesHelper.Load<ActionConfig>("ActionConfig");
                Init(config);
            }
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }

        #endregion

    }
}
