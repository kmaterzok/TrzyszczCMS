using NUnit.Framework;
using System.Collections.Generic;

namespace Core.Shared.Helpers.Extensions.Tests
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [TestCase("a", 1, "b", 1)]
        public void Test_AddOrUpdate_Adding(object key, object value, object addedKey, object addedValue)
        {
            var dictionary = new Dictionary<object, object>()
            {
                { key, value }
            };
            dictionary.AddOrUpdate(addedKey, addedValue);
            Assert.AreEqual(dictionary.Count, 2, "The MUST be 2 elements.");

            Assert.AreEqual(dictionary[key],      value,      "The value under _key_ is defferent.");
            Assert.AreEqual(dictionary[addedKey], addedValue, "The value under _addedKey_ is defferent.");
        }

        [TestCase("a", 1, "a", 2)]
        public void Test_AddOrUpdate_Updating(object key, object value, object addedKey, object addedValue)
        {
            var dictionary = new Dictionary<object, object>()
            {
                { key, value }
            };
            dictionary.AddOrUpdate(addedKey, addedValue);
            Assert.AreEqual(dictionary.Count, 1, "There MUST be 1 element.");

            Assert.AreEqual(dictionary[key],      addedValue     , "The value under _key_ is defferent.");
            Assert.AreEqual(dictionary[addedKey], addedValue, "The value under _addedKey_ is defferent.");
        }
    }
}