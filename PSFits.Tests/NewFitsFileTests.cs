using System;
using System.IO;
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
                Path = $"{nameof(TestWhenProvidingANewFileNameAndDataAFitsObjectIsCreated)}_{Guid.NewGuid():D}.fits",
                Data = new int[][] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 } }
            };

            var result = newFitsFile.Invoke<FitsFileHandle>().SingleOrDefault();

            Assert.NotNull(result);
            Assert.Equal(newFitsFile.Path, result.FullName);
            Assert.Equal(new int[] { 2, 3 }, result.Axes);

            result.Handle.Close();

            File.Delete(newFitsFile.Path);
        }
    }
}
