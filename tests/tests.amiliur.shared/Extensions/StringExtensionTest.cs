using amiliur.shared.Extensions;

namespace tests.amiliur.shared.Extensions;

public class StringExtensionTest
{
    [Fact(DisplayName = "Generate slug containing underscore")]
    public void GenerateSlugOfStringsWithUnderscores()
    {
        Assert.Equal("test_with_under-score", "test_with_under Score".GenerateSlug());
        Assert.Equal("a-piece-of", "A piece of text to be converted into a slug".GenerateSlug(10));
    }
}