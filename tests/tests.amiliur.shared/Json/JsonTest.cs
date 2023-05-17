using System.Text.Json;
using amiliur.figforms.shared.Models;
using amiliur.shared.Json;
using Xunit.Abstractions;

namespace tests.amiliur.shared.Json;

public class JsonTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public JsonTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestFormData()
    {
        var strPayload = """
        {
          "__objectType__": "amiliur.figforms.shared.Models.FormDataSearchInputModel, amiliur.figforms.shared",
          "FormDefinition": {
                "__objectType__": "amiliur.figforms.shared.FormDefinition, amiliur.figforms.shared",
                "FormContext": "admin",
                "FormModule": "saude",
                "FormCode": "doencas",
                "FormName": "Doenças",
                "FormTitle": "Editar Doença",
                "FormMode": "Edit",
                "FormDescription": "",
                "Rows": [
                    {
                        "__objectType__": "amiliur.figforms.shared.FormDefinitionRow, amiliur.figforms.shared",
                        "Cells": [
                            {
                                "__objectType__": "amiliur.figforms.shared.FormDefinitionCell, amiliur.figforms.shared",
                                "Fields": [
                                    {
                                        "__objectType__": "amiliur.figforms.shared.Fields.Models.TextFormFieldModel, amiliur.figforms.shared",
                                        "Multiline": false,
                                        "ReadOnly": true,
                                        "ValidationRules": [],
                                        "FieldBindings": [],
                                        "FieldName": "Code",
                                        "DisplayName": "Código",
                                        "Description": "Código da doença",
                                        "FieldRenderer": {
                                            "__objectType__": "amiliur.figforms.shared.Fields.Models.Renderers.TextFieldRenderer, amiliur.figforms.shared"
                                        },
                                        "IsHidden": false
                                    }
                                ],
                                "ColumnBootstrapSpan": 12,
                                "Visible": true
                            }
                        ]
                    },
                    {
                        "__objectType__": "amiliur.figforms.shared.FormDefinitionRow, amiliur.figforms.shared",
                        "Cells": [
                            {
                                "__objectType__": "amiliur.figforms.shared.FormDefinitionCell, amiliur.figforms.shared",
                                "Fields": [
                                    {
                                        "__objectType__": "amiliur.figforms.shared.Fields.Models.TextFormFieldModel, amiliur.figforms.shared",
                                        "Multiline": false,
                                        "ReadOnly": false,
                                        "ValidationRules": [
                                            {
                                                "__objectType__": "amiliur.figforms.shared.Validation.RequiredFormFieldValidation, amiliur.figforms.shared",
                                                "AllowEmptyStrings": false,
                                                "ErrorMessageString": "The field `Nome` is required"
                                            }
                                        ],
                                        "FieldBindings": [],
                                        "FieldName": "Nome",
                                        "DisplayName": "Nome",
                                        "Description": "Nome da doença",
                                        "FieldRenderer": {
                                            "__objectType__": "amiliur.figforms.shared.Fields.Models.Renderers.TextFieldRenderer, amiliur.figforms.shared"
                                        },
                                        "IsHidden": false
                                    }
                                ],
                                "ColumnBootstrapSpan": 12,
                                "Visible": true
                            }
                        ]
                    }
                ],
                "SaveOnClick": true,
                "DataTypeName": "Agroori.Shared.Doencas.DoencaEditModel, Agroori.Shared",
                "LoadDataSource": {
                    "__objectType__": "amiliur.figforms.shared.ApiDataSource, amiliur.figforms.shared",
                    "ApiUrl": "api/doenca",
                    "ApiMethod": "get-data"
                }
            },
          "DataFilter": {
                "__objectType__": "FieldValueExpr, amiliur.figforms.shared",
                "Field": "Id",
                "Value": "80787aa5-5a4a-42de-80a7-4f29847ea9ad",
                "FilterType": "None"
            },
          "Ts": "2023-05-16T17:46:37.0000000Z",
          "AsOf": "2023-05-16"
        }
        """;
        
        var options = SerializableModel.SerializerOptions();
        // var obj =  JsonSerializer.Deserialize<FormDataSearchInputModel>(strPayload, options);
        
        // _testOutputHelper.WriteLine(obj.ToString());
        // Assert.Equal(obj.Ts, new DateTime(2023, 5, 16, 17, 46, 37));
    }
}