/********************************************************************
	created:	2019/01/10  11:13
	filename: 	EditorUtilityTools.cs
	author:		chuanshao
	
	purpose:	编辑器模式下实用工具集
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmileGame
{
    public static class EditorUtilityTools
    {
        /// <summary>
        /// 遍历目前所有的Dll，根据类型字符串获取类型
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetType(string typeName)
        {
            Type t = null;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type tt = asm.GetType(typeName);
                if (tt != null)
                {
                    t = tt;
                    return t;
                }
            }
            return t;
        }
    }
}
