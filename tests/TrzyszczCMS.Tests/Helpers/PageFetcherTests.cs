using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrzyszczCMS.Core.Shared.Models;

namespace TrzyszczCMS.Core.Application.Helpers.Tests
{
    [TestFixture()]
    public class PageFetcherTests
    {
        private List<int> _samples;

        [SetUp]
        public void Setup()
        {
            this._samples = new List<int>(Enumerable.Range(0, 9));
        }

        private Task<DataPage<int>> GetFromSamples(int pageNumber)
        {
            var index = pageNumber - 1;
            var pageData = this._samples.Skip(3 * index).Take(3);
            return Task.FromResult(new DataPage<int>()
            {
                PageNumber = pageNumber,
                Entries = pageData.ToList(),
                HasNextPage = pageNumber < 3,
                HasPreviousPage = pageNumber > 1
            });
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void Test_GetCurrent(int currentPageNumber)
        {
            var fetcher = new PageFetcher<int>(currentPageNumber, this.GetFromSamples);
            var task = Task.Run(() => fetcher.GetCurrent());
            task.Wait();
            var values = task.Result;
            Assert.AreEqual(values.PageNumber, currentPageNumber, "Requested page numbers MUST be equal to each other.");
            Assert.AreEqual(fetcher.CurrentPageNumber, currentPageNumber, "Page number in the fetcher MUST be equal to requested one.");

            Assert.IsTrue(values.Entries.ToArray().SequenceEqual(Enumerable.Range((currentPageNumber - 1) * 3, 3).ToArray()),
                "The sequences MUST be equal to each other.");
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Test_GetNext(int currentPageNumber)
        {
            var fetcher = new PageFetcher<int>(currentPageNumber, this.GetFromSamples);
            Task.Run(() => fetcher.GetCurrent()).Wait();
            var task = Task.Run(() => fetcher.GetNext());
            task.Wait();
            var values = task.Result;

            Assert.AreEqual(values.PageNumber, currentPageNumber+1, "Requested page numbers MUST be equal to each other.");
            Assert.AreEqual(fetcher.CurrentPageNumber, currentPageNumber+1, "Page number in the fetcher MUST be equal to requested one.");

            Assert.IsTrue(values.Entries.ToArray().SequenceEqual(Enumerable.Range((currentPageNumber) * 3, 3).ToArray()),
                "The sequences MUST be equal to each other.");
        }

        [TestCase(2)]
        [TestCase(3)]
        public void Test_GetPrevious(int currentPageNumber)
        {
            var fetcher = new PageFetcher<int>(currentPageNumber, this.GetFromSamples);
            Task.Run(() => fetcher.GetCurrent()).Wait();
            var task = Task.Run(() => fetcher.GetPrevious());
            task.Wait();
            var values = task.Result;

            Assert.AreEqual(values.PageNumber, currentPageNumber-1, "Requested page numbers MUST be equal to each other.");
            Assert.AreEqual(fetcher.CurrentPageNumber, currentPageNumber-1, "Page number in the fetcher MUST be equal to requested one.");

            Assert.IsTrue(values.Entries.ToArray().SequenceEqual(Enumerable.Range((currentPageNumber - 2) * 3, 3).ToArray()),
                "The sequences MUST be equal to each other.");
        }

        [Test]
        public void Test_GetNext_Null()
        {
            int currentPageNumber = 3;
            var fetcher = new PageFetcher<int>(currentPageNumber, this.GetFromSamples);
            Task.Run(() => fetcher.GetCurrent()).Wait();
            var task = Task.Run(() => fetcher.GetNext());
            task.Wait();
            var values = task.Result;

            Assert.IsNull(values, "The returned value for getting nonexistent page MUST be null.");
        }

        [Test]
        public void Test_GetPrevious_Null()
        {
            int currentPageNumber = 1;
            var fetcher = new PageFetcher<int>(currentPageNumber, this.GetFromSamples);
            Task.Run(() => fetcher.GetCurrent()).Wait();
            var task = Task.Run(() => fetcher.GetPrevious());
            task.Wait();
            var values = task.Result;

            Assert.IsNull(values, "The returned value for getting nonexistent page MUST be null.");
        }

        [Test]
        public void Test_GetPrevious_ThrowException()
        {
            var fetcher = new PageFetcher<int>(1, this.GetFromSamples);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await fetcher.GetPrevious(),
                "There must be thrown an exception about invoking other method.");
        }

        [Test]
        public void Test_GetNext_ThrowException()
        {
            var fetcher = new PageFetcher<int>(1, this.GetFromSamples);
            Assert.ThrowsAsync<InvalidOperationException>(async () => await fetcher.GetNext(),
                "There must be thrown an exception about invoking other method.");
        }
    }
}