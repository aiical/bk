namespace CourseManager.Common.Config.ImagesDPI
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("ImagesDpiGroup")]
    public class ImagesDpiGroup
    {
        [XmlElement(ElementName = "ImagesDpiDetail")]
        public List<ImagesDpiDetail> ImagesDpiDetails { get; set; }
    }
}
