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
            if (FitsFile != null && FitsFile.PrimaryHDU.Rewriteable)
            {
                FitsFile.PrimaryHDU.Rewrite();
                WriteObject(FitsFile);
            }
            else
            {
                throw new InvalidDataException($"Fits file {FitsFile?.FullName} is not rewritable");
            }
        }
    }
}
