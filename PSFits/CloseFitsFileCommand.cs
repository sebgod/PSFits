using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsCommon.Close, "FitsFile")]
    public class CloseFitsFileCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public FitsFileHandle FitsFile { get; set; }

        protected override void ProcessRecord()
        {
            if (FitsFile != null)
            {
                FitsFile.Handle.Close();
            }
        }
    }
}
