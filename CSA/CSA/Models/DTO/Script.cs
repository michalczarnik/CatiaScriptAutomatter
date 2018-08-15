using System.Collections.Generic;
using System.Xml.Serialization;

namespace CSA.Models.DTO
{
    [XmlRoot(ElementName = "parameter")]
    public class Parameter
    {
        [XmlElement(ElementName = "parameterScriptName")]
        public string ParameterScriptName { get; set; }
        [XmlElement(ElementName = "parameterDisplayName")]
        public string ParameterDisplayName { get; set; }
        [XmlElement(ElementName = "parameterType")]
        public string ParameterType { get; set; }
        [XmlElement(ElementName = "parameterDefaultValue")]
        public string ParameterDefaultValue { get; set; }
        [XmlElement(ElementName = "parameterIsSharedInContext")]
        public bool ParameterIsSharedInContext { get; set; }
    }

    [XmlRoot(ElementName = "parameters")]
    public class Parameters
    {
        [XmlElement(ElementName = "parameter")]
        public List<Parameter> Parameter { get; set; }
    }

    [XmlRoot(ElementName = "image")]
    public class Image
    {
        [XmlElement(ElementName = "imageName")]
        public string ImageName { get; set; }
        [XmlElement(ElementName = "imageText")]
        public string ImageText { get; set; }
    }

    [XmlRoot(ElementName = "images")]
    public class Images
    {
        [XmlElement(ElementName = "image")]
        public List<Image> Image { get; set; }
    }

    [XmlRoot(ElementName = "warning")]
    public class Warning
    {
        [XmlElement(ElementName = "warningImage")]
        public string WarningImage { get; set; }
        [XmlElement(ElementName = "warningText")]
        public string WarningText { get; set; }
    }

    [XmlRoot(ElementName = "warnings")]
    public class Warnings
    {
        [XmlElement(ElementName = "warningIsAfterMacro")]
        public string WarningIsAfterMacro { get; set; }
        [XmlElement(ElementName = "warning")]
        public List<Warning> Warning { get; set; }
    }

    [XmlRoot(ElementName = "script")]
    public class Script
    {
        [XmlElement(ElementName = "scriptName")]
        public string ScriptName { get; set; }
        [XmlElement(ElementName = "scriptDescription")]
        public string ScriptDescription { get; set; }
        [XmlElement(ElementName = "parameters")]
        public Parameters Parameters { get; set; }
        [XmlElement(ElementName = "images")]
        public Images Images { get; set; }
        [XmlElement(ElementName = "warnings")]
        public Warnings Warnings { get; set; }
    }

}
