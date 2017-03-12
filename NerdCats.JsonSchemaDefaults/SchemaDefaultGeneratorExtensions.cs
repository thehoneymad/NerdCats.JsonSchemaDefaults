namespace NerdCats.JsonSchemaDefaults
{
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;

    public static class SchemaDefaultGeneratorExtensions
    {
        public static JToken GetDefaultJSON(this JSchema schema, bool validateGeneratedJson = true)
        {
            SchemaDefaultGenerator generator = new SchemaDefaultGenerator();
            return generator.GetDefaults(schema, validateGeneratedJson);
        }
    }
}
