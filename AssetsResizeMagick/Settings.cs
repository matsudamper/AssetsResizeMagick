using System.Collections.Generic;
using System.Xml.Serialization;

namespace AssetsResizeMagick
{
    [XmlRoot("Types")]
    public class Settings
    {
        [XmlElement("Type")]
        public List<Type> types { get; set; }
    }

    public class Type
    {
        [XmlAttribute("name")]
        public string name { get; set; }

        [XmlAttribute("folderName")]
        public string folderName { get; set; }


        [XmlElement("Image")]
        public List<Image> images { get; set; }
    }

    public class Image
    {
        [XmlAttribute("x")]
        public int x { get; set; }

        [XmlAttribute("y")]
        public int y { get; set; }

        [XmlAttribute("folderName")]
        public string folderName { get; set; }

        [XmlText]
        public string filename { get; set; }
    }
}
