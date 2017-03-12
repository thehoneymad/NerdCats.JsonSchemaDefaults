namespace NerdCats.JsonSchemaDefaults
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
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

            return GetDefaultsFromSchema(schemaObj) as JObject;
        }

        private JToken GetDefaultsFromSchema(JSchema schemaObj)
        {
            JToken returnToken = default(JToken);
            switch (schemaObj.Type)
            {
                case JSchemaType.Object:
                    returnToken = GetDefaultsFromObject(schemaObj);
                    break;
                case JSchemaType.String:
                    returnToken = GetDefaultValue(schemaObj);
                    break;
                case JSchemaType.Integer:
                    returnToken = GetDefaultValue(schemaObj);
                    break;
                default:
                    throw new NotImplementedException(schemaObj.Type.ToString());
            }

            return returnToken;
        }

        private JObject GetDefaultsFromObject(JSchema schemaObj)
        {
            var schemaProperties = schemaObj.Properties;
            if (schemaProperties?.Count == 0)
                return new JObject();

            var returnObject = new JObject();
            foreach (var property in schemaProperties.Keys)
            {
                returnObject[property] = GetDefaultsFromSchema(schemaProperties[property]);
            }
            return returnObject;
        }

        private JToken GetDefaultValue(JSchema jSchema)
        {
            var defaultVal = jSchema.Default;
            return defaultVal;
        }
    }
}
