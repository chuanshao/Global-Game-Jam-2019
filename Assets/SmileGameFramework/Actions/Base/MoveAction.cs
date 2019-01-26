using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    /// <summary>
    /// Move Action
    /// </summary>
    public class MoveAction : ActionTween
    {
        #region 共有字段

        public MoveToType mToType = MoveToType.Pos;
        public Transform mMoveToTarget;
        public Vector3 mTargetPos;

        #endregion

        #region 私有字段
        private Vector3 m_oriPos, m_tarPos;
        private bool mIsInit;
        #endregion

        #region Enum
        /// <summary>
        /// 目标类型
        /// </summary>
        public enum MoveToType
        {
            Pos, //目标点为坐标
            Target, //目标位置为Transform
        }

        #endregion

        #region ActionTween CallFun

        protected override void UpdateValue(float proc,float deltaTime)
        {
            if (mRelative)
            {
                 Target.localPosition = Vector3.Lerp(m_oriPos, m_tarPos, proc);
            }
            else
            {
                 Target.position = Vector3.Lerp(m_oriPos, m_tarPos, proc);
            }
        }

        protected override bool InitAction()
        {
            if (mIsInit) return true;
            if (mActionDirectionType == ActionEnum.ActionDirectionType.To)
            {
                m_oriPos = mRelative ? Target.localPosition : Target.position;
                m_tarPos = mToType == MoveToType.Pos ? mTargetPos : mRelative ? mMoveToTarget.localPosition : mMoveToTarget.position;
            }
            else
            {
                m_oriPos = mToType == MoveToType.Pos ? mTargetPos : mRelative ? mMoveToTarget.localPosition : mMoveToTarget.position;
                m_tarPos = mRelative ? Target.localPosition : Target.position;
            }

            mIsInit = true;
            return true;
        }

        protected override void OnFinish(float progress)
        {
            UpdateValue(progress,0);
        }

        protected override float GetDuration(float speed)
        {
            float duration = Vector3.Distance(m_oriPos, m_tarPos) / speed;
            return duration;
        }
        #endregion

        #region Override Funs

        public override void Reset()
        {
            base.Reset();
            if (mRelative) Target.localPosition = m_oriPos;
            else Target.position = m_oriPos;
        }

        #endregion
    }
}
