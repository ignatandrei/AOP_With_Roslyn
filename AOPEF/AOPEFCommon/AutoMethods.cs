using System;

namespace AOPEFCommon
{
    public enum TemplateRepositoryMethod: long
    {

        None = 0,
        GenericRepository,
        CustomTemplateFile = 10000,
    }
    
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class RepositoryAttribute : Attribute
    {
        public TemplateRepositoryMethod template { get; set; }
        public string CustomTemplateFileName { get; set; }

        public string POCOName { get; set; }

        public string PK1 { get; set; }
    }
    


}
