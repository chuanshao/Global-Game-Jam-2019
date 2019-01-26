/********************************************************************
	created:	2018/06/11  9:50
	filename: 	SGActionConfig.cs
	author:		Chuanshao
	
	purpose:	动画库配置文件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    //[CreateAssetMenu(menuName = "SmileGameActon/Create Config ")]
    public class ActionConfig : ScriptableObject
    {
        /// <summary>
        /// 时间缩放系数
        /// </summary>
        public float ScaleTime = 1;

        /// <summary>
        /// 是否忽略Unity的时间缩放
        /// </summary>
        public bool IgnoreUnitySacleTime = true;

        /// <summary>
        /// 是否使用平滑时间
        /// </summary>
        public bool UseSmoothTime = false;

        /// <summary>
        /// 平滑时间的最大值
        /// </summary>
        public float MaxSmoothTime = 0.15f;

        /// <summary>
        /// 是否使用安全模式
        /// </summary>
        public bool UseSafetyMode = true;

        public float period;

        public float overshootOrAmplitude;

    }
}
