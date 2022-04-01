using System;
using System.Globalization;
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

        public DateTime? ObservationStartDateTime => ReadDate("DATE-OBS");

        public DateTime? ObservationMidDateTime => ReadDate("DATE-AVG");

        public DateTime? ObservationEndDateTime => ReadDate("DATE-END");

        public double ObserverLatitude => ReadDouble("OBSLAT");

        public double ObserverLongitude => ReadDouble("OBSLONG");

        public string Software => PrimaryHDU.GetTrimmedString("SWCREATE");

        public double Equinox => PrimaryHDU.Equinox;

        public string Filter => PrimaryHDU.GetTrimmedString("FILTER");

        public string FrameType => PrimaryHDU.GetTrimmedString("FRAMETYP");

        public string Object => PrimaryHDU.Object;

        public double RA => ReadDouble("RA");

        public double DEC => ReadDouble("DEC");

        public double BScale => PrimaryHDU.BScale;

        public double BZero => PrimaryHDU.BZero;

        public int BitDepth => PrimaryHDU.BitPix;

        public int BlackLevel => ReadInt("BLKLEVEL");

        public int Gain => ReadInt("GAIN");

        public double XPixelSize => ReadDouble("XPIXSZ");

        public double YPixelSize => ReadDouble("YPIXSZ");

        public int XBinning => ReadInt("XBINNING", 1);

        public int YBinning => ReadInt("YBINNING", 1);

        public int FocalLength => ReadInt("FOCALLEN");

        public TimeSpan? ExposureTime
        {
            get
            {
                var expTimeSec = ReadDouble("EXPTIME");
                return !double.IsNaN(expTimeSec) ? TimeSpan.FromSeconds(expTimeSec) : null as TimeSpan?;
            }
        }

        public double? Temperature => ReadDouble("CCD-TEMP");

        #region Helper functions
        double ReadDouble(string prop, double @default = double.NaN) => PrimaryHDU.Header?.GetDoubleValue(prop, @default) ?? @default;

        int ReadInt(string prop, int @default = -1) => PrimaryHDU.Header?.GetIntValue(prop, @default) ?? @default;

        DateTime? ReadDate(string prop) =>
            DateTime.TryParse(PrimaryHDU.GetTrimmedString(prop), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date)
                ? date
                : null as DateTime?;
        #endregion
    }
}
