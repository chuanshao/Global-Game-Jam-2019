/********************************************************************
	created:	2018/06/11  10:36
	filename: 	ActionTween.cs
	author:		Chuanshao
	
	purpose:	补间动画
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    /// <summary>
    /// 补间动画
    /// </summary>
    public abstract class ActionTween : IActionTween, IDisposable
    {
        #region 补间动画属性

        /// <summary>
        /// 是否激活
        /// </summary>
        private bool mActive;
        /// <summary>
        /// 是否杀死
        /// </summary>
        public bool mKill;
        /// <summary>
        /// 独立的缩放时间系数
        /// </summary>
        public float mScaleTime = 1;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float mDuration;
        /// <summary>
        /// 速度
        /// </summary>
        public float mSpeed;
        /// <summary>
        /// 是否使用速度初始，否则就根据时间初始化
        /// </summary>
        public bool mUseSpeed;
        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool mAutoPlay;
        /// <summary>
        /// 是否自动自杀
        /// </summary>
        public bool mAutoKill;
        /// <summary>
        /// 是否是相对坐标系，如果方式True ，使用本地坐标系,否则为世界坐标,继承者使用
        /// </summary>
        public bool mRelative;
        /// <summary>
        /// 动画循环次数，-1 为无限次
        /// </summary>
        public int mLoopTime;
        /// <summary>
        /// 动画类型
        /// </summary>
        public ActionEnum.ActionType mActionType;
        /// <summary>
        /// 缓冲类型
        /// </summary>
        public EaseEnum mEaseEnum = EaseEnum.Linear;
        /// <summary>
        /// 动画曲线
        /// </summary>
        public AnimationCurve mActionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        /// <summary>
        /// 运动方向,由继承者使用的属性
        /// </summary>
        public ActionEnum.ActionDirectionType mActionDirectionType = ActionEnum.ActionDirectionType.To;

        public float mOvershootOrAmplitude;
        public float mPeriod;

        #endregion

        #region 私有属性
        private uint _IdAction;
        /// <summary>
        /// 动画操作类型
        /// </summary>
        private ActionEnum.ActionOperateType mActionOperateType;
        /// <summary>
        /// 动画状态
        /// </summary>
        private ActionEnum.ActionState mActionState;

        /// <summary>
        /// 已经更新的时间
        /// </summary>
        private float mHasUpdateTime;

        /// <summary>
        /// 是否已经开始播放
        /// </summary>
        private bool mIsBeginPlay;
        /// <summary>
        /// 动画运行的Target
        /// </summary>
        private Transform mControllerTarget;
        /// <summary>
        /// 是否以速度初始化持续时间
        /// </summary>
        private bool mIsSpeedInit;
        /// <summary>
        /// 缓存已经Loop的次数
        /// </summary>
        private int mTempLoopTime;
        /// <summary>
        /// 自动播放是否已经执行
        /// </summary>
        private bool mIsAutoPlayInit;

        private EaseFunction mEaseFunction;
        private EaseCurve mEaseCurve; //自定义动画曲线

        #endregion

        #region 属性

        /// <summary>
        /// 动画执行目标对象
        /// </summary>
        public Transform Target
        {
            get { return mControllerTarget; }
            set { mControllerTarget = value; }
        }

        /// <summary>
        /// 目前动画状态
        /// </summary>
        public ActionEnum.ActionState currentActionState
        {
            get { return mActionState; }
        }

        public bool ActionActive
        {
            get
            {
                return mActive;
            }

            set
            {
                mActive = value;
                if (mActive)
                {
                    ActionTweenManager.Instance.Add(this);
                }
                else
                {
                    ActionTweenManager.Instance.Remove(this);
                }
            }
        }

        #endregion

        #region 回调函数
        /// <summary>
        /// 动画开始时Callback
        /// </summary>
        public ActionTweenCallback mOnBeginPlay;
        /// <summary>
        /// 动画完成时Callback
        /// </summary>
        public ActionTweenCallback mOnComplete;
        /// <summary>
        /// 动画每帧Callback
        /// </summary>
        public ActionTweenCallback mOnStepUpdate;
        /// <summary>
        /// 当loop次数>1时，每次循环完回调，最后一次循环完，不回调
        /// </summary>
        public ActionTweenCallback mOnLoopComplete;
        /// <summary>
        /// 被Kill时，回调
        /// </summary>
        public ActionTweenCallback mOnKill;

        #endregion

        #region construction

        public ActionTween()
        {
            _IdAction = ActionTweenIdBuilder.CreateNewActionId();
            mEaseFunction = DoEaseFunction;
            ActionConfig config = ActionComponent.Instance.Config;
            mOvershootOrAmplitude = config.overshootOrAmplitude;
            mPeriod = config.period;
            mActive = false;
            ActionTweenManager.Instance.Add(this);
        }

        ~ActionTween()
        {
        }

        public virtual void Dispose()
        {
            mActive = false;
            mKill = true;
            mOnBeginPlay = mOnComplete = mOnKill = mOnLoopComplete = null;
            mEaseFunction = null;
            mEaseCurve = null;
        }

        #endregion

        #region Public API

        public uint GetActionId()
        {
            return _IdAction;
        }

        public bool Update(float dt)
        {
            if (mAutoPlay && !mIsAutoPlayInit)
            {
                Play();
                mIsAutoPlayInit = true;
            }

            if (IsPlaying())
            {
                TryCallOnBeginPlay();
                if (mKill) return true; //在 beginPlay 回调中可能kill掉这个动画
                if (!InitAction()) 
                {
                    //初始化失败直接结束动画
                    TryCallOnComplete(mActionState == ActionEnum.ActionState.Action ? 1 : 0);
                    if (mKill) return true; //在 complete 回调中可能kill掉这个动画
                    return mAutoKill;
                }

                if (mActionState == ActionEnum.ActionState.Action)
                {
                    mHasUpdateTime += dt * mScaleTime;
                }
                else if (mActionState == ActionEnum.ActionState.Revert)
                {
                    mHasUpdateTime -= dt * mScaleTime;
                }

                float durtionTime = CalDurationTime();
                float proc = EaseManager.Evaluate(mEaseEnum, GetEaseFunc(), mHasUpdateTime, durtionTime, mOvershootOrAmplitude,mPeriod);
                UpdateValue(proc,dt);
                if (mActionState == ActionEnum.ActionState.Action)
                {
                    if (mHasUpdateTime >= durtionTime)
                    {
                        if (!ParseActionType(mActionType, mActionState))
                        {
                            TryCallOnComplete(1);
                            if (mKill) return true; //在 complete 回调中可能kill掉这个动画
                            return mAutoKill;
                        }
                        if (mKill) return true; //在loop 回调中可能kill掉这个动画
                    }
                }
                else if (mActionState == ActionEnum.ActionState.Revert)
                {
                    if (mHasUpdateTime <= 0.00001f)
                    {
                        if (!ParseActionType(mActionType, mActionState))
                        {
                            TryCallOnComplete(0);
                            if (mKill) return true; //在 complete 回调中可能kill掉这个动画
                            return mAutoKill;
                        }
                        if (mKill) return true; //在loop 回调中可能kill掉这个动画
                    }
                }
                TryCallOnStepUpdate();
                if (mKill) return true; //在 stepUpdate 回调中可能kill掉这个动画
            }
            return (mAutoKill && mActionState == ActionEnum.ActionState.Complete) || mKill;
        }

        /// <summary>
        /// 动画是否在播放状态
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            if (mActionOperateType == ActionEnum.ActionOperateType.Play)
            {
                if (mActionState == ActionEnum.ActionState.Action || mActionState == ActionEnum.ActionState.Revert)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取目前动画已经执行的时间
        /// </summary>
        /// <returns></returns>
        public float GetCurrentUpdateTime()
        {
            return mHasUpdateTime;
        }

        /// <summary>
        /// 应用操作符
        /// </summary>
        /// <param name="type"></param>
        public void ApplyOperate2Action(ActionEnum.ActionOperateType type)
        {
            mActionOperateType = type;
        }

        /// <summary>
        /// 重新计算持续时间，可以在回调中调用等
        /// </summary>
        /// <param name="speed"></param>
        public void ReCalDurationBySpeed(float speed)
        {
            if (speed <= 0) speed = 0.0001f;
            mDuration = GetDuration(speed);
        }

        /// <summary>
        /// 获取动画运行一次的持续时间 由序列动画调用
        /// </summary>
        /// <returns></returns>
        public float GetCalDurationTime()
        {
            if (mUseSpeed)
            {
                mDuration = GetDuration(mSpeed);
            }
            return mDuration;
        }

        /// <summary>
        /// 安全回调函数
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool OnTweenCallback(ActionTweenCallback callback)
        {
            if (ActionComponent.Instance.Config.UseSafetyMode)
            {
                try
                {
                    callback();
                }
                catch (Exception e)
                {
                    LOG.LogError("An error inside a tween callback was silently taken care of > " + e.Message + "\n\n" + e.StackTrace + "\n\n");
                    return false; // Callback error
                }
            }
            else callback();
            return true;
        }

        #endregion

        #region Private Funs

        /// <summary>
        /// 计算动画持续时间
        /// </summary>
        /// <returns></returns>
        private float CalDurationTime()
        {
            if (mUseSpeed && !mIsSpeedInit)
            {
                mDuration = GetDuration(mSpeed);
                mIsSpeedInit = true;
            }

            if (mActionType == ActionEnum.ActionType.Pingpang)
            {
                return mDuration / 2.0f;
            }

            return mDuration;
        }

        ///// <summary>
        ///// 计算速度 //TODO 没用的东西
        ///// </summary>
        ///// <returns></returns>
        //private float CalSpeed()
        //{
        //    if (mUseSpeed && mSpeed > 0)
        //    {
        //        return mSpeed;
        //    }
        //    else
        //    {
        //        if (!mIsSpeedInit)
        //        {
        //            mSpeed = mDuration == 0 ? 9999999.0f : (1.0f / mDuration);
        //            mIsSpeedInit = true;
        //        }
        //        return mSpeed;
        //    }
        //}

        /// <summary>
        /// 解析动画类型，分析动画按照何种方式运行,返回False，动画执行完毕，True则需要继续处理动画类型解析
        /// </summary>
        /// <param name="actionType"></param>
        /// <param name="actionState"></param>
        /// <returns></returns>
        private bool ParseActionType(ActionEnum.ActionType actionType, ActionEnum.ActionState actionState)
        {
            switch (actionType)
            {
                case ActionEnum.ActionType.Loop:
                    mHasUpdateTime = 0;
                    ++mTempLoopTime;
                    bool isLoopFinish = mLoopTime == -1 ? true : mTempLoopTime < mLoopTime;
                    if (isLoopFinish)
                    {
                        if (mOnLoopComplete != null) OnTweenCallback(mOnLoopComplete);
                    }
                    return isLoopFinish;
                case ActionEnum.ActionType.Pingpang:
                    if (actionState == ActionEnum.ActionState.Action)
                    {
                        ChangedActionState(ActionEnum.ActionState.Revert);
                        return true;
                    }
                    else if(actionState == ActionEnum.ActionState.Revert)
                    {
                        mHasUpdateTime = 0;
                        ++mTempLoopTime;
                        bool isPingpangLoopFinish = mLoopTime == -1 ? true : mTempLoopTime < mLoopTime;
                        if (isPingpangLoopFinish)
                        {
                            ChangedActionState(ActionEnum.ActionState.Action);
                            if (mOnLoopComplete != null) OnTweenCallback(mOnLoopComplete);
                        }
                        return isPingpangLoopFinish;
                    }
                    break;
                case ActionEnum.ActionType.None:
                default:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// 改变动态运行状态
        /// </summary>
        /// <param name="state"></param>
        private void ChangedActionState(ActionEnum.ActionState state)
        {
            mActionState = state;
        }

        /// <summary>
        /// 当动画开始的时候 尝试调用mOnBeginPlay回调函数
        /// </summary>
        private void TryCallOnBeginPlay()
        {
            if (!mIsBeginPlay)
            {
                if (mOnBeginPlay != null) OnTweenCallback(mOnBeginPlay);
                mIsBeginPlay = true;
            }
        }
        /// <summary>
        /// 动画执行的时候，每帧更新
        /// </summary>
        private void TryCallOnStepUpdate()
        {
            if (mOnStepUpdate != null)
            {
                OnTweenCallback(mOnStepUpdate);
            }
        }

        /// <summary>
        /// if toEnd is True , action is to end or to begin
        /// </summary>
        /// <param name="toEnd"></param>
        private void TryCallOnComplete(float progress)
        {
            ChangedActionState(ActionEnum.ActionState.Complete);
            OnFinish(progress);
            if (mOnComplete != null) OnTweenCallback(mOnComplete);
            mActive = false;
        }

        public void TryCallKill()
        {
            if (mOnKill != null) OnTweenCallback(mOnKill);
            Dispose();
        }

        /// <summary>
        /// 自定义曲线
        /// </summary>
        /// <param name="time"></param>
        /// <param name="duration"></param>
        /// <param name="overshootOrAmplitude"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        private float DoEaseFunction(float time, float duration, float overshootOrAmplitude, float period)
        {
            if (mEaseCurve == null)
            {
                mEaseCurve = new EaseCurve(mActionCurve);
            }
            return mEaseCurve.Evaluate(time, duration, overshootOrAmplitude, period);
        }

        #endregion

        #region protected Funss

        /// <summary>
        /// 获取缓冲自动函数
        /// </summary>
        /// <returns></returns>
        protected EaseFunction GetEaseFunc()
        {
            return mEaseFunction;
        }

        #endregion

        #region Action Controller API

        public virtual void Play(bool autoActive = true)
        {
            ActionActive = autoActive;
            ApplyOperate2Action(ActionEnum.ActionOperateType.Play);
            ChangedActionState(ActionEnum.ActionState.Action);
        }

        public virtual void RePlay(bool autoActive = true)
        {
            Reset();
            ActionActive = autoActive;
            ApplyOperate2Action(ActionEnum.ActionOperateType.Play);
            ChangedActionState(ActionEnum.ActionState.Action);
        }

        public virtual void Revert(bool autoActive = true)
        {
            ActionActive = autoActive;
            ApplyOperate2Action(ActionEnum.ActionOperateType.Play);
            ChangedActionState(ActionEnum.ActionState.Revert);
        }

        public virtual void Pause()
        {
            ApplyOperate2Action(ActionEnum.ActionOperateType.Pause);
        }

        public virtual void Resume()
        {
            ApplyOperate2Action(ActionEnum.ActionOperateType.Play);
        }

        public virtual void Reset()
        {
            mHasUpdateTime = 0;
            mTempLoopTime = 0;
            mIsBeginPlay = false;
            mIsSpeedInit = false;
            ApplyOperate2Action(ActionEnum.ActionOperateType.None);
            ChangedActionState(ActionEnum.ActionState.Idle);
        }

        /// <summary>
        /// 杀死这个动画
        /// </summary>
        public virtual void Kill()
        {
            mKill = true;
            if (!mActive)
            {
                TryCallKill();
            }
            ActionActive = false;
        }

        #endregion

        #region interface

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        protected abstract bool InitAction();

        /// <summary>
        /// 动画执行完成
        /// </summary>
        /// <param name="progress"></param>
        protected abstract void OnFinish(float progress);

        /// <summary>
        /// 进度更新 
        /// </summary>
        /// <param name="proc">其范围在0-1</param>
        protected abstract void UpdateValue(float proc,float deltaTime);

        /// <summary>
        /// 获取持续时间
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        protected abstract float GetDuration(float speed);

        #endregion

    }

}
