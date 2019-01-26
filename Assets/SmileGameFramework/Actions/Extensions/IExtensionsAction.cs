/********************************************************************
	created:	2018/06/12  2:12 
	file base:	IExtensionsAction.cs	
	author:		chuanshao
	
	purpose:	动作扩展类
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    public static class IExtensionsAction
    {

        #region Transform Extensions

        /// <summary>
        /// 移动到指定点
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetPos"></param>
        /// <param name="mDuration"></param>
        /// <returns></returns>
        public static ActionTween MoveTween(this Transform target,Vector3 targetPos,float mDuration)
        {
            var move = new MoveAction();
            move.mActionType = ActionEnum.ActionType.None;
            move.mToType = MoveAction.MoveToType.Pos;
            move.mTargetPos = targetPos;
            move.Target = target;
            move.mDuration = mDuration;
            move.mEaseEnum = EaseEnum.Linear;
            return move;
        }

        /// <summary>
        /// 移动到指定物体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetTran"></param>
        /// <param name="mDuration"></param>
        /// <returns></returns>
        public static ActionTween MoveTween(this Transform target, Transform targetTran, float duration)
        {
            var move = new MoveAction();
            move.mActionType = ActionEnum.ActionType.None;
            move.mToType = MoveAction.MoveToType.Target;
            move.mMoveToTarget = targetTran;
            move.Target = target;
            move.mDuration = duration;
            return move;
        }

        /// <summary>
        /// 选中物体到指定角度
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetRot"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static ActionTween RotationTween(this Transform target, Vector3 targetRot, float duration, RotationAction.RotModel modle = RotationAction.RotModel.Fast)
        {
            var rotAction = new RotationAction();
            rotAction.SetRotModle(modle);
            rotAction.mActionType = ActionEnum.ActionType.None;
            rotAction.mToType = RotationAction.RotToType.Value;
            rotAction.mTargetRot = targetRot;
            rotAction.Target = target;
            rotAction.mDuration = duration;
            return rotAction;
        }

        /// <summary>
        /// 选中物体到指定物体
        /// </summary>
        /// <param name="target"></param>
        /// <param name="targetRot"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static ActionTween RotationTween(this Transform target, Transform targetTran, float duration, RotationAction.RotModel modle = RotationAction.RotModel.Fast)
        {
            var rotAction = new RotationAction();
            rotAction.SetRotModle(modle);
            rotAction.mActionType = ActionEnum.ActionType.None;
            rotAction.mToType = RotationAction.RotToType.Target;
            rotAction.mRotToTarget = targetTran;
            rotAction.Target = target;
            rotAction.mDuration = duration;
            return rotAction;
        }

        public static ActionTween ScaleTween(this Transform target, Vector3 targetScale, float duration)
        {
            var scaleAction = new ScaleAction();
            scaleAction.mActionType = ActionEnum.ActionType.None;
            scaleAction.Target = target;
            scaleAction.m_TargetScale = targetScale;
            scaleAction.mDuration = duration;
            return scaleAction;
        }

        public static ActionTween ColorTween(this Transform target,Color targetCol , float duration,ColorAction.ColorType colorType = ColorAction.ColorType.Mat)
        {
            var colAction = new ColorAction();
            colAction.mActionType = ActionEnum.ActionType.None;
            colAction.Target = target;
            colAction.mDuration = duration;
            colAction.m_TarCol = targetCol;
            colAction.m_ColorType = colorType;
            return colAction;
        }

        public static ActionTween FadeTween(this Transform target, float targetFade, float duration, FadeAction.FadeType fadeType = FadeAction.FadeType.Mat)
        {
            var fadeAction = new FadeAction();
            fadeAction.mActionType = ActionEnum.ActionType.None;
            fadeAction.Target = target;
            fadeAction.mDuration = duration;
            fadeAction.m_TarAlpha = targetFade;
            fadeAction.m_FadeType = fadeType;
            return fadeAction;
        }

        #endregion

        #region ActionTween Extensions

        /// <summary>
        /// 设置缓冲动作
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="ease"></param>
        /// <returns></returns>
        public static ActionTween SetEase(this ActionTween tween,EaseEnum ease)
        {
            tween.mEaseEnum = ease;
            return tween;
        }

        /// <summary>
        /// 设置动画曲线
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="curve"></param>
        /// <returns></returns>
        public static ActionTween SetAnimCurve(this ActionTween tween,AnimationCurve curve)
        {
            tween.mActionCurve = curve;
            return tween;
        }

        /// <summary>
        /// 设置自动Kill
        /// </summary>
        /// <param name="tween"></param>
        /// <returns></returns>
        public static ActionTween SetAutoKill(this ActionTween tween,bool autoKill)
        {
            tween.mAutoKill = autoKill;
            return tween;
        }

        /// <summary>
        /// 设置运动方向
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static ActionTween SetActionDirectionType(this ActionTween tween , ActionEnum.ActionDirectionType direction)
        {
            tween.mActionDirectionType = direction;
            return tween;
        }

        /// <summary>
        /// 设置此动画的时间缩放
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="scaleTime"></param>
        /// <returns></returns>
        public static ActionTween SetActionScaleTime(this ActionTween tween,float scaleTime)
        {
            tween.mScaleTime = scaleTime;
            return tween;
        }

        /// <summary>
        /// 设置循环次数
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="loopTime"></param>
        /// <returns></returns>
        public static ActionTween SetLoopTime(this ActionTween tween, int loopTime)
        {
            tween.mLoopTime = loopTime;
            return tween;
        }

        /// <summary>
        /// 设置动画类型
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static ActionTween SetActionType(this ActionTween tween,ActionEnum.ActionType actionType)
        {
            tween.mActionType = actionType;
            return tween;
        }

        /// <summary>
        /// 是否启动速度来运行动画
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="useSpeed"></param>
        /// <returns></returns>
        public static ActionTween SetSpeedAble(this ActionTween tween,bool useSpeed)
        {
            tween.mUseSpeed = useSpeed;
            return tween;
        }

        /// <summary>
        /// 设置速度
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static ActionTween SetSpeed(this ActionTween tween,float speed)
        {
            tween.mSpeed = speed;
            return tween;
        }

        /// <summary>
        /// 是否采用相对关系
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="isRelative"></param>
        /// <returns></returns>
        public static ActionTween SetRelative(this ActionTween tween, bool isRelative)
        {
            tween.mRelative = isRelative;
            return tween;
        }

        /// <summary>
        /// 设置是否自动播放
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="autoPlay"></param>
        /// <returns></returns>
        public static ActionTween SetAutoPlay(this ActionTween tween, bool autoPlay)
        {
            tween.ActionActive = autoPlay;
            tween.mAutoPlay = autoPlay;
            return tween;
        }

        /// <summary>
        /// 动画开始的时候回调函数
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ActionTween SetBeginPlayCallback(this ActionTween tween, ActionTweenCallback callback)
        {
            tween.mOnBeginPlay = callback;
            return tween;
        }

        /// <summary>
        /// 动画完成时候回调
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ActionTween SetCompleteCallback(this ActionTween tween, ActionTweenCallback callback)
        {
            tween.mOnComplete = callback;
            return tween;
        }

        /// <summary>
        /// 动画被杀死时回调
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ActionTween SetKillCallback(this ActionTween tween, ActionTweenCallback callback)
        {
            tween.mOnKill = callback;
            return tween;
        }

        /// <summary>
        /// 每帧更新的时候调用，尽量不要在这个函数内调用过度消耗性能的方法
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ActionTween SetStepUpdateCallback(this ActionTween tween, ActionTweenCallback callback)
        {
            tween.mOnStepUpdate = callback;
            return tween;
        }

        /// <summary>
        /// 每次Loop完回调一次,最后一次循环不会触发此函数，直接触发OnComplete函数
        /// </summary>
        /// <param name="tween"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static ActionTween SetLoopCompleteCallback(this ActionTween tween, ActionTweenCallback callback)
        {
            tween.mOnLoopComplete = callback;
            return tween;
        }

        #endregion

    }
}
