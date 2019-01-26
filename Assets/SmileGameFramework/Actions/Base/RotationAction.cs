using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{

    /// <summary>
    /// Rotation Action
    /// </summary>
    public class RotationAction : ActionTween
    {
        #region 共有字段

        public RotToType mToType = RotToType.Value;
        public Transform mRotToTarget;
        public Vector3 mTargetRot;
        public RotModel mRotModle = RotModel.Fast;

        #endregion

        #region 私有字段
        private Vector3 m_oriRot, m_tarRot;
        private Quaternion m_oriQ, m_tarQ;
        private Vector3 m_ChangeVal;
        private bool mIsInit;

        #endregion

        #region Enum
        /// <summary>
        /// 目标类型
        /// </summary>
        public enum RotToType
        {
            Value, //目标点为值
            Target, //目标位置为Transform
        }

        /// <summary>
        /// 旋转模式
        /// </summary>
        public enum RotModel
        {
            Fast, //最小夹角旋转
            Fast360, //最大夹角旋转
        }

        #endregion

        #region Public API

        /// <summary>
        /// 设置旋转模式
        /// </summary>
        /// <param name="modle"></param>
        /// <returns></returns>
        public RotationAction SetRotModle(RotModel modle)
        {
            mRotModle = modle;
            return this;
        }

        #endregion

        #region ActionTween CallFun

        protected override float GetDuration(float speed)
        {
            float duration = 1;
            if (mRotModle == RotModel.Fast)
            {
                duration = Quaternion.Angle(Quaternion.Euler(m_oriRot), Quaternion.Euler(m_tarRot)) / speed;
            }
            else
            {
                Vector3 v = m_ChangeVal / speed;
                duration = Mathf.Max(v.x, v.y, v.z);
            }
            return duration;
        }

        protected override bool InitAction()
        {
            if (mIsInit) return true;
            if (mActionDirectionType == ActionEnum.ActionDirectionType.To)
            {
                m_oriRot = mRelative ? Target.localEulerAngles : Target.eulerAngles;
                m_tarRot = mToType == RotToType.Value ? mTargetRot : mRelative ? mRotToTarget.localEulerAngles : mRotToTarget.eulerAngles;
            }
            else
            {
                m_oriRot = mToType == RotToType.Value ? mTargetRot : mRelative ? mRotToTarget.localEulerAngles : mRotToTarget.eulerAngles;
                m_tarRot = mRelative ? Target.localEulerAngles : Target.eulerAngles;
            }
            m_ChangeVal = m_tarRot - m_oriRot;
            m_oriQ = Quaternion.Euler(m_oriRot);
            m_tarQ = Quaternion.Euler(m_tarRot);
            mIsInit = true;
            return true;
        }

        protected override void OnFinish(float progress)
        {
            UpdateValue(progress,0);
        }

        protected override void UpdateValue(float proc,float deltaTime)
        {
            switch (mRotModle)
            {
                case RotModel.Fast360:
                    Quaternion rot = Quaternion.Euler(m_ChangeVal.x * proc, m_ChangeVal.y * proc, m_ChangeVal.z * proc);
                    if (mRelative)
                    {
                        Target.localRotation = rot;
                    }
                    else
                    {
                        Target.rotation = rot;
                    }
                    break;
                case RotModel.Fast:
                default:
                    if (mRelative)
                    {
                        Target.localRotation = Quaternion.Lerp(m_oriQ, m_tarQ , proc);
                    }
                    else
                    {
                        Target.rotation = Quaternion.Lerp(m_oriQ, m_tarQ, proc);
                    }
                    break;
            }

        }

        #endregion

        #region Override Funs

        public override void Reset()
        {
            base.Reset();
            if (mRelative) Target.localEulerAngles = m_oriRot;
            else Target.eulerAngles = m_oriRot;
        }

        #endregion
    }
}
