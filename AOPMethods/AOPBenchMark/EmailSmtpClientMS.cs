using AOPMethodsCommon;
using System.Linq;

namespace AOPBenchMark
{
    [AutoMethods(template = TemplateMethod.CustomTemplateFile, CustomTemplateFileName = "ClassToDictionary.txt")]

    public partial class EmailSmtpClientMS 
    {
        public string GetHostReflection()
        {
            return this.GetType().GetProperty("Host").GetValue(this).ToString();
        }
        public string GetHostViaDictionary()
        {
            return this.ReadMyProperties().First(it => it.Key == "Host").Value.ToString();
        }
        public string GetHostViaSwitch()
        {
            return this.GetValueProperty("Host").ToString();
        }
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
