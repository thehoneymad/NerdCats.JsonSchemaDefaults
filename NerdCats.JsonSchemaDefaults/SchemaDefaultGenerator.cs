namespace NerdCats.JsonSchemaDefaults
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System.Linq;

    public class SchemaDefaultGenerator
    {
        public SchemaDefaultGenerator()
        { }

        public JObject GetDefaults(JObject schema)
        {
            return GetDefaults(schema.ToString());
        }

        public JObject GetDefaults(string schema)
        {
            var schemaObj = JSchema.Parse(schema);
            var schemaProperties = schemaObj.Properties;

            // TODO: Expecting to parse only Object JSON schemas for now, arrays are coming soon
            if (schemaProperties == null || !schema.Any())
                return JObject.Parse("{}");

            var returnObject = new JObject();

            foreach (var property in schemaProperties.Keys)
            {
                returnObject[property] = GetDefaultValue(schemaProperties[property]);
            }

            return returnObject;
        }

        private JToken GetDefaultValue(JSchema jSchema)
        {
            var defaultVal =  jSchema.Default;
            return defaultVal;
        }
    }
}
