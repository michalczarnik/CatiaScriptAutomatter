using CSA.Models;
using CSA.Models.DTO.Configuration;
using CSA.Models.Static;
using CSA.Services.Implementations;
using CSA.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace CSA.Services.Static
{
    static class FileService
    {
        private static IMacroService _macroService;       

        public static void PopulateMacros(string pathToFolder, bool isXml=true)
        {
            _macroService = isXml ? (IMacroService) new XmlMacroService() : (IMacroService) new CommentMacroService();
            var regexPattern = isXml ? Constants.MacroFilePatternXml : Constants.MacroFilePatternScript;
            StaticMacroList.ListOfMacros = new List<MacroModel>();
            if (!Directory.Exists(pathToFolder))
                return;
            var files = Directory.GetFiles(pathToFolder);
            var fileRegex = new Regex(regexPattern);
            foreach (var file in files)
            {
                var fileMatch = fileRegex.Match(file);
                if (fileMatch.Success)
                {
                    var model = _macroService.GetMacro(fileMatch.Value, pathToFolder);
                    StaticMacroList.ListOfMacros.Add(model);
                }
            }
        }

        public static Folders GetRepositories()
        {
            var xmlSerializer = new XmlSerializer(typeof(Folders));
            if (File.Exists(Constants.RepositoriesPath))
            {
                using (var reader = new StreamReader(Constants.RepositoriesPath))
                {
                    return (Folders)xmlSerializer.Deserialize(reader);
                }

            }
            using (var writer = new StreamWriter(Constants.RepositoriesPath))
            {
                xmlSerializer.Serialize(writer, new Folders());
            }
            return new Folders();
        }

        public static bool AddRepository(string path, bool isDefault)
        {
            var folders = FileService.GetRepositories();
            if (folders.Folder == null)
                folders.Folder = new List<Folder>();
            if (folders.Folder.Any(_ => _.Path.Equals(path, System.StringComparison.InvariantCultureIgnoreCase)))
                return false;
            var xmlSerializer = new XmlSerializer(typeof(Folders));
            var folder = new Folder
            {
                IsDefault = isDefault,
                Path = path
            };
            if (folders.Folder.Any(_ => _.IsDefault) && isDefault)
            {
                folders.Folder = folders.Folder.Select(_ => { _.IsDefault = false; return _; }).ToList();
            }
            folders.Folder.Add(folder);
            using(var writer = new StreamWriter(Constants.RepositoriesPath))
            {
                xmlSerializer.Serialize(writer, folders);
            }
            return true;
        }

        public static void DeleteRepository(string path)                                                                                                    
        {                                                                                        
            var folders = FileService.GetRepositories();                   
            if (folders.Folder == null)
                return;
            folders.Folder = folders.Folder.Where(_ => !_.Path.Equals(path, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
            var xmlSerializer = new XmlSerializer(typeof(Folders));       
            using (var writer = new StreamWriter(Constants.RepositoriesPath))
            {
                xmlSerializer.Serialize(writer, folders);
            }
        }

        public static void ChangeToDefault(string path)
        {
            var folders = FileService.GetRepositories();
            if (folders.Folder == null)
                folders.Folder = new List<Folder>();
            var xmlSerializer = new XmlSerializer(typeof(Folders));
            folders.Folder = folders.Folder.Select(_ => {
                if (_.Path.Equals(path, System.StringComparison.InvariantCultureIgnoreCase))
                    _.IsDefault = true;
                else
                    _.IsDefault = false;
                return _;
            }).ToList();
            using (var writer = new StreamWriter(Constants.RepositoriesPath))
                xmlSerializer.Serialize(writer, folders);
            
        }

    }
}
