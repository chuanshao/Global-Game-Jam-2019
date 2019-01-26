using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    public class ScaleAction : ActionTween
    {
        #region 共有字段

        public Vector3 m_TargetScale;

        #endregion

        #region 私有字段
        private Vector3 m_oriScale, m_tarScale;
        private bool mIsInit;
        #endregion

        #region Par Fun

        private Vector3 GetWolrdScale()
        {
            var scale = Vector3.one;
            scale.x = m_TargetScale.x / Target.lossyScale.x;
            scale.y = m_TargetScale.y / Target.lossyScale.y;
            scale.z = m_TargetScale.z / Target.lossyScale.z;
            return scale;
        }

        #endregion

        #region ActionTween CallFun
        protected override float GetDuration(float speed)
        {
            float duration = Vector3.Distance(m_oriScale, m_tarScale) / speed;
            return duration;
        }

        protected override bool InitAction()
        {
            if (mIsInit) return true;
            if (mActionDirectionType == ActionEnum.ActionDirectionType.To)
            {
                m_oriScale = Target.localScale ;
                m_tarScale = mRelative ? m_TargetScale : GetWolrdScale();
            }
            else
            {
                m_oriScale = mRelative ? m_TargetScale : GetWolrdScale();
                m_tarScale = Target.localScale ;
            }

            mIsInit = true;
            return true;
        }

        protected override void OnFinish(float progress)
        {
            UpdateValue(progress, 0);
        }

        protected override void UpdateValue(float proc, float deltaTime)
        {
            Target.localScale = Vector3.Lerp(m_oriScale, m_tarScale, proc);
        }
        #endregion

        #region Override Funs

        public override void Reset()
        {
            base.Reset();
            Target.localScale = m_oriScale;
        }

        #endregion

    }
}
