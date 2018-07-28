using CSA.Models;
using CSA.Models.Static;
using CSA.Services.Implementations;
using CSA.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

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
        
    }
}
