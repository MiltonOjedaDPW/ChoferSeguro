using GenericParsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Caucedo.Base.Common
{
    public static class Utils
    {
        public static Image ScaleImage(string path, int maxWidth, int maxHeight)
        {
            Image image = Image.FromFile(path, true);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path"> Path to which the image would be saved. </param> 
        /// <param name="quality"> An integer from 0 to 100, with 100 being the highest quality. </param> 
        public static void SaveJpeg(string path, int quality = 50)
        {
            if (quality < 0 || quality > 100)
            {
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            var tmpPath = GenTmpPath(path);
            Image img = Image.FromFile(path, true);
            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(tmpPath, jpegCodec, encoderParams);
            File.Copy(tmpPath, path, true);
            File.Delete(tmpPath);
        }

        public static string GenTmpPath(string imgPath)
        {
            var ext = Path.GetExtension(imgPath);
            var path = Path.GetDirectoryName(imgPath);
            var fname = Path.GetFileNameWithoutExtension(imgPath) + "_tmp";
            return Path.Combine(path, fname + ext);
        }
        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }
        public static string ByteArrayToString(Byte[] byteArray)
        {
            return System.Text.Encoding.UTF8.GetString(byteArray);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static ContainerSize GetUnitSize(string ISOType)
        {
            var unitType = GetUnitType(ISOType);
            var size = ISOType[0] == '2' ? 20 :
                       ISOType[0] == '4' ? 40 :
                       ISOType[0] == 'L' ? 45 :
                       ISOType[0] == 'M' ? 48 : 0;
            return (ContainerSize)(((unitType == ISOContainerType.Reefer) ? -1 : 1) * size);
        }
        public static ISOContainerType GetUnitType(string ISOType)
        {
            var map = new Dictionary<char, ISOContainerType>
            {
                {'B', ISOContainerType.Bulk },
                {'G', ISOContainerType.General },
                {'H', ISOContainerType.Insulated },
                {'P', ISOContainerType.Flat},
                {'R', ISOContainerType.Reefer },
                {'S', ISOContainerType.Named },
                {'T', ISOContainerType.Tank},
                {'K', ISOContainerType.Tank},
                {'U', ISOContainerType.OpenTop },
                {'V', ISOContainerType.Ventilated }
            };
            var c = ISOType[ISOType.Length - 2];
            return map.ContainsKey(c) ? map[c] : ISOContainerType.General;
        }
        public static bool ValidateContainerNumber(string equipmentNumber)
        {
            //ISO6346Check
            //Details on algorithm can be found here https://en.wikipedia.org/wiki/ISO_6346#Check_Digit
            //Step 1 
            var numerics = equipmentNumber.Select(c => "0123456789A?BCDEFGHIJK?LMNOPQRSTU?VWXYZ".IndexOf(c));
            //Step 2, Step 3(i)
            var totalStringValue = numerics.Select((n, index) => Math.Pow(2, index) * n).Sum(x => (int)x);
            //Step 3(ii,iii,iv, v)
            var retval = totalStringValue % 11;
            return equipmentNumber[equipmentNumber.Length - 1].ToString() == (retval == 10 ? 0 : retval).ToString();
        }
        internal static string GenMod10ValidNumber(string number)
        {
            var result = number.Substring(0, number.Length - 1);
            result = result + GetLuhnCheckDigit(result);
            return result;
        }
        internal static bool Mod10Check(string number)
        {
            //// check whether input string is null or empty
            if (string.IsNullOrEmpty(number))
            {
                return false;
            }

            //// 1.	Starting with the check digit double the value of every other digit 
            //// 2.	If doubling of a number results in a two digits number, add up
            ///   the digits to get a single digit number. This will results in eight single digit numbers                    
            //// 3. Get the sum of the digits
            int sumOfDigits = number.Where((e) => e >= '0' && e <= '9')
                            .Reverse()
                            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                            .Sum((e) => e / 10 + e % 10);


            //// If the final sum is divisible by 10, then the credit card number
            //   is valid. If it is not divisible by 10, the number is invalid.            
            return sumOfDigits % 10 == 0;
        }
        private static string GetLuhnCheckDigit(string number)
        {
            var sum = 0;
            var alt = true;
            var digits = number.ToCharArray();
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                var curDigit = (digits[i] - 48);
                if (alt)
                {
                    curDigit *= 2;
                    if (curDigit > 9)
                        curDigit -= 9;
                }
                sum += curDigit;
                alt = !alt;
            }
            if ((sum % 10) == 0)
            {
                return "0";
            }
            return (10 - (sum % 10)).ToString();
        }
        public static System.Data.DataTable LoadCSVFile(string file, char ColumnDelimiter = ',', char TextQualifier = '"', int MaxRows = 15000)
        {
            using (GenericParserAdapter parser = new GenericParserAdapter(file))
            {
                parser.SetDataSource(file);
                parser.ColumnDelimiter = ColumnDelimiter;
                parser.FirstRowHasHeader = true;
                //parser.SkipStartingDataRows = 10;
                parser.MaxBufferSize = 8096;
                parser.MaxRows = MaxRows;
                parser.TextQualifier = TextQualifier;
                return parser.GetDataTable();
            }
        }
        public static void SetColumnsOrder(this System.Data.DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
        /// <summary>
        /// Creates a zip file
        /// </summary>
        /// <param name="fnContents">Input functions returning Tuple<NameOfFile,Content></param>
        /// <returns>Zip content</returns>
        public static byte[] CreateZip(params Func<Tuple<string, string>>[] fnContents)
        {
            using (var ms = new MemoryStream())
            {
                using (var ar = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {
                    for (int i = 0; i < fnContents.Length; i++)
                    {
                        var t = fnContents[i]();
                        var file = ar.CreateEntry(t.Item1);
                        using (var entryStream = file.Open())
                        {
                            using (var sw = new StreamWriter(entryStream))
                            {
                                sw.Write(t.Item2);
                            }
                        }
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
