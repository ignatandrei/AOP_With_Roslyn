using Newtonsoft.Json;

namespace AOPRoslyn
{
    public abstract class RewriteAction
    {
        public RewriteAction()
        {
            this.Formatter = AOPFormatter.DefaultFormatter;
            this.Options = new RewriteOptions();
        }
        public abstract void Rewrite();

        public AOPFormatter Formatter { get; set; }
        public RewriteOptions Options { get; set; }
        public string SerializeMe()
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,

                //Error = HandleDeserializationError
                ConstructorHandling= ConstructorHandling.AllowNonPublicDefaultConstructor

            };
            return JsonConvert.SerializeObject(this, settings);

        }
        public static RewriteAction UnSerializeMe(string data)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                //Error = HandleDeserializationError
                ConstructorHandling= ConstructorHandling.AllowNonPublicDefaultConstructor

            };
            //settings.Converters.Add(new JsonEncodingConverter());
            //var i = new Interpret();
            //var newText = i.InterpretText(serializeData);
            var ret = (RewriteAction)JsonConvert.DeserializeObject(data, settings);
            return ret;
        }
    }
}
