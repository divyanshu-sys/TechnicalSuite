using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Spire.Doc;
using System.Globalization;
namespace Ts.Application.Helpers
{
    public static class OfficeHelper
    {
        private static readonly string[] DateTimeFormats = {
            "dd-MM-yyyy",
            "dd-MM-yyyy hh:mm:ss tt",
            "dd-MM-yyyy hh:mm:ss",
            "dd-MMM-yy hh:mm:ss",
            "dd-MMM-yyyy hh:mm:ss",
            "dd-MMM-yy h:mm tt",
            "dd-MMM-yyyy h:mm tt",
            "MM-dd-yyyy",
            "M-d-yyyy",
            "M-dd-yyyy",
            "MM-d-yyyy",
            "M-d-yyyy h:mm:ss tt",
            "MM-d-yyyy h:mm:ss tt",
            "M-d-yyyy h:mm tt",
            "MM-dd-yyyy hh:mm:ss",
            "M-d-yyyy h:mm:ss",
            "M-d-yyyy hh:mm tt",
            "M-d-yyyy hh tt",
            "M-d-yyyy h:mm",
            "M-d-yyyy h:mm",
            "MM-dd-yyyy hh:mm",
            "M-dd-yyyy hh:mm",
            "yyyy-MM-dd",
            "yyyy-MM-ddTHH:mm",
            "MM/dd/yyyy",
            "M/d/yyyy",
            "M/dd/yyyy",
            "MM/d/yyyy",
            "M/d/yyyy h:mm:ss tt",
            "MM/d/yyyy h:mm:ss tt",
            "M/d/yyyy h:mm tt",
            "MM/dd/yyyy hh:mm:ss",
            "M/d/yyyy h:mm:ss",
            "M/d/yyyy hh:mm tt",
            "M/d/yyyy hh tt",
            "M/d/yyyy h:mm",
            "M/d/yyyy h:mm",
            "MM/dd/yyyy hh:mm",
            "M/dd/yyyy hh:mm",
            "d/M/yyyy hh:mm:ss tt",
            "d/MM/yyyy hh:mm:ss tt",
            "dd/MM/yyyy hh:mm:ss tt",
            "dd/MM/yyyy hh:mm:ss",
            "dd/MMM/yy hh:mm:ss",
            "dd/MMM/yyyy hh:mm:ss",
            "dd/MMM/yy h:mm tt",
            "dd/MMM/yyyy hh:mm tt",
            "yyyy/MM/ddTHH:mm"
        };

        public static void ReplaceTextAndImages(Dictionary<string, string> textDictionary, string relativepath, string sourceName, string destinationName)
        {
            var sourceFullPath = System.IO.Path.Combine(relativepath, sourceName);
            var destinationFullPath = System.IO.Path.Combine(relativepath, $"{destinationName}.docx");

            // Create a copy of the template file and open the copy
            File.Copy(sourceFullPath, destinationFullPath, true);

            using WordprocessingDocument doc = WordprocessingDocument.Open(destinationFullPath, true);

            //Replace text elements
            ReplaceTextElement(doc, textDictionary);
            //Replace image placeholder with QR Code
            if (textDictionary.ContainsKey("RWQRCode"))
                ReplaceInternalImage(doc, "RWQRCode.jpg", Convert.FromBase64String(textDictionary["RWQRCode"]));
            doc.Close();
        }

        private static void ReplaceTextElement(WordprocessingDocument doc, Dictionary<string, string> textDictionary)
        {
            var body = doc.MainDocumentPart.Document.Body;
            var paras = body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>();
            foreach (var para in paras)
            {
                foreach (var run in para.Elements<DocumentFormat.OpenXml.Wordprocessing.Run>())
                {
                    foreach (var text in run.Elements<DocumentFormat.OpenXml.Wordprocessing.Text>())
                    {
                        foreach (var item in textDictionary)
                        {
                            if (text.Text == item.Key)
                                text.Text = text.Text.Replace(item.Key, item.Value);
                        }
                    }
                }
            }
        }

        private static void ReplaceInternalImage(WordprocessingDocument document, string oldImagesPlaceholderText, byte[] newImageBytes)
        {
            IEnumerable<Drawing> drawings = document.MainDocumentPart.Document.Descendants<Drawing>();
            foreach (Drawing drawing in drawings)
            {
                DocProperties dpr = drawing.Descendants<DocProperties>().FirstOrDefault();
                if (dpr != null && dpr.Name == oldImagesPlaceholderText)
                {
                    foreach (Blip b in drawing.Descendants<Blip>())
                    {
                        OpenXmlPart imagePart = document.MainDocumentPart.GetPartById(b.Embed);
                        using var writer = new BinaryWriter(imagePart.GetStream());
                        writer.Write(newImageBytes);
                    }
                }
            }
        }

        public static string ConvertWordToPdf(string relativepath, string destinationName)
        {
            var WordFullPath = System.IO.Path.Combine(relativepath, $"{destinationName}.docx");
            var PdfFullPath = System.IO.Path.Combine(relativepath, $"{destinationName}.pdf");
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(WordFullPath);
            document.SaveToFile(PdfFullPath, FileFormat.PDF);
            document.Close();
            return PdfFullPath;
        }

        public static void SetHeaders(IXLWorksheet sheet, List<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                sheet.Cell(1, i + 1).Value = headers[i];
                sheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.FromTheme(XLThemeColor.Accent1);
                sheet.Cell(1, i + 1).Style.Font.FontColor = XLColor.FromTheme(XLThemeColor.Background1);
                sheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

        public static bool IsBetween<T>(this T item, T start, T end, bool includeStart = false, bool includeEnd = false)
        {
            if (includeEnd && includeStart)
                return Comparer<T>.Default.Compare(item, start) >= 0
                       && Comparer<T>.Default.Compare(item, end) <= 0;
            if (includeStart)
                return Comparer<T>.Default.Compare(item, start) >= 0
                    && Comparer<T>.Default.Compare(item, end) < 0;
            if (includeEnd)
                return Comparer<T>.Default.Compare(item, start) > 0
                       && Comparer<T>.Default.Compare(item, end) <= 0;
            return Comparer<T>.Default.Compare(item, start) > 0
                   && Comparer<T>.Default.Compare(item, end) < 0;
        }

        public static DateTime GetDateFromString(string date)
        {
            return DateTime.ParseExact(date, DateTimeFormats, CultureInfo.InvariantCulture);
        }

        public static DateTime GetTimeFromString(string time)
        {
            return DateTime.ParseExact(time, new[] { "h:mm tt", "hh:mm tt", "HH:mm", "H:mm" }, CultureInfo.InvariantCulture);
        }

        public static DateTime GetDateTimeFromString(string date, string time)
        {
            var Date = DateTime.ParseExact(date, DateTimeFormats, CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(time)) return Date;
            var Time = DateTime.ParseExact(time, new[] { "h:mm tt", "hh:mm tt", "HH:mm", "H:mm" }, CultureInfo.InvariantCulture);
            Date = Date.Add(Time.TimeOfDay);
            return Date;
        }

        public static DateTime GetDateTimeFromString(DateTime date, string time)
        {
            if (string.IsNullOrEmpty(time)) return date;
            var Time = DateTime.ParseExact(time, new[] { "h:mm tt", "hh:mm tt", "HH:mm", "H:mm" }, CultureInfo.InvariantCulture);
            date = date.Add(Time.TimeOfDay);
            return date;
        }
    }
}
