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

        public JToken GetDefaults(JObject schema)
        {
            return GetDefaults(schema.ToString());
        }

        public JToken GetDefaults(string schema)
        {
            var schemaObj = JSchema.Parse(schema);
            var schemaProperties = schemaObj.Properties;

            // TODO: Expecting to parse only Object JSON schemas for now, arrays are coming soon
            if (schemaProperties == null || !schema.Any())
                return JObject.Parse("{}");

            var finalResult = GetDefaultsFromSchema(schemaObj);
            
            switch(finalResult.Type)
            {
                case JTokenType.Object:
                    return finalResult as JObject;
                case JTokenType.Array:
                    return finalResult as JArray;
                default:
                    throw new ArgumentException();
            }
        }

        private JToken GetDefaultsFromSchema(JSchema schemaObj)
        {
            JToken returnToken = default(JToken);
            switch (schemaObj.Type)
            {
                case JSchemaType.Array:
                    returnToken = GetDefaultValueFromArray(schemaObj);
                    break;
                case JSchemaType.Object:
                    returnToken = GetDefaultsFromObject(schemaObj);
                    break;
                case JSchemaType.String:
                case JSchemaType.Integer:
                case JSchemaType.Boolean:
                    returnToken = GetDefaultValue(schemaObj);
                    break;
                case JSchemaType.None:
                case JSchemaType.Null:
                    returnToken = new JObject();
                    break;
                default:
                    throw new NotImplementedException(schemaObj.Type.ToString());
            }

            return returnToken;
        }

        private JToken GetDefaultValueFromArray(JSchema schemaObj)
        {
            if (schemaObj.Items?.Count == 0)
                return new JArray();

            var minItemCount = schemaObj.MinimumItems ?? 0;

            var enumerableArray = schemaObj.Items.Select(schema => GetDefaultsFromSchema(schema));

            return new JArray(enumerableArray);
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
