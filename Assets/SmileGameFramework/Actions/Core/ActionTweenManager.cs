/********************************************************************
	created:	2018/06/11  9:55
	filename: 	SGActionTween.cs
	author:		Chuanshao
	
	purpose:	SmileGame 补间动画管理器
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    /// <summary>
    /// 补间动画管理器
    /// </summary>
    public class ActionTweenManager : Singleton<ActionTweenManager>
    {
        const int _DefaultMaxTweeners = 100;
        const int _DefaultMaxSequences = 50;

        /// <summary>
        /// 是否正在loop循环更新状态
        /// </summary>
        static bool isUpdateLoop;
        //static Dictionary<uint, ActionTween> mActionTweens = new Dictionary<uint, ActionTween>(_DefaultMaxTweeners);
        static HashSet<ActionTween> mActionTweens = new HashSet<ActionTween>();
        static HashSet<ActionTween> mWillAddTweens = new HashSet<ActionTween>();
        static HashSet<ActionTween> mWillRemoveTweens = new HashSet<ActionTween>();
        static HashSet<ActionTween> mWillKillTweens = new HashSet<ActionTween>();

        #region private funs

        private void AddTweenToAction()
        {
            if (mWillAddTweens.Count > 0)
            {
                foreach (var item in mWillAddTweens)
                {
                    mActionTweens.Add(item);
                }
                mWillAddTweens.Clear();
            }
        }

        private void RemoveTweenFromAction()
        {
            if (mWillRemoveTweens.Count > 0)
            {
                foreach (var item in mWillRemoveTweens)
                {
                    mActionTweens.Remove(item);
                }
                mWillRemoveTweens.Clear();
            }
        }

        private void MakeForKilling(ActionTween t)
        {
            t.mKill = true;
            mWillKillTweens.Add(t);
        }

        private void KillActiveTween()
        {
            foreach (var t in mWillKillTweens)
            {
                mActionTweens.Remove(t);
                t.TryCallKill();
            }
            mWillKillTweens.Clear();
        }

        #endregion

        /// <summary>
        /// 添加动作进入缓存
        /// </summary>
        /// <param name="actionTween"></param>
        public void Add(ActionTween actionTween)
        {
            mWillAddTweens.Add(actionTween);
        }

        /// <summary>
        /// 删除缓存里的动作
        /// </summary>
        /// <param name="actionTween"></param>
        public void Remove(ActionTween actionTween)
        {
            mWillRemoveTweens.Add(actionTween);
        }

        //public void Kill(ActionTween actionTween)
        //{
        //    mWillKillTweens.Add(actionTween);
        //}

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime">更新时间</param>
        public void Update(float deltaTime)
        {
            isUpdateLoop = true;
            bool willKill = false;
            RemoveTweenFromAction();
            AddTweenToAction();
            foreach (var t in mActionTweens)
            {
                if (t == null) continue;
                if (t.mKill)
                {
                    willKill = true;
                    MakeForKilling(t);
                    continue;
                }
                if (!t.ActionActive)
                {
                    Remove(t);
                    continue;
                }

                bool isNeedKill = t.Update(deltaTime);

                if (isNeedKill)
                {
                    willKill = true;
                    MakeForKilling(t);
                }else if (!t.ActionActive)
                {
                    Remove(t);
                }
            }
            if (willKill)
            {
                KillActiveTween();
            }
            isUpdateLoop = false;
        }

        public override void OnSingletonInit()
        {
            base.OnSingletonInit();
            ActionComponent.Instance.Init(null); //初始化以默认方式
        }

        public override void OnDispose()
        {
            base.OnDispose();
        }

    }
}
