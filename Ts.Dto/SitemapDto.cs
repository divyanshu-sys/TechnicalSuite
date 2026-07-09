using System.Xml.Serialization;
namespace Ts.Dto
{
    [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public class SitemapDto
    {
        [XmlElement("url")]
        public List<UrlData> Urls { get; set; }
    }

    public class UrlData
    {
        [XmlElement("loc")]
        public string Loc { get; set; }

        [XmlElement("lastmod")]
        public string LastMod { get; set; }

        [XmlElement("changefreq")]
        public string ChangeFreq { get; set; }

        [XmlElement("priority")]
        public string Priority { get; set; }
    }
}
