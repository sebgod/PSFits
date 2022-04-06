using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsCommon.New, "FitsFile")]
    [OutputType(typeof(FitsFileHandle))]
    public class NewFitsFile : Cmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Path { get; set; }

        [Parameter(
            Mandatory = true,
            Position = 1,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true)]
        public object Data { get; set; }

        protected override void ProcessRecord()
        {
            if (Path != null && Data != null)
            {
                var fitsFile = new FitsFileHandle(Path, Data);
                WriteObject(fitsFile);
            }
        }
    }
}
