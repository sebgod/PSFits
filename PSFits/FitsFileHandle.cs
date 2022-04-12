using nom.tam.fits;
using nom.tam.util;
using System;
using System.Globalization;
using System.IO;

namespace PSFits
{
    public class FitsFileHandle
    {
        public static string NormalizePath(string path) =>
            Uri.TryCreate(path, UriKind.Absolute, out var uri)
                ? Path.GetFullPath(uri.LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                : path;

        public FitsFileHandle(string path, object data)
        {
            FullName = NormalizePath(path);

            if (!File.Exists(FullName))
            {
                using (var _ = File.Create(FullName))
                {
                    // create emptyt file
                }
            }

            Handle = new Fits(FullName, false, FileAccess = FileAccess.ReadWrite);
            PrimaryHDU = Handle.ReadHDU();
            if (PrimaryHDU is null)
            {
                Handle.AddHDU(PrimaryHDU = FitsFactory.HDUFactory(data));
            }
        }

        public FitsFileHandle(string path, FileAccess fileAccess)
        {
            PrimaryHDU = (Handle = new Fits(FullName = NormalizePath(path), FileAccess = fileAccess)).ReadHDU();

            if (PrimaryHDU is null)
            {
                throw new ArgumentException("File contains no HDU", nameof(path));
            }
        }

        internal Fits Handle { get; }

        public FileAccess FileAccess { get; }

        internal BasicHDU PrimaryHDU { get; }

        public string FullName { get; private set; }

        public string Author
        {
            get => PrimaryHDU.Author;
            set => SetValue("AUTHOR", value);
        }

        public int[] Axes => PrimaryHDU.Axes;

        public string Instrument
        {
            get => PrimaryHDU.Instrument;
            set => SetValue("INSTRUME", value);
        }

        public string Telescope
        {
            get => PrimaryHDU.Telescope;
            set => SetValue("TELESCOP", value);
        }

        public DateTime? ObservationStartDateTime => ReadDate("DATE-OBS");

        public DateTime? ObservationMidDateTime => ReadDate("DATE-AVG");

        public DateTime? ObservationEndDateTime => ReadDate("DATE-END");

        public double ObserverLatitude
        {
            get => ReadDouble("OBSLAT");
            set => SetValue("OBSLAT", value);
        }

        public double ObserverLongitude
        {
            get => ReadDouble("OBSLONG");
            set => SetValue("OBSLONG", value);
        }

        public string Software
        {
            get => PrimaryHDU.GetTrimmedString("SWCREATE");
            set => SetValue("SWCREATE", value);
        }

        public double Equinox
        {
            get => PrimaryHDU.Equinox;
            set => SetValue("EQUINOX", value);
        }

        public string Filter
        {
            get => PrimaryHDU.GetTrimmedString("FILTER");
            set => SetValue("FILTER", value);
        }

        public string FrameType
        {
            get => PrimaryHDU.GetTrimmedString("FRAMETYP");
            set => SetValue("FRAMETYP", value);
        }

        public string Object
        {
            get => PrimaryHDU.Object;
            set => SetValue("OBJECT", value);
        }

        public double RA
        {
            get => ReadDouble("RA");
            set => SetValue("RA", value);
        }

        public double DEC
        {
            get => ReadDouble("DEC");
            set => SetValue("DEC", value);
        }

        public double BScale => PrimaryHDU.BScale;

        public double BZero => PrimaryHDU.BZero;

        public int BitDepth => PrimaryHDU.BitPix;

        public int BlackLevel
        {
            get => ReadInt("BLKLEVEL");
            set => SetValue("BLKLEVEL", value);
        }

        public int Gain
        {
            get => ReadInt("GAIN");
            set => SetValue("GAIN", value);
        }

        public double XPixelSize {
            get => ReadDouble("XPIXSZ"); set => SetValue("XPIXSZ", value);
        }

        public double YPixelSize
        {
            get => ReadDouble("YPIXSZ");
            set => SetValue("YPIXSZ", value);
        }

        public int XBinning
        {
            get => ReadInt("XBINNING", 1);
            set => SetValue("YBINNING", value);
        }

        public int YBinning
        {
            get => ReadInt("YBINNING", 1);
            set => SetValue("YBINNING", value);
        }

        public int FocalLength
        {
            get => ReadInt("FOCALLEN");
            set => SetValue("FOCALLEN", value);
        }

        public TimeSpan? ExposureTime
        {
            get
            {
                var expTimeSec = ReadDouble("EXPTIME");
                return !double.IsNaN(expTimeSec) ? TimeSpan.FromSeconds(expTimeSec) : null as TimeSpan?;
            }

            set => SetValue("EXPTIME", value?.TotalSeconds);
        }

        public double? Temperature
        {
            get => ReadDouble("CCD-TEMP");
            set => SetValue("CCD-TEMP", value);
        }

        #region Helper functions
        double ReadDouble(string prop, double @default = double.NaN) => PrimaryHDU.Header?.GetDoubleValue(prop, @default) ?? @default;

        int ReadInt(string prop, int @default = -1) => PrimaryHDU.Header?.GetIntValue(prop, @default) ?? @default;

        DateTime? ReadDate(string prop) =>
            DateTime.TryParse(PrimaryHDU.GetTrimmedString(prop), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var date)
                ? date
                : null as DateTime?;

        void SetValue(string key, int value)
        {
            var (cursor, comment) = RemoveExistingCard(key);
            cursor.Add(key, new HeaderCard(key, value, comment));
        }

        void SetValue(string key, double value)
        {
            var (cursor, comment) = RemoveExistingCard(key);
            cursor.Add(key, new HeaderCard(key, value, comment));
        }

        void SetValue(string key, string value)
        {
            var (cursor, comment) = RemoveExistingCard(key);
            cursor.Add(key, new HeaderCard(key, value, comment));
        }

        void SetValue(string key, double? maybeValue)
        {
            var (cursor, comment) = RemoveExistingCard(key);

            if (maybeValue.HasValue && !double.IsNaN(maybeValue.Value))
            {
                cursor.Add(key, new HeaderCard(key, maybeValue.Value, comment));
            }
        }

        (Cursor cursor, string comment) RemoveExistingCard(string key)
        {
            var header = PrimaryHDU.Header;
            var card = header.FindCard(key);
            var cursor = header.GetCursor();
            string comment;
            if (card != null)
            {
                comment = card.Comment;
                header.RemoveCard(card);
            }
            else
            {
                comment = null;
            }
            return (cursor, comment);
        }

        #endregion
    }
}
