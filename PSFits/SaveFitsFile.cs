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
            if (FitsFile == null)
            {
                return;
            }
            else if (!FitsFile.FileAccess.HasFlag(FileAccess.Write))
            {
                throw new InvalidDataException($"FITS file \"{FitsFile?.FullName}\" is opened read-only");
            }
            else if (FitsFile?.PrimaryHDU?.Rewriteable == true)
            {
                FitsFile.PrimaryHDU.Rewrite();
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
                throw new InvalidDataException($"FITS file \"{FitsFile?.FullName}\" is has no writable file stream");
            }
        }
    }
}
