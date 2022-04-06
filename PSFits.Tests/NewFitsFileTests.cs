using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PSFits.Tests
{
    public class NewFitsFileTests
    {
        [Fact]
        public void TestWhenProvidingANewFileNameAndDataAFitsObjectIsCreated()
        {
            var newFitsFile = new NewFitsFile
            {
                Path = $"{Guid.NewGuid():D}.fits",
                Data = new int[][] { new[] { 1, 2 }, new[] { 3, 4 } }
            };


            var result = newFitsFile.Invoke<FitsFileHandle>().SingleOrDefault();

            Assert.NotNull(result);
            Assert.Equal(newFitsFile.Path, result.FullName);
            Assert.Equal(new int[] { 2, 2 }, result.Axes);
        }
    }
}
