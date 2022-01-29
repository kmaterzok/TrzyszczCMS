using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TrzyszczCMS.Client.Helpers.Extensions.Tests
{
    [TestFixture]
    public class ListExtensionsTests
    {
        private List<string> _samples;

        [SetUp]
        public void Setup() =>
            this._samples = new List<string>(Enumerable.Range(1, 5).Select(i => i.ToString()));

        [TestCase("3", true,  "1 3 2 4 5")]
        [TestCase("3", false, "1 2 4 3 5")]
        [TestCase("1", true,  "5 2 3 4 1")]
        [TestCase("5", false, "5 2 3 4 1")]
        public void Test_MoveItem(string item, bool moveUp, string expectedSequence)
        {
            this._samples.MoveItem(item, moveUp);
            var expectedArray = expectedSequence.Split(' ').ToArray();
            Assert.IsTrue(this._samples.ToArray().SequenceEqual(expectedArray), "The sequences after moving MUST be equal to each other.");
        }

        [Test]
        public void Test_MoveItem_NotThrowingExceptions()
        {
            var oneElementList = new List<string>() { "1" };
            Assert.DoesNotThrow(() => this._samples.MoveItem("1", true),  "The moving operation MUST NOT throw an exception (move up).");
            Assert.DoesNotThrow(() => this._samples.MoveItem("1", false), "The moving operation MUST NOT throw an exception (move down).");
        }

        [TestCase(1, 4, "1 5 3 4 2")]
        [TestCase(2, 3, "1 2 4 3 5")]
        [TestCase(0, 0, "1 2 3 4 5")]
        public void Test_SwapItemsByIndex(int firstIndex, int secondIndex, string expectedSequence)
        {
            this._samples.SwapItemsByIndex(firstIndex, secondIndex);
            var expectedArray = expectedSequence.Split(' ').ToArray();
            Assert.IsTrue(this._samples.ToArray().SequenceEqual(expectedArray), "The sequences after swapping MUST be equal to each other.");
        }
    }
}