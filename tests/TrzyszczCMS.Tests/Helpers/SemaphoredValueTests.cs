using NUnit.Framework;
using System;

namespace TrzyszczCMS.Core.Server.Helpers.Tests
{
    [TestFixture()]
    public class SemaphoredValueTests
    {
        private SemaphoredValue<string> _testedValue;

        [SetUp]
        public void Setup()
        {
            this._testedValue = new SemaphoredValue<string>(() => " random ");
        }

        [Test]
        public void Test_SemaphoredValue_SynchronisationRequirement()
        {
            Assert.Throws<InvalidOperationException>(
                () => this._testedValue.SetValue("test"),
                "Usage of value without synchronising MUST throw an exception."
            );

            Assert.Throws<InvalidOperationException>(
                () => { var x = this._testedValue.Invoke(i => i.Trim()); },
                "Usage of value without synchronising MUST throw an exception."
            );

            Assert.Throws<InvalidOperationException>(
                () => this._testedValue.Invoke(i => { var x = i.Trim(); }),
                "Usage of value without synchronising MUST throw an exception."
            );
        }

        [Test]
        public void Test_SemaphoredValue_ProperUsage()
        {
            this._testedValue.Synchronise(i => this._testedValue.SetValue("abcd"));
            this._testedValue.Synchronise(i =>
                Assert.AreEqual("abcd", i.Invoke(t => t), "The values MUST be equal to each other.")
            );
            this._testedValue.Synchronise(i =>
                Assert.AreEqual("abcd", i.Invoke(t => t), "The values MUST be equal to each other.")
            );
        }
    }
}