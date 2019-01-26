using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmileGame.Action
{
    public class FadeAction : ActionTween
    {

        #region 共有字段

        public FadeType m_FadeType = FadeType.Mat;
        public float m_TarAlpha;

        #endregion

        #region 私有字段
        private float m_oriAlpha, m_tarAlpha;
        private bool mIsInit;
        private Material m_mat;
        private Light m_light;
        private Graphic m_UIGraphic;

        #endregion

        #region Enum

        public enum FadeType
        {
            Mat, //材质
            Light, //灯光
            UI,
        }

        #endregion

        #region private Funs

        private void InitColorObj()
        {
            switch (m_FadeType)
            {
                case FadeType.Mat:
                    if (m_mat == null)
                        m_mat = Target.GetComponent<Renderer>().material;
                    break;
                case FadeType.Light:
                    if (m_light == null)
                        m_light = Target.GetComponent<Light>();
                    break;
                case FadeType.UI:
                    if (m_UIGraphic == null)
                        m_UIGraphic = Target.GetComponent<Graphic>();
                    break;
                default:
                    break;
            }
        }

        private Color color
        {
            get
            {
                InitColorObj();
                Color col = Color.white;
                switch (m_FadeType)
                {
                    case FadeType.Mat:
                        col = m_mat.color;
                        break;
                    case FadeType.Light:
                        col = m_light.color;
                        break;
                    case FadeType.UI:
                        col = m_UIGraphic.color;
                        break;
                }
                return col;
            }
            set
            {
                InitColorObj();
                switch (m_FadeType)
                {
                    case FadeType.Mat:
                        m_mat.color = value;
                        break;
                    case FadeType.Light:
                        m_light.color = value;
                        break;
                    case FadeType.UI:
                        m_UIGraphic.color = value;
                        break;
                }
            }
        }

        #endregion

        #region ActionTween CallFun

        protected override float GetDuration(float speed)
        {
            float interpolation = Mathf.Abs(m_oriAlpha - m_tarAlpha);
            return interpolation * 255 / speed;
        }

        protected override bool InitAction()
        {
            if (mIsInit) return true;
            if (mActionDirectionType == ActionEnum.ActionDirectionType.To)
            {
                m_oriAlpha = color.a;
                m_tarAlpha = m_TarAlpha;
            }
            else
            {
                m_oriAlpha = m_TarAlpha;
                m_tarAlpha = color.a;
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
            float a = Mathf.Lerp(m_oriAlpha, m_tarAlpha, proc);
            var col = color;
            col.a = a;
            color = col;
        }
        #endregion

        #region Override Funs
        public override void Reset()
        {
            base.Reset();
            var col = color;
            col.a = m_oriAlpha;
            color = col;
        }
        #endregion
    }
}
