using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 动画挂载Mono基础组件
/// </summary>
namespace SmileGame.Action
{
    public class ActionTweenComponent : MonoBehaviour
    {
        #region 动画通用属性

        /// <summary>
        /// 持续时间
        /// </summary>
        public float m_Duration;

        /// <summary>
        /// 运动方向
        /// </summary>
        public ActionEnum.ActionDirectionType m_ActionDirection = ActionEnum.ActionDirectionType.To;

        /// <summary>
        /// 动画类型
        /// </summary>
        public ActionEnum.ActionType m_ActionType;

        /// <summary>
        /// 缓冲类型
        /// </summary>
        public EaseEnum m_EaseEnum = EaseEnum.Linear;

        /// <summary>
        /// 动画曲线
        /// </summary>
        public AnimationCurve m_ActionCurve = AnimationCurve.Linear(0, 0, 1, 1);

        /// <summary>
        /// 动画循环次数，-1 为无限次
        /// </summary>
        public int m_LoopTime;

        /// <summary>
        /// 独立的缩放时间系数
        /// </summary>
        public float m_ScaleTime = 1;

        /// <summary>
        /// 速度
        /// </summary>
        public float m_Speed;
        /// <summary>
        /// 是否使用速度初始，否则就根据时间初始化
        /// </summary>
        public bool m_UseSpeed;
        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool m_AutoPlay;
        /// <summary>
        /// 是否自动自杀
        /// </summary>
        public bool m_AutoKill;
        /// <summary>
        /// 是否是相对坐标系，如果为True ，使用本地坐标系,否则为世界坐标,继承者使用
        /// </summary>
        public bool m_Relative;

        #endregion

        #region 组件属性

        /// <summary>
        /// 动画类型
        /// </summary>
        public ActionTweenType m_AnimationType;

        /// <summary>
        /// 运行时可以重新配置动画属性
        /// </summary>
        public bool m_EnableEditorInRuntime;

        private ActionTween m_TargetActionTween;

        #region 通用动画回调事件

        public UnityEvent OnBeginPlayEvent;
        public UnityEvent OnCompleteEvent;
        public UnityEvent OnKillEvent;
        public UnityEvent OnLoopCompleteEvent;
        public UnityEvent OnStepUpdateEvent;

        #region Action is Enable

        public bool m_EnableBeginAction;
        public bool m_EnableCompleteAction;
        public bool m_EnableKillAction;
        public bool m_EnableLoopCompleteAction;
        public bool m_EnableStepUpdateAction;

        #endregion

        #endregion

        #endregion

        #region Move Action 

        public MoveAction.MoveToType m_MoveToType;
        public Vector3 m_MoveTargetPos;
        public Transform m_MoveTargetTransform;

        #endregion

        #region Rotation Action

        public RotationAction.RotToType m_RotToType = RotationAction.RotToType.Value;
        public Transform m_RotToTarget;
        public Vector3 m_RotToVec;
        public RotationAction.RotModel m_RotModel = RotationAction.RotModel.Fast;

        #endregion

        #region Scale Action

        public Vector3 m_ScaleTarget;

        #endregion

        #region Color Action

        public ColorAction.ColorType m_ColorType = ColorAction.ColorType.Mat;
        public Color m_TarCol;

        #endregion

        #region Fade Action

        public FadeAction.FadeType m_FadeType = FadeAction.FadeType.Mat;
        public float m_FadeTarAlpha;

        #endregion

        #region Unity API

        private void Awake()
        {
            m_TargetActionTween = CreateActionTween(m_AnimationType);
            InitActionTween(m_TargetActionTween);
        }

        private void OnDestroy()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Kill();
            }
            m_TargetActionTween = null;

        }



        #endregion

        #region Public Controller API

        public void Play()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Play();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void RePlay()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.RePlay();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void Revert()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Revert();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void Pause()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Pause();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void Resume()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Resume();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void Reset()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Reset();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void Kill()
        {
            if (m_TargetActionTween != null)
            {
                m_TargetActionTween.Kill();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public void ReInit()
        {
            if (m_TargetActionTween != null)
            {
                TryReInitActionTweenIfNeed();
            }
            else
            {
                LOG.LogError("ActionTween Must Statr ");
            }
        }

        public bool IsKill()
        {
            if (m_TargetActionTween != null)
            {
                return m_TargetActionTween.mKill;
            }
            else
            {
                return true;
            }
        }

        public ActionTween GetActionTween()
        {
            return m_TargetActionTween;
        }

        #endregion

        #region Private API

        private ActionTween CreateActionTween(ActionTweenType tweenType)
        {
            ActionTween resultAction = null;
            switch (tweenType)
            {
                case ActionTweenType.None:
                    break;
                case ActionTweenType.Move:
                    if (m_MoveToType == MoveAction.MoveToType.Target)
                    {
                        resultAction = transform.MoveTween(m_MoveTargetTransform, m_Duration);
                    }
                    else if (m_MoveToType == MoveAction.MoveToType.Pos)
                    {
                        resultAction = transform.MoveTween(m_MoveTargetPos, m_Duration);
                    }
                    break;
                case ActionTweenType.Rotation:
                    if (m_RotToType == RotationAction.RotToType.Target)
                    {
                        resultAction = transform.RotationTween(m_RotToTarget, m_Duration,m_RotModel);
                    }
                    else if (m_RotToType == RotationAction.RotToType.Value)
                    {
                        resultAction = transform.RotationTween(m_RotToVec, m_Duration, m_RotModel);
                    }
                    break;
                case ActionTweenType.Scale:
                    resultAction = transform.ScaleTween(m_ScaleTarget, m_Duration);
                    break;
                case ActionTweenType.Color:
                    resultAction = transform.ColorTween(m_TarCol,m_Duration,m_ColorType);
                    break;
                case ActionTweenType.Fade:
                    resultAction = transform.FadeTween(m_FadeTarAlpha, m_Duration, m_FadeType);
                    break;
                default:
                    break;
            }

           return  resultAction;
        }

        private void InitActionTween(ActionTween at)
        {
            if (at != null)
            {
                at.SetEase(m_EaseEnum)
                    .SetActionDirectionType(m_ActionDirection)
                    .SetActionScaleTime(m_ScaleTime)
                    .SetActionType(m_ActionType)
                    .SetAnimCurve(m_ActionCurve)
                    .SetLoopTime(m_LoopTime)
                    .SetSpeed(m_Speed)
                    .SetSpeedAble(m_UseSpeed)
                    .SetRelative(m_Relative)
                    .SetAutoPlay(m_AutoPlay)
                    .SetAutoKill(m_AutoKill);

                // Init BeginPlay Event
                if (m_EnableBeginAction)
                {
                    at.SetBeginPlayCallback(() =>
                    {
                        if (OnBeginPlayEvent != null)
                        {
                            OnBeginPlayEvent.Invoke();
                        }
                    });
                }
                // Init Complete Event
                if (m_EnableCompleteAction)
                {
                    at.SetCompleteCallback(() =>
                    {
                        if (OnCompleteEvent != null)
                        {
                            OnCompleteEvent.Invoke();
                        }
                    });
                }

                // Init Kill Event
                if (m_EnableKillAction)
                {
                    at.SetKillCallback(() =>
                    {
                        if (OnKillEvent != null)
                        {
                            OnKillEvent.Invoke();
                        }
                    });
                }

                // Init LoppComplete Event
                if (m_EnableLoopCompleteAction)
                {
                    at.SetLoopCompleteCallback(() =>
                    {
                        if (OnLoopCompleteEvent != null)
                        {
                            OnLoopCompleteEvent.Invoke();
                        }
                    });
                }

                // Init LoppComplete Event
                if (m_EnableStepUpdateAction)
                {
                    at.SetStepUpdateCallback(() =>
                    {
                        if (OnStepUpdateEvent != null)
                        {
                            OnStepUpdateEvent.Invoke();
                        }
                    });
                }

            }
        }

        private void TryReInitActionTweenIfNeed()
        {
            if (m_EnableEditorInRuntime)
            {
                InitActionTween(m_TargetActionTween);
            }
        }

        #endregion

    }
}
