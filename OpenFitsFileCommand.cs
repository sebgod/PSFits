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
        public FileAccess FileAccess {get;set;} = FileAccess.Read;

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            var fullName = NormalizePath(Path);
            if (!File.Exists(fullName))
            {
                throw new FileNotFoundException($"Cannot find file {fullName}", fullName);
            }

            var fits = new nom.tam.fits.Fits(fullName, FileAccess);
            fits.ReadHDU();
            WriteObject(new FitsFileHandle {
                FullName = fullName,
                FitsFile = fits
            });
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }

        public static string NormalizePath(string path)
        {
            return System.IO.Path.GetFullPath(new Uri(path).LocalPath)
                    .TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar)
                    .ToUpperInvariant();
        }
    }

    public class FitsFileHandle
    {
        public string FullName { get; internal set; }

        public int Size => FitsFile?.Size() ?? -1;

        public string Author => FitsFile?.ReadHDU()?.Author;

        public int[] Axes => FitsFile?.ReadHDU()?.Axes;

        public string Instrument => FitsFile?.ReadHDU()?.Instrument;

        public string Telescope => FitsFile?.ReadHDU()?.Telescope;

        public DateTime? ObservationDate => FitsFile?.ReadHDU()?.ObservationDate;

        internal nom.tam.fits.Fits FitsFile { get; set; }
    }
}
