﻿using System;

namespace SkinnyControllersCommon
{
    public enum TemplateIndicator:long
    {
        
        None = 0,
        AllPost=1,
        NoArgs_Is_Get_Else_Post=2,
        Rest=3,
        AllPostWithRecord =4,
        TryCatchLogging=5,
        CustomTemplateFile = 10000,
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoActionsAttribute:Attribute 
    {
        public TemplateIndicator template { get; set; }
        public string[] FieldsName { get; set; }
        public string[] ExcludeFields { get; set; }
        public string CustomTemplateFileName { get; set; }

    }



}
