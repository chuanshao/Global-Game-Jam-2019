using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{

    public enum EaseEnum
    {

       // Unset, // Used to let TweenParams know that the ease was not set and apply it differently if used on Tweeners or Sequences
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
        /// <summary>
        /// Don't assign this! It's assigned automatically when setting the ease to an AnimationCurve or to a custom ease function
        /// </summary>
        INTERNAL_Custom

    }
}
