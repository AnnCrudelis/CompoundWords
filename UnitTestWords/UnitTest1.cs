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
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.Equal("Psychologie", result);
        }

        [Fact]
        public void TestAllowedToSplit()
        {
            string original = "Krankenhaus";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.Equal("Kranken haus", result);
        }

        [Fact]
        public void TestWrongAllowedToSplit()
        {
            string original = "Krankenhaus";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.NotEqual("Krank en haus ", result);
        }

        [Fact]
        public void TestNotInDictionary()
        {
            string original = "123";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.Equal("123 ", result);
        }

        [Fact]
        public void TestIsEmpty()
        {
            string original = "";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.Equal("", result);
        }

        [Fact]
        public void TestAlreadySplited()
        {
            string original = "Farn Fasan";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.Equal("Farn Fasan", result);
        }

        [Fact]
        public void TestIsMultiWord()
        {
            string original = "Kranken haus";
            var result = Program.SomeAlgoritm(dictionary, original);
            Assert.NotEqual("Krank en haus", result);
        }
    }
}