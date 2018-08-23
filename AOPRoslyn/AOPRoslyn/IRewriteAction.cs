using Newtonsoft.Json;

namespace AOPRoslyn
{
    /// <summary>
    /// generic rewrite ( used for File/Folder)
    /// </summary>
    public abstract class RewriteAction
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public RewriteAction()
        {
            this.Formatter = AOPFormatter.DefaultFormatter;
            this.Options = new RewriteOptions();
        }
        /// <summary>
        /// the action
        /// </summary>
        public abstract void Rewrite();
        /// <summary>
        /// the formatter
        /// </summary>
        public AOPFormatter Formatter { get; set; }
        /// <summary>
        /// the options
        /// </summary>
        public RewriteOptions Options { get; set; }
        /// <summary>
        /// how to serialize in a string
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// unserialize the action
        /// </summary>
        /// <param name="data">string that contains object RewriteAction serialized</param>
        /// <returns>RewriteAction</returns>
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
