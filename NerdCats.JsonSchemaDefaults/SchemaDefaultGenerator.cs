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

        public JToken GetDefaults(string schema, bool validateGeneratedJson = true)
        {
            var schemaObj = JSchema.Parse(schema);
            var schemaProperties = schemaObj.Properties;

            // TODO: Expecting to parse only Object JSON schemas for now, arrays are coming soon
            if (schemaProperties == null || !schema.Any())
                return JObject.Parse("{}");

            var finalResult = GetDefaultsFromSchema(schemaObj);

            if (validateGeneratedJson)
                finalResult.Validate(schemaObj);

            switch (finalResult.Type)
            {
                case JTokenType.Object:
                    return finalResult as JObject;
                case JTokenType.Array:
                    return finalResult as JArray;
                default:
                    throw new InvalidOperationException();
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
                case null:
                    returnToken = GetDefaultsFromOperators(schemaObj);
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

        private JToken GetDefaultsFromOperators(JSchema schemaObj)
        {
            /* INFO: We are here because the schema is lacking a type. 
             * We will check for allOf operator for now and if we see one
             * We will try to generate a default JSON based on what we see
             * and use the default value found in the subschema. */
            JToken returnObject = default(JToken);

            if (schemaObj.AllOf?.Count > 0)
            {
                returnObject = new JObject(); // AllOf requires the type to be an object by default
                foreach (var subSchema in schemaObj.AllOf)
                    foreach (var property in subSchema.Properties.Keys)
                        returnObject[property] = GetDefaultsFromSchema(subSchema.Properties[property]);
            }
            else
            {
                // INFO: This is a fallback, this property came without any type 
                // and we have no clue what it is, last resort is treating it as a value
                returnObject = GetDefaultValue(schemaObj);
            }
            return returnObject;
        }

        private JToken GetDefaultValueFromArray(JSchema schemaObj)
        {
            if (schemaObj.Items?.Count == 0)
                return new JArray();

            var defaultArray = schemaObj.Default as JArray;
            if (defaultArray == null)
                throw new NullReferenceException(nameof(defaultArray));

            defaultArray.Validate(schemaObj);
            return defaultArray;
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
