/********************************************************************
	created:	2018/06/13  11:27
	filename: 	SequenceAction.cs
	author:		Chuanshao
	
	purpose:	序列动画 封闭类，不允许继承
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    /// <summary>
    /// 序列动画,使用duration来运行序列动画，不适用Speed
    /// </summary>
    public sealed class SequenceAction : ActionTween
    {
        #region 私有字段

        // 序列真实持续运行时间
        private float m_DurationFullTime;
        // 序列动画集合
        private readonly List<SequenceTweenProperty> m_SequenceTweens = new List<SequenceTweenProperty>();
        #endregion

        #region Inside Property

        /// <summary>
        /// 序列动画属性
        /// </summary>
        private struct SequenceTweenProperty
        {
            /// <summary>
            /// 序列动画开始时间
            /// </summary>
            public float m_BeginTime;
            /// <summary>
            /// 动画结束时间
            /// </summary>
            public float m_EndTime;
            ///// <summary>
            ///// 动画开始时候的百分比
            ///// </summary>
            //public float m_BeginPercent;
            /// <summary>
            /// 动画片段
            /// </summary>
            public ActionTween m_ActionTween;
            /// <summary>
            /// 是否已经Play
            /// </summary>
            public bool m_IsPlay;
        }

        #endregion

        #region construction

        public SequenceAction()
        {
            mActionDirectionType = ActionEnum.ActionDirectionType.To;
            mEaseEnum = EaseEnum.Linear;
            mActionType = ActionEnum.ActionType.None;
        }

        public override void Dispose()
        {
            m_SequenceTweens.Clear();
            base.Dispose();
        }

        #endregion

        #region Controller API

        public override void Reset()
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_IsPlay = false;
                stp.m_ActionTween.Reset();
                m_SequenceTweens[i] = stp;
                ActionTweenManager.Instance.Remove(stp.m_ActionTween);
            }
            base.Reset();
        }

        public override void Revert(bool autoActive = true)
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_IsPlay = false;
                //stp.m_ActionTween.Revert();
                m_SequenceTweens[i] = stp;
            }
            base.Revert(autoActive);
        }

        public override void Play(bool autoActive = true)
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_ActionTween.Play(stp.m_ActionTween.ActionActive);
                m_SequenceTweens[i] = stp;
            }
            base.Play(autoActive);
        }

        public override void RePlay(bool autoActive = true)
        {
            base.RePlay(autoActive);
        }

        public override void Pause()
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_ActionTween.Pause();
                m_SequenceTweens[i] = stp;
            }
            base.Pause();
        }

        public override void Resume()
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_ActionTween.Resume();
                m_SequenceTweens[i] = stp;
            }
            base.Resume();
        }

        public override void Kill()
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_ActionTween.Kill();
                m_SequenceTweens[i] = stp;
            }
            base.Kill();
        }

        #endregion

        #region Public API

        /// <summary>
        /// 添加动画Tween到序列指定时间点beginTimePos
        /// </summary>
        /// <param name="beginTimePos">时间点</param>
        /// <param name="tween">添加的动画</param>
        /// <returns></returns>
        public SequenceAction Add(float beginTimePos, ActionTween tween)
        {
            tween.mAutoKill = false;
            tween.mAutoPlay = false;
            ActionTweenManager.Instance.Remove(tween);
            SequenceTweenProperty stp;
            stp.m_IsPlay = false;
            stp.m_ActionTween = tween;
            stp.m_BeginTime = beginTimePos;
            stp.m_EndTime = beginTimePos + CalFullTimeOfTween(tween);
            //stp.m_BeginPercent = beginTimePos / mDuration;
            m_SequenceTweens.Add(stp);

            if (m_DurationFullTime < stp.m_EndTime)
            {
                m_DurationFullTime = stp.m_EndTime;
            }
            return this;
        }

        /// <summary>
        /// 添加动画在序列末尾
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        public SequenceAction Append(ActionTween tween)
        {
            return Add(m_DurationFullTime, tween);
        }

        #endregion

        #region private Funs

        /// <summary>
        /// 计算Tween的总共持续时间
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        private float CalFullTimeOfTween(ActionTween tween)
        {
            float durationTime = tween.GetCalDurationTime();
            if (tween.mActionType == ActionEnum.ActionType.Loop || tween.mActionType == ActionEnum.ActionType.Pingpang)
            {
                if (tween.mLoopTime == -1 || tween.mLoopTime == 0) return durationTime;
                else
                {
                    return durationTime * tween.mLoopTime;
                }
            }
           return durationTime;
        }

        #endregion

        #region ActionTween Funs

        protected override float GetDuration(float speed)
        {
            return m_DurationFullTime;
        }

        protected override bool InitAction()
        {
            mDuration = m_DurationFullTime;
            return true;
        }

        protected override void OnFinish(float progress)
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                stp.m_IsPlay = false;
                m_SequenceTweens[i] = stp;
            }
        }

        protected override void UpdateValue(float proc,float deltaTime)
        {
            for (int i = 0; i < m_SequenceTweens.Count; i++)
            {
                SequenceTweenProperty stp = m_SequenceTweens[i];
                ActionEnum.ActionState state = currentActionState;
                if (state == ActionEnum.ActionState.Action)
                {
                    if (GetCurrentUpdateTime() >= stp.m_BeginTime && GetCurrentUpdateTime() <= stp.m_EndTime)
                    {
                        if (!stp.m_IsPlay)
                        {
                            stp.m_IsPlay = true;
                            stp.m_ActionTween.ActionActive = true;
                            m_SequenceTweens[i] = stp;
                            stp.m_ActionTween.Play();
                        }
                    } else if (GetCurrentUpdateTime() > stp.m_EndTime || GetCurrentUpdateTime() < stp.m_BeginTime)
                    {
                        if (stp.m_IsPlay)
                        {
                            stp.m_IsPlay = false;
                            stp.m_ActionTween.ActionActive = false;
                            m_SequenceTweens[i] = stp;
                        }
                    }
                } else if (state == ActionEnum.ActionState.Revert)
                {
                    if (GetCurrentUpdateTime() >= stp.m_BeginTime && GetCurrentUpdateTime() <= stp.m_EndTime)
                    {
                        if (!stp.m_IsPlay)
                        {
                            stp.m_IsPlay = true;
                            stp.m_ActionTween.ActionActive = true;
                            m_SequenceTweens[i] = stp;
                            stp.m_ActionTween.Revert();
                        }

                    }
                    else if (GetCurrentUpdateTime() > stp.m_EndTime || GetCurrentUpdateTime() < stp.m_BeginTime)
                    {
                        if (stp.m_IsPlay)
                        {
                            stp.m_IsPlay = false;
                            stp.m_ActionTween.ActionActive = false;
                            m_SequenceTweens[i] = stp;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
