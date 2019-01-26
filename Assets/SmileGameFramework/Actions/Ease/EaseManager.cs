using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    public class EaseManager
    {

        

        const float _PiOver2 = Mathf.PI * 0.5f;
        const float _TwoPi = Mathf.PI * 2;

        /// <summary>
        /// Returns a value between 0 and 1 (inclusive) based on the elapsed time and EaseEnum selected
        /// </summary>
        public static float Evaluate(EaseEnum easeType, EaseFunction customEase, float time, float duration, float overshootOrAmplitude, float period)
        {
            switch (easeType)
            {
                case EaseEnum.Linear:
                    return time / duration;
                case EaseEnum.InSine:
                    return -(float)Math.Cos(time / duration * _PiOver2) + 1;
                case EaseEnum.OutSine:
                    return (float)Math.Sin(time / duration * _PiOver2);
                case EaseEnum.InOutSine:
                    return -0.5f * ((float)Math.Cos(Mathf.PI * time / duration) - 1);
                case EaseEnum.InQuad:
                    return (time /= duration) * time;
                case EaseEnum.OutQuad:
                    return -(time /= duration) * (time - 2);
                case EaseEnum.InOutQuad:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time;
                    return -0.5f * ((--time) * (time - 2) - 1);
                case EaseEnum.InCubic:
                    return (time /= duration) * time * time;
                case EaseEnum.OutCubic:
                    return ((time = time / duration - 1) * time * time + 1);
                case EaseEnum.InOutCubic:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time;
                    return 0.5f * ((time -= 2) * time * time + 2);
                case EaseEnum.InQuart:
                    return (time /= duration) * time * time * time;
                case EaseEnum.OutQuart:
                    return -((time = time / duration - 1) * time * time * time - 1);
                case EaseEnum.InOutQuart:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time;
                    return -0.5f * ((time -= 2) * time * time * time - 2);
                case EaseEnum.InQuint:
                    return (time /= duration) * time * time * time * time;
                case EaseEnum.OutQuint:
                    return ((time = time / duration - 1) * time * time * time * time + 1);
                case EaseEnum.InOutQuint:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * time * time * time * time * time;
                    return 0.5f * ((time -= 2) * time * time * time * time + 2);
                case EaseEnum.InExpo:
                    return (time == 0) ? 0 : (float)Math.Pow(2, 10 * (time / duration - 1));
                case EaseEnum.OutExpo:
                    if (time == duration) return 1;
                    return (-(float)Math.Pow(2, -10 * time / duration) + 1);
                case EaseEnum.InOutExpo:
                    if (time == 0) return 0;
                    if (time == duration) return 1;
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (float)Math.Pow(2, 10 * (time - 1));
                    return 0.5f * (-(float)Math.Pow(2, -10 * --time) + 2);
                case EaseEnum.InCirc:
                    return -((float)Math.Sqrt(1 - (time /= duration) * time) - 1);
                case EaseEnum.OutCirc:
                    return (float)Math.Sqrt(1 - (time = time / duration - 1) * time);
                case EaseEnum.InOutCirc:
                    if ((time /= duration * 0.5f) < 1) return -0.5f * ((float)Math.Sqrt(1 - time * time) - 1);
                    return 0.5f * ((float)Math.Sqrt(1 - (time -= 2) * time) + 1);
                case EaseEnum.InElastic:
                    float s0;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s0 = period / 4;
                    }
                    else s0 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return -(overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s0) * _TwoPi / period));
                case EaseEnum.OutElastic:
                    float s1;
                    if (time == 0) return 0;
                    if ((time /= duration) == 1) return 1;
                    if (period == 0) period = duration * 0.3f;
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s1 = period / 4;
                    }
                    else s1 = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    return (overshootOrAmplitude * (float)Math.Pow(2, -10 * time) * (float)Math.Sin((time * duration - s1) * _TwoPi / period) + 1);
                case EaseEnum.InOutElastic:
                    float s;
                    if (time == 0) return 0;
                    if ((time /= duration * 0.5f) == 2) return 1;
                    if (period == 0) period = duration * (0.3f * 1.5f);
                    if (overshootOrAmplitude < 1)
                    {
                        overshootOrAmplitude = 1;
                        s = period / 4;
                    }
                    else s = period / _TwoPi * (float)Math.Asin(1 / overshootOrAmplitude);
                    if (time < 1) return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2, 10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period));
                    return overshootOrAmplitude * (float)Math.Pow(2, -10 * (time -= 1)) * (float)Math.Sin((time * duration - s) * _TwoPi / period) * 0.5f + 1;
                case EaseEnum.InBack:
                    return (time /= duration) * time * ((overshootOrAmplitude + 1) * time - overshootOrAmplitude);
                case EaseEnum.OutBack:
                    return ((time = time / duration - 1) * time * ((overshootOrAmplitude + 1) * time + overshootOrAmplitude) + 1);
                case EaseEnum.InOutBack:
                    if ((time /= duration * 0.5f) < 1) return 0.5f * (time * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time - overshootOrAmplitude));
                    return 0.5f * ((time -= 2) * time * (((overshootOrAmplitude *= (1.525f)) + 1) * time + overshootOrAmplitude) + 2);
                case EaseEnum.InBounce:
                    return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
                case EaseEnum.OutBounce:
                    return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
                case EaseEnum.InOutBounce:
                    return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
                case EaseEnum.INTERNAL_Custom:
                    return customEase(time, duration, overshootOrAmplitude, period);

                // Default
                default:
                    // OutQuad
                    return -(time /= duration) * (time - 2);
            }
        }

    }
}
