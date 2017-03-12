namespace NerdCats.JsonSchemaDefaults.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void Test_Reading_Defaults_From_Schema_Gets_Default_Json()
        {
            var schemaDefaultGenerator = new SchemaDefaultGenerator();
            var defaultJSON = schemaDefaultGenerator.GetDefaults(JObject.Parse("{" +
                "\"title\": \"Album Options\", " +
                "\"type\": \"object\"," +
                "\"properties\": {" +
                "   \"sort\": {" +
                "       \"type\": \"string\"," +
                "       \"default\": \"id\"" +
                "   }," +
                "   \"per_page\": {" +
                "       \"default\": 30," +
                "       \"type\": \"integer\"" +
                "   }" +
                "}}"));

            var expectedResult = JObject.Parse("{ sort: 'id', per_page: 30 }");

            Assert.IsTrue(JToken.DeepEquals(defaultJSON, expectedResult));
        }

        [TestMethod]
        public void Test_Reading_Defaults_From_String_Schema_Gets_Default_Json()
        {
            var schemaDefaultGenerator = new SchemaDefaultGenerator();
            var defaultJSON = schemaDefaultGenerator.GetDefaults("{" +
                "\"title\": \"Album Options\", " +
                "\"type\": \"object\"," +
                "\"properties\": {" +
                "   \"sort\": {" +
                "       \"type\": \"string\"," +
                "       \"default\": \"id\"" +
                "   }," +
                "   \"per_page\": {" +
                "       \"default\": 30," +
                "       \"type\": \"integer\"" +
                "   }" +
                "}}");

            var expectedResult = JObject.Parse("{ sort: 'id', per_page: 30 }");

            Assert.IsTrue(JToken.DeepEquals(defaultJSON, expectedResult));
        }
    }
}
