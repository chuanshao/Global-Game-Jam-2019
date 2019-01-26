/********************************************************************
	created:	2015/12/06 12:59  
	filename: 	EnumDescriptionAttribute
	author:		Chuanshao
	
	purpose:	标签属性
*********************************************************************/

using System;


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public  class EnumDescriptionAttribute : Attribute
{
    private string description;
    public string Description { get { return description; } }

    public EnumDescriptionAttribute(string description)
        : base()
    {
        this.description = description;
    }
}
