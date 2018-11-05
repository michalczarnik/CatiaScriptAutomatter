using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSA.Models.DTO.Configuration
{
    [XmlRoot(ElementName = "folder")]
    public class Folder
    {
        [XmlElement(ElementName = "path")]
        public string Path { get; set; }
        [XmlElement(ElementName = "isDefault")]
        public bool IsDefault { get; set; }
    }

    [XmlRoot(ElementName = "folders")]
    public class Folders
    {
        [XmlElement(ElementName = "folder")]
        public List<Folder> Folder { get; set; }
    }

}
