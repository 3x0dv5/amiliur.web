using amiliur.shared.Extensions;

namespace tests.amiliur.shared.Extensions;

public class DatetimeExtensionsTest
{
    [Fact(DisplayName = "has time")]
    public void HasTimeTest()
    {
        Assert.False(new DateTime(2022, 1, 23).HasTime());
        Assert.True(new DateTime(2022, 1, 23, 10, 10, 10).HasTime());
    }
}