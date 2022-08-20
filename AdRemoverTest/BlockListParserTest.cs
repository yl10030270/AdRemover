using AdRemover.Core;
using FluentAssertions;

namespace AdRemoverTest
{
    public class BlockListParserTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ParserTest()
        {
            var blockList = await BlockListParser.Parse(@".\TestData\blocklist.txt");
            blockList.Should().NotBeEmpty();

        }
    }
}