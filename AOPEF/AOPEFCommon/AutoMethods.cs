using System;

namespace AOPEFCommon
{
    public enum TemplateMethod: long
    {

        None = 0,
        GenericRepository,
        GenericSearch,
        CustomTemplateFile = 10000,
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class TemplateAttribute : Attribute
    {
        public TemplateMethod template { get; set; }
        public string CustomTemplateFileName { get; set; }

        //public string POCOName { get; set; }

        public string PK1 { get; set; }
    }
    


}
