using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//StackTrace st = new StackTrace(new StackFrame(true));
//StackFrame sf = st.GetFrame(0);
//Console.WriteLine(" File: {0}", sf.GetFileName());                               //文件名
//Console.WriteLine(" Method: {0}", sf.GetMethod().Name);                          //函数名
//Console.WriteLine(" Line Number: {0}", sf.GetFileLineNumber());                  //文件行号
//Console.WriteLine(" Column Number: {0}", sf.GetFileColumnNumber());
namespace SmileGame
{
    public class LOG 
    {

        private static System.Diagnostics.StackFrame GetStackFrame(int layer = 2)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(new System.Diagnostics.StackFrame(layer, true));
            return st.GetFrame(0);
        }

        private static string FormatLogMsg(string msg, System.Diagnostics.StackFrame sf)
        {
            return "<b><color={0}><size={1}>" + msg + "</size></color></b>\n\t<color=#CC6600>File:" + sf.GetFileName() + "</color>\n\t<color=#009900>Line:" + sf.GetFileLineNumber() + "</color>   <color=#633974>Method : " + sf.GetMethod().Name+ "</color>";
        }

        public static void Log(string msg,string tag = "GameLog",int layer = 2)
        {
            Debug.unityLogger.Log(tag, string.Format(FormatLogMsg(msg, GetStackFrame(layer)), "#0033FF",12));
        }

        public static void LogError(string msg, string tag = "GameErr", int layer = 2)
        {
            Debug.unityLogger.LogError(tag, string.Format(FormatLogMsg(msg, GetStackFrame(layer)),"red",12));
        }

        public static bool Assert(bool isTrue, string msg)
        {
            if (!isTrue)
            {
                Log(msg);
            }
            return isTrue;
        }

        public static void LogDic<T, T1>(Dictionary<T, T1> dic)
        {
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    Debug.Log("Key -->> " + item.Key + "  Value -->> " + item.Value);
                }

            }
        }

        public static void LogArray<T>(T[] array)
        {
            if (array != null)
            {
                int i = 0;
                foreach (var item in array)
                {
                    Debug.Log("Item_"+i +" --> "+item);
                    i++;
                }
            }
        }

    }
}
