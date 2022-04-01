using System;
using System.IO;

namespace PSFits
{
    public class FitsFileHandle
    {
        public FitsFileHandle(string fullName, FileAccess fileAccess)
        {
            PrimaryHDU = (FitsFile = new nom.tam.fits.Fits(FullName = fullName, fileAccess)).ReadHDU();
        }

        internal nom.tam.fits.Fits FitsFile { get; }

        internal nom.tam.fits.BasicHDU PrimaryHDU { get; }

        public long Size => PrimaryHDU.Size;

        public string FullName { get; }

        public string Author => PrimaryHDU.Author;

        public int[] Axes => PrimaryHDU.Axes;

        public string Instrument => PrimaryHDU.Instrument;

        public string Telescope => PrimaryHDU.Telescope;

        public DateTime CreationDate => PrimaryHDU.CreationDate;

        public DateTime ObservationDate => PrimaryHDU.ObservationDate;

        public double? ObserverLatitude => ReadDouble("OBSLAT");

        public double? ObserverLongitude => ReadDouble("OBSLONG");

        public string Software => PrimaryHDU.GetTrimmedString("SWCREATE");

        public double Equinox => PrimaryHDU.Equinox;

        public string Filter => PrimaryHDU.GetTrimmedString("FILTER");

        public string FrameType => PrimaryHDU.GetTrimmedString("FRAMETYP");

        public string Object => PrimaryHDU.Object;

        public double? RA => ReadDouble("RA");

        public double? DEC => ReadDouble("DEC");

        public double BScale => PrimaryHDU.BScale;

        public double BZero => PrimaryHDU.BZero;

        public int BitDepth => PrimaryHDU.BitPix;

        public int? BlackLevel => ReadInt("BLKLEVEL");

        public int? Gain => ReadInt("GAIN");

        public double? XPixelSize => ReadDouble("XPIXSZ");

        public double? YPixelSize => ReadDouble("YPIXSZ");

        public int XBinning => ReadInt("XBINNING") ?? 1;

        public int YBinning => ReadInt("YBINNING") ?? 1;

        public int? FocalLength => ReadInt("FOCALLEN");

        public TimeSpan? ExposureTime => double.TryParse(PrimaryHDU.GetTrimmedString("EXPTIME"), out var expSec) ? TimeSpan.FromSeconds(expSec) : null as TimeSpan?;

        public double? Temperature => ReadDouble("CCD-TEMP");

        double? ReadDouble(string prop) => double.TryParse(PrimaryHDU.GetTrimmedString(prop), out var blkLevel) ? blkLevel : null as double?;

        int? ReadInt(string prop) => int.TryParse(PrimaryHDU.GetTrimmedString(prop), out var len) ? len : null as int?;
    }
}
