using nom.tam.util;
using System.IO;
using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsData.Save, "FitsFile")]
    [OutputType(typeof(FitsFileHandle))]
    public class SaveFitsFile : Cmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public FitsFileHandle FitsFile { get; set; }

        protected override void ProcessRecord()
        {
            if (FitsFile?.PrimaryHDU?.Header?.Rewriteable == true)
            {
                FitsFile.PrimaryHDU.Header.Rewrite();
                WriteObject(FitsFile);
            }
            else if (FitsFile?.Handle?.Stream is BufferedFile bf && bf.CanSeek && bf.CanWrite)
            {
                bf.Seek(0);
                bf.SetLength(0);
                FitsFile.Handle.Write(bf);
                WriteObject(FitsFile);
            }
            else
            {
                throw new InvalidDataException($"Primary header of FITS file \"{FitsFile?.FullName}\" is not writable!");
            }
        }
    }
}
