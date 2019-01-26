using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame.Action
{
    /// <summary>
    /// 动画接口
    /// </summary>
    public interface IActionTween
    {
        void Play(bool autoActive = true);

        void RePlay(bool autoActive = true);

        void Revert(bool autoActive = true);

        void Pause();

        void Resume();

        void Reset();

        void Kill();

    }
}
