using System.Text;
using System.Xml;
using System.Xml.Serialization;
namespace Ts.Application.HelperExtensions
{
    public static class XmlExtension
    {
        public static string ToFormatXml(this XmlNode xmlNode)
        {
            var stringBuilder = new StringBuilder();
            using var stringWriter = new StringWriter(stringBuilder);
            using var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            xmlNode.WriteTo(xmlTextWriter);
            return stringBuilder.ToString();
        }

        public static string SerializeToXml<T>(this T value)
        {
            var serializer = new XmlSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                Encoding = new UTF8Encoding(false) // Set encoding to UTF-8 without BOM
            };

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, "http://www.sitemaps.org/schemas/sitemap/0.9");

            using var memoryStream = new MemoryStream();
            using var xmlWriter = XmlWriter.Create(memoryStream, settings);
            serializer.Serialize(xmlWriter, value, namespaces);
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}
