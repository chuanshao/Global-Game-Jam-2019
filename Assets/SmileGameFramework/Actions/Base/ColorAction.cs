using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmileGame.Action
{
    public class ColorAction : ActionTween
    {
        #region 共有字段
       
        public ColorType m_ColorType = ColorType.Mat;
        public Color m_TarCol;

        #endregion

        #region 私有字段
        private Color m_oriColor, m_tarColor;
        private bool mIsInit;
        private Material m_mat;
        private Light m_light;
        private Graphic m_UIGraphic;

        #endregion

        #region Enum

        public enum ColorType
        {
            Mat, //材质
            Light, //灯光
            UI,
        }

        #endregion

        #region private Funs

        private void InitColorObj()
        {
            switch (m_ColorType)
            {
                case ColorType.Mat:
                    if (m_mat == null)
                        m_mat = Target.GetComponent<Renderer>().material;
                    break;
                case ColorType.Light:
                    if (m_light == null)
                        m_light = Target.GetComponent<Light>();
                    break;
                case ColorType.UI:
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
                switch (m_ColorType)
                {
                    case ColorType.Mat:
                        col = m_mat.color;
                        break;
                    case ColorType.Light:
                        col = m_light.color;
                        break;
                    case ColorType.UI:
                        col = m_UIGraphic.color;
                        break;
                }
                return col;
            }
            set
            {
                InitColorObj();
                switch (m_ColorType)
                {
                    case ColorType.Mat:
                        m_mat.color = value;
                        break;
                    case ColorType.Light:
                        m_light.color = value;
                        break;
                    case ColorType.UI:
                        m_UIGraphic.color = value;
                        break;
                }
            }
        }

        #endregion

        #region ActionTween CallFun

        protected override float GetDuration(float speed)
        {
           Color interpolation = m_oriColor - m_tarColor;
           return (interpolation.r + interpolation.g + interpolation.b + interpolation.a) * 255/( 4 * speed);
        }

        protected override bool InitAction()
        {
            if (mIsInit) return true;
            if (mActionDirectionType == ActionEnum.ActionDirectionType.To)
            {
                m_oriColor = color;
                m_tarColor = m_TarCol;
            }
            else
            {
                m_oriColor = m_TarCol;
                m_tarColor = color;
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
           color = Color.Lerp(m_oriColor,m_tarColor,proc);
        }
        #endregion

        #region Override Funs
        public override void Reset()
        {
            base.Reset();
            color = m_oriColor;
        }
        #endregion
    }
}
