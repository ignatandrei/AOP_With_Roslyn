using AOPMethodsCommon;

namespace AOPBenchMark
{
    [AutoMethods(template = TemplateMethod.CustomTemplateFile, CustomTemplateFileName = "ClassToDictionary.txt")]

    public partial class EmailSmtpClientMS 
    {
        public EmailSmtpClientMS()
        {

            Port = 25;

        }
        public string Name { get; set; }


        public string Type
        {
            get
            {
                return this.GetType().Name;
            }
        }
        public string Host { get; set; }
        public int Port { get; set; }

        public string Description
        {
            get
            {
                return $"{Type} {Host}:{Port}";
            }
        }
    }
}
