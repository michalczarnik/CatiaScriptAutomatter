using System.IO;
using System.Xml.Serialization;
using CSA.Models;
using CSA.Models.DTO;
using CSA.Services.Interfaces;

namespace CSA.Services.Implementations
{
    class XmlMacroService : IMacroService
    {
        public MacroModel GetMacro(string fileName, string path)
        {
            var scriptDTO = this.GetSerializedData($"{path}\\{fileName}");
            var macroModel = AutoMapper.Mapper.Map<MacroModel>(scriptDTO);
            macroModel.DirectoryName = path;
            return macroModel;
        }

        private Script GetSerializedData(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(Script));
            using(var reader = new StreamReader(path))
            {
                return (Script)xmlSerializer.Deserialize(reader);
            }
        }
    }
}
