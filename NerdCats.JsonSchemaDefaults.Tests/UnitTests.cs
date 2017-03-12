namespace NerdCats.JsonSchemaDefaults.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;

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

        [TestMethod]
        public void Test_Reading_Defaults_From_Int_Array_Schema_With_String_Defaults_Throws_Exception()
        {
            Assert.ThrowsException<JSchemaValidationException>(() =>
            {
                var schemaDefaultGenerator = new SchemaDefaultGenerator();
                var defaultJSON = schemaDefaultGenerator.GetDefaults("{" +
                    "type: 'array'," +
                    "items : {" +
                        " type : 'number'" +
                        "}," +
                    "default: ['getchute', 'chute']" +
                     "}");
            });
        }

        [TestMethod]
        public void Test_Reading_Defaults_From_String_Array_Schema_Gets_Default_Data()
        {
            var schemaDefaultGenerator = new SchemaDefaultGenerator();
            var defaultJSON = schemaDefaultGenerator.GetDefaults("{" +
                "type: 'array'," +
                "items : {" +
                    " type : 'string'" +
                    "}," +
                "default: ['getchute', 'chute']" +
                 "}");

            var expectedResult = JArray.Parse("['getchute', 'chute']");
            Assert.IsTrue(JToken.DeepEquals(defaultJSON, expectedResult));
        }

        [TestMethod]
        public void Test_Reading_Defaults_From_String_Schema_With_Enum_And_Wrong_Defaults_Throws()
        {
            Assert.ThrowsException<JSchemaValidationException>(() =>
            {
                var schemaDefaultGenerator = new SchemaDefaultGenerator();
                var defaultJSON = schemaDefaultGenerator.GetDefaults("{" +
                    "\"title\": \"Album Options\", " +
                    "\"type\": \"object\"," +
                    "\"properties\": {" +
                    "   \"sort\": {" +
                    "       \"type\": \"string\"," +
                    "        enum: ['somethingElse']," +
                    "       \"default\": \"id\"" +
                    "   }," +
                    "   \"per_page\": {" +
                    "       \"default\": 30," +
                    "       \"type\": \"integer\"" +
                    "   }" +
                    "}}");
            });
        }

        [TestMethod]
        public void Test_Reading_Defaults_From_String_Schema_With_Enum_Gets_Defaults()
        {
            var schemaDefaultGenerator = new SchemaDefaultGenerator();
            var defaultJSON = schemaDefaultGenerator.GetDefaults("{" +
                "\"title\": \"Album Options\", " +
                "\"type\": \"object\"," +
                "\"properties\": {" +
                "   \"sort\": {" +
                "       \"type\": \"string\"," +
                "        enum: ['id']," +
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

        [TestMethod]
        public void Test_Reading_Defaults_From_String_Schema_With_Local_Reference()
        {
            var schemaDefaultGenerator = new SchemaDefaultGenerator();
            var schemaJson = "{" +
                "type: 'object'," +
                "properties: {" +
                    "farewell_to_arms: { " +
                        "allOf: [" +
                            "{'$ref': '#/definitions/book'}," +
                            "{'properties': {" +
                                    "price: {" +
                                        "default : 30" +
                                    "}" +
                            "}" +
                        "}]" +
                     "}," +
                    "for_whom_the_bell_tolls: {" +
                        "allOf: [" +
                            "{'$ref': '#/definitions/book'}, " +
                            "{ properties: { " +
                                " price: { " +
                                    "default: 100 " +
                                    "}" +
                                "}" +
                             "}" +
                           "]" +
                        "}" +
                  "}," +
                "definitions: {" +
                    "book: {" +
                        "type: 'object'," +
                        "properties: {" +
                            "author: {" +
                                "type: 'string'," +
                                "default: 'Hemingway'" +
                             "}," +
                            "price: {" +
                                "type: 'integer'," +
                                "default: 10" +
                            "}" +
                        "}" +
                    "}" +
                "}" +
            "}";

            var defaultJson = schemaDefaultGenerator.GetDefaults(schemaJson);
            var expectedDefault = JObject.Parse("{ farewell_to_arms: { author: 'Hemingway', price: 30 }, for_whom_the_bell_tolls: { author: 'Hemingway', price: 100 } }");

            Assert.IsTrue(JToken.DeepEquals(defaultJson, expectedDefault));
        }
    }
}
