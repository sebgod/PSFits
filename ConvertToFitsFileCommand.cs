using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsData.ConvertTo, "FitsFile")]
    public class ConvertToFitsFileCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public int[,] ImageArray { get; set; }

        protected override void ProcessRecord()
        {
            if (ImageArray != null)
            {

            }
        }
    }
}
