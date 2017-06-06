namespace CourseManager.Common.Config.ImagesDPI
{
    using System.Xml.Serialization;

    public class SectionItem
    {
        [XmlAttribute("sortno")]
        public int sortno { get; set; }

        [XmlAttribute("width")]
        public int width { get; set; }

        [XmlAttribute("height")]
        public int height { get; set; }

        [XmlAttribute("dpi")]
        public string dpi { get; set; }

    }
}
