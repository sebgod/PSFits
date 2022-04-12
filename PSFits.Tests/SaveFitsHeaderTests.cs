using nom.tam.util;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PSFits.Tests
{
    public class SaveFitsHeaderTests
    {
        [Fact]
        public void GivenChangedHeadersWhenSavingHeadersAreUpdated()
        {
            var fileName = $"{nameof(GivenChangedHeadersWhenSavingHeadersAreUpdated)}_{Guid.NewGuid():D}.fits";
            var data = new int[][] { new[] { 1, 2 }, new[] { 3, 4 } };
            var fullPath = Path.Combine(Path.GetTempPath(), fileName);

            var fileHandle = new FitsFileHandle(fullPath, data)
            {
                Author = "Me"
            };

            var saveFitsHeader = new SaveFitsFile
            {
                FitsFile = fileHandle
            };

            var result = saveFitsHeader.Invoke<FitsFileHandle>().FirstOrDefault();

            Assert.NotNull(result);

            result.Handle.Close();

            File.Delete(fileHandle.FullName);
        }
    }
}
