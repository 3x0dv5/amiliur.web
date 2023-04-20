
using amiliur.shared.Reflection;

namespace tests.amiliur.shared.Reflection;

public class PropertiesTest
{
    class ObjTest
    {
        public string? Value { get; set; }
        public string? ValueInt { get; set; }
        public string? ValueFloat { get; set; }
        public string? ValueDatetime { get; set; }
        public string? ValueDate { get; set; }
        public DateTime? ADate { get; set; }
    }

    class ObjTolist
    {
        public string? Name { get; set; }
    }

    class ObjWithList
    {
        public List<ObjTolist> Objects { get; set; }
    }

    enum Valores
    {
        Zero
    }

    [Fact(DisplayName = "Set a string prop from enum")]
    public void StringFromEnumTest()
    {
        var obj = new ObjTest();

        obj.SetPropertyValue(nameof(ObjTest.Value), Valores.Zero);

        Assert.Equal("Zero", obj.Value);

        obj.SetPropertyValue(nameof(ObjTest.ValueInt), 10);
        Assert.Equal("10", obj.ValueInt);

        obj.SetPropertyValue(nameof(ObjTest.ValueFloat), 10.54f);
        Assert.Equal("10.54", obj.ValueFloat);

        obj.SetPropertyValue(nameof(ObjTest.ValueDatetime), new DateTime(2023, 1, 15, 10, 19, 30));
        Assert.Equal("2023-01-15T10:19:30.0000000", obj.ValueDatetime);


        obj.SetPropertyValue(nameof(ObjTest.ValueDate), new DateTime(2023, 1, 15));
        Assert.Equal("2023-01-15", obj.ValueDate);

        
    }

    [Fact(DisplayName = "Set a list of known type from a list of objects")]
    public void SetPropertyValueListOfObject()
    {
        var list = new List<object>()
        {
            new ObjTolist {Name = "nome 1"}
        };

        var obj = new ObjWithList();
        obj.SetPropertyValue(nameof(ObjWithList.Objects), list);
        Assert.Equal(obj.Objects, list);
    }
    
    [Fact(DisplayName = "Set a datetime property from a string")]
    public void SetPropertyValueOfADateFromString()
    {
        var obj = new ObjTest();
        obj.SetPropertyValue(nameof(ObjTest.ADate), "2022-04-25");
        Assert.Equal(new DateTime(2022, 4, 25), obj.ADate);
    }
}