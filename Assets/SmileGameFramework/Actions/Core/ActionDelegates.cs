using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
        /// <summary>
        /// 补间动画回调委托
        /// </summary>
        public delegate void ActionTweenCallback();

        /// <summary>
        /// Ease缓冲委托
        /// </summary>
        /// <param name="time"></param>
        /// <param name="duration"></param>
        /// <param name="overshootOrAmplitude"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public delegate float EaseFunction(float time, float duration, float overshootOrAmplitude, float period);
    
}
