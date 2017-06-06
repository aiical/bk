namespace CourseManager.Common.Config.ImagesDPI
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class ImagesDpiDetail
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("desc")]
        public string desc { get; set; }
        /// <summary>
        /// 默认尺寸
        /// </summary>
        [XmlAttribute("def")]
        public string def { get; set; }

        [XmlElement(ElementName = "SectionItem")]
        public List<SectionItem> SectionItems { get; set; }

    }
}
