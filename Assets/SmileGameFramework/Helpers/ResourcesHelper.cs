using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{

    /// <summary>
    /// Resources文件夹下资源加载器
    /// </summary>
    public class ResourcesHelper
    {
        /// <summary>
        /// 加载Resources路径下path资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

    }
}
