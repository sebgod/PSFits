using System.IO;
using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsData.Save, "FitsHeader")]
    [OutputType(typeof(FitsFileHandle))]
    public class SaveFitsHeader : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true)]
        public FitsFileHandle FitsFile { get; set; }

        protected override void ProcessRecord()
        {
            if (FitsFile?.PrimaryHDU?.Rewriteable == true)
            {
                FitsFile.PrimaryHDU.Header.Rewrite();
                WriteObject(FitsFile);
            }
            else
            {
                throw new InvalidDataException($"Primary header of FITS file \"{FitsFile?.FullName}\" is not rewritable");
            }
        }
    }
}
