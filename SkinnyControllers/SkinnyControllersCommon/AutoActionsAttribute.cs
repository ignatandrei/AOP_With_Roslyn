using System;

namespace SkinnyControllersCommon
{
    public enum TemplateIndicator
    {
        Default = 0,
        NoArgs_Get_Else_Post=1
    }
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class AutoActionsAttribute:Attribute 
    {
        public TemplateIndicator template { get; set; }
    }



}
