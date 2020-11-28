﻿using System;

namespace SkinnyControllersCommon
{
    public enum TemplateIndicator:long
    {
        None = 0,
        AllPost=1
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoActionsAttribute:Attribute 
    {
        public TemplateIndicator template { get; set; }
        public string[] FieldsName { get; set; }
    }



}
