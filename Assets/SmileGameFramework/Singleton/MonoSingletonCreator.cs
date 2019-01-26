using SmileGame.Action;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{
    public static class MonoSingletonCreator
    {
        public static T CreateSingleton<T>() where T : MonoSingleton<T>
        {
            GameObject obj = new GameObject(typeof(T).ToString() + "->Instance");
            var retInstance = obj.AddComponent<T>(); //会立刻执行Awake 
            if (!retInstance.IsInit)
            {
                retInstance.OnSingletonInit();
                retInstance.IsInit = true;
            }
            return retInstance;
        }

    }
}
