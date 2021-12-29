using CompoundWords;
using System.IO;
using Xunit;

namespace UnitTestWords
{
    public class UnitTest1
    {
        string[] dictionary = File.ReadAllLines(@".\Resources\de-dictionary.tsv");

        [Fact]
        public void TestNotAllowedToSplit()
        {
            string original = "Psychologie";
            var result = Program.SplitWord(dictionary, original);
            Assert.Equal("psychologie", result.ToLower());
        }

        [Fact]
        public void TestAllowedToSplit()
        {
            string original = "Krankenhaus";
            var result = Program.SplitWord(dictionary, original);
            Assert.Equal("kranken haus", result.ToLower());
        }

        [Fact]
        public void TestWrongAllowedToSplit()
        {
            string original = "Krankenhaus";
            var result = Program.SplitWord(dictionary, original);
            Assert.NotEqual("krank en haus ", result.ToLower());
        }

        [Fact]
        public void TestNotInDictionary()
        {
            string original = "123";
            var result = Program.SplitWord(dictionary, original);
            Assert.Equal("123", result.ToLower());
        }

        [Fact]
        public void TestIsEmpty()
        {
            string original = "";
            var result = Program.SplitWord(dictionary, original);
            Assert.Equal("", result.ToLower());
        }

        [Fact]
        public void TestAlreadySplited()
        {
            string original = "Farn Fasan";
            var result = Program.SplitWord(dictionary, original);
            Assert.Equal("farn fasan", result.ToLower());
        }

        [Fact]
        public void TestIsMultiWord()
        {
            string original = "Kranken haus";
            var result = Program.SplitWord(dictionary, original);
            Assert.NotEqual("krank en haus", result.ToLower());
        }
    }
}