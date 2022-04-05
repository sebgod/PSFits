using System;
using System.IO;
using System.Management.Automation;

namespace PSFits
{
    [Cmdlet(VerbsCommon.Open, "FitsFile")]
    [OutputType(typeof(FitsFileHandle))]
    public class OpenFitsFileCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Path { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipelineByPropertyName = true)]
        public FileAccess FileAccess { get; set; } = FileAccess.Read;

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            if (!File.Exists(Path))
            {
                throw new FileNotFoundException($"Cannot find file {Path}", Path);
            }

            WriteObject(new FitsFileHandle(Path, FileAccess));
        }
    }
}
