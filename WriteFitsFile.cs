using nom.tam.util;
using System.IO;
using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsCommunications.Write, "FitsFile")]
    [OutputType(typeof(FitsFileHandle))]
    public class WriteFitsFile : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true)]
        public FitsFileHandle FitsFile { get; set; }

        protected override void ProcessRecord()
        {
            if (FitsFile?.PrimaryHDU.Rewriteable == true)
            {
                FitsFile.PrimaryHDU.Rewrite();
                WriteObject(FitsFile);
            }
            else if (string.IsNullOrWhiteSpace(FitsFile?.FullName))
            {
                throw new InvalidDataException($"FITS file path is empty");
            }
            else
            {
                using (FileStream fileStream = File.Create(FitsFile?.FullName))
                using (BufferedDataStream os = new BufferedDataStream(fileStream))
                {
                    FitsFile.Handle.Write(os);
                }
            }
        }
    }
}
