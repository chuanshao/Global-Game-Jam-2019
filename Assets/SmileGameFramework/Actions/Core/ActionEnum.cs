/********************************************************************
	created:	2018/06/11  10:45
	filename: 	ActionEnum.cs
	author:		Chuanshao
	
	purpose:	SmileGame Action 枚举类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{

    public class ActionEnum
    {

        /// <summary>
        /// 操作类型
        /// </summary>
        public enum ActionOperateType
        {
            None,
            Play,  //播放
            Pause, //暂停
            RePlay, //重新播放，重设后自动转换到Play
            Revert, //倒着播放，重设后自动转换到Play
            Stop, // 停止
        }

        /// <summary>
        /// 动画状态
        /// </summary>
        public enum ActionState
        {
            Idle, //待机
            Action, //激活阶段
            Revert, //倒着执行阶段
            Complete, //动画执行完成
        }

        /// <summary>
        /// 动画类型
        /// </summary>
        public enum ActionType
        {
            None,
            Loop,
            Pingpang,
        }

        /// <summary>
        /// 动画方向类型
        /// </summary>
        public enum ActionDirectionType
        {
            To, // 由自己到目标
            From, //由目标到自己
        }

    }
}
