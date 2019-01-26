using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

/// <summary>
/// 对Log日志重定向打开文件
/// </summary>
public class SGLogRedirect  {

    private const string logCSName = "LOG.cs";
    private static object logListView;
    private static EditorWindow consoleWindow;
    private static FieldInfo logListViewCurrentRow;
    private static MethodInfo LogEntriesGetEntry;
    private static object logEntry;
    private static FieldInfo logEntryCondition;
    private static int openInstanceID;
    private static int openLine;

    private static bool GetConsoleWindowListView()
    {
        if (logListView == null)
        {
            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            Type consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            consoleWindow = fieldInfo.GetValue(null) as EditorWindow;

            if (consoleWindow == null)
            {
                logListView = null;
                return false;
            }

            FieldInfo listViewFieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
            logListView = listViewFieldInfo.GetValue(consoleWindow);
            logListViewCurrentRow = listViewFieldInfo.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);

            Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
            LogEntriesGetEntry = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
            Type logEntryType = unityEditorAssembly.GetType("UnityEditor.LogEntry");
            logEntry = Activator.CreateInstance(logEntryType);
            logEntryCondition = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
        }

        return true;
    }


    private static string GetListViewRowCount(ref int line)
    {
        int row = (int)logListViewCurrentRow.GetValue(logListView);
        LogEntriesGetEntry.Invoke(null, new object[] { row, logEntry });
        string condition = logEntryCondition.GetValue(logEntry) as string;

        int index = condition.IndexOf(logCSName);
        if (index < 0)//不是经过我们封装的日志
        {
            return null;
        }

        int lineIndex = condition.IndexOf(")", index);
        condition = condition.Substring(lineIndex + 2);
        index = condition.IndexOf(".cs:");

        if (index >= 0)
        {
           // int lineStartIndex = condition.IndexOf(")");
            int lineEndIndex = condition.IndexOf(")", index);
            string _line = condition.Substring(index + 4, lineEndIndex - index - 4);
            Int32.TryParse(_line, out line);

            condition = condition.Substring(0, index);
            int startIndex = condition.LastIndexOf("/");

            string fileName = condition.Substring(startIndex + 1);
            fileName += ".cs";
            return fileName;
        }

        return null;
    }

    [OnOpenAssetAttribute(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (!EditorWindow.focusedWindow.titleContent.text.Equals("Console"))//只对控制台的开启进行重定向
            return false;

        if (openInstanceID == instanceID && openLine == line)
        {
            openInstanceID = -1;
            openLine = -1;
            return false;
        }
        openInstanceID = instanceID;
        openLine = line;

        if (!GetConsoleWindowListView())
        {
            return false;
        }


        string fileName = GetListViewRowCount(ref line);

        if (fileName == null)
        {
            return false;
        }

        if (fileName.EndsWith(".cs"))
        {
            string filter = fileName.Substring(0, fileName.Length - 3);
            filter += " t:MonoScript";
            string[] searchPaths = AssetDatabase.FindAssets(filter);

            for (int i = 0; i < searchPaths.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(searchPaths[i]);

                if (path.EndsWith(fileName))
                {
                    UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    AssetDatabase.OpenAsset(obj, line);
                    return true;
                }
            }
        }


        return false;
    }

}
