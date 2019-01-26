/********************************************************************
	created:	2015/12/06 13:02  
	filename: 	EnumHelper
	author:		Chuanshao
	
	purpose:	枚举获取字符串值帮助类
*********************************************************************/

using System;

/// <summary>
/// 枚举获取字符串值帮助类
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// 通过自定义特性“EnumDescriptionAttribute”获取枚举成员描述信息
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(Enum value)
    {
        if (value == null)
        {
            throw new ArgumentException("value");
        }
        string description = value.ToString();
        var fieldInfo = value.GetType().GetField(description);
        var attributes =
            (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
        if (attributes != null && attributes.Length > 0)
        {
            description = attributes[0].Description;
        }
        return description;
    }

    /// <summary>
    /// 通过字符串获取枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strType"></param>
    /// <returns></returns>
    public static T GetEnumType<T>(string strType)
    {
        T t = (T)Enum.Parse(typeof(T), strType);
        return t;
    }

}




