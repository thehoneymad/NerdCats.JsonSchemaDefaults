# Json Schema Defaults for .net
A simple helper library to generate default JSON from a JSONSchema. Written in .NET standard for maximum portability and proudly [JSON.net](https://github.com/JamesNK/Newtonsoft.Json) and [Newtonsoft.Json.Schema](https://github.com/JamesNK/Newtonsoft.Json.Schema) driven.

## How to Build
Clone the repo, run `dotnet restore` and `dotnet build`. Built on Visual Studio 2017 with .net core SDK 1.1.0

## Usage

#### Generate default JSON example 1
Sample JSON:

```json
{
    title: "Album Options",
    type: "object",
    properties: {
        sort: {
                type : "string",
                default: "id"
            },
        per_page : {
                default : 30,
                type: "integer"
        }
    }
}
```

```csharp
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
```

## Contributors

* Swagata 'thehoneymad' Prateek @SwagataPrateek


## License
(c) 2017 NerdCats. Released under the terms of the MIT License.