using amiliur.shared.Utils;
using Xunit.Abstractions;

namespace tests.amiliur.shared.Utils;

public class DateTimeUtilsTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DateTimeUtilsTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestParseFromIso()
    {
        Assert.Equal(new DateTime(2023, 5, 16, 17, 46, 37), DateTimeUtils.ParseFromIso("2023-05-16T17:46:37.0000000Z"));
        Assert.True(DateTimeUtils.ParseFromIso("2023-05-16T17:46:37.0000000Z")!.Value.Kind == DateTimeKind.Utc);
    }
    
    [Fact]
    public void TestParseDateFromIso()
    {
        Assert.Equal(new DateTime(2022, 4, 25), DateTimeUtils.ParseFromIso("2022-04-25"));
        Assert.True(DateTimeUtils.ParseFromIso("2022-04-25")!.Value.Kind == DateTimeKind.Utc);
    }
}