# Json Schema Defaults for .net
A simple helper library to generate default JSON from a JSONSchema. Written in .NET standard for maximum portability and proudly [JSON.net](https://github.com/JamesNK/Newtonsoft.Json) and [Newtonsoft.Json.Schema](https://github.com/JamesNK/Newtonsoft.Json.Schema) driven.

## Install
You can install it from [Nuget](https://www.nuget.org/packages/NerdCats.JsonSchemaDefaults/) using

```
PM> Install-Package NerdCats.JsonSchemaDefaults
```

## How to Build
Clone the repo, run `dotnet restore` and `dotnet build`. Built on Visual Studio 2017 with .net core SDK 1.1.0

## Usage

#### Generate default JSON example 1
Sample JSON schema:

```json
{
    "title": "Album Options",
    "type": "object",
    "properties": {
        "sort": {
                "type" : "string",
                "default": "id"
            },
        "per_page" : {
                "default" : 30,
                "type": "integer"
        }
    }
}
```

To generate a JSON with all default values defined here use `SchemaDefaultGenerator` like the following:

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

Generated default JSON would be :

```json
    {
        "sort": "id",
        "per_page": 30
    }
```

You can also opt for the sweet extension method for `JSchema` class like the following:

```csharp
var schema = JSchema.Parse("{" +
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
var defaultJSON = schema.GetDefaultJSON();
var expectedResult = JObject.Parse("{ sort: 'id', per_page: 30 }");

Assert.IsTrue(JToken.DeepEquals(defaultJSON, expectedResult));
```

#### Generate default JSON example 2

Let's up the ante a bit with local references and allOf operator

Sample JSON schema:

```json
{
    "type": "object",
    "properties": {
        "farewell_to_arms": {
            "allOf": [
                {
                    "$ref": "#/definitions/book"
                },
                {
                    "properties": {
                            "price": {
                                "default" : 30,
                            }
                        }
                }
            ]
        },
        "for_whom_the_bell_tolls": {
            "allOf": [
                {
                    "$ref": "#/definitions/book"
                },
                {
                    "properties": {
                        "price": {
                            "default": 100
                        }
                    }
                }
            ]
        }
    },
    "definitions": {
        "book": {
            "type": "object" ,
            "properties": {
                "author": {
                    "type": "string",
                    "default": "Hemingway",
                },
                "price": {
                    "type": "integer",
                    "default": 10,
                }
            }
        }
    }
}
```

To generate a default JSON from this let's go the same way we have gone a moment ago

```csharp
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
```
The resultant default JSON would look like

```json
{
    "farewell_to_arms": {
        "author": "Hemingway",
        "price": 30
    },
    "for_whom_the_bell_tolls": {
        "author": "Hemingway",
        "price": 100
    }
}
```

## Known Limitations
This library conforms to JsonSchema draft 4. This still doesn't support default values for `anyOf` and `oneOf` operator defaults.

## Contributors

* Swagata 'thehoneymad' Prateek @SwagataPrateek

# Special mentions
Thanks goes to Eugene Tsypkin for [json-schema-defaults](https://github.com/chute/json-schema-defaults) for inspiration.


## License
Released under the terms of the MIT License.
