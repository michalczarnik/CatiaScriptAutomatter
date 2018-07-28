using System.IO;
using System.Text.RegularExpressions;
using CSA.Helpers;
using CSA.Models;
using CSA.Services.Interfaces;

namespace CSA.Services.Implementations
{
    class CommentMacroService : IMacroService
    {
        public MacroModel GetMacro(string fileName, string path)
        {
            var scriptRegex = new Regex(Constants.MacroScriptPattern);
            var model = new MacroModel();
            model.FileName = fileName;
            model.DirectoryName = path;
            using (var sr = new StringReader(System.IO.File.ReadAllText($"{path}/{fileName}")))
            {
                string line;
                var foundSubmethod = false;
                while ((line = sr.ReadLine()) != null)
                {
                    if (!foundSubmethod)
                    {
                        var scriptMatch = scriptRegex.Match(line);
                        if (scriptMatch.Success && scriptMatch.Groups.Count > 1 && !string.IsNullOrWhiteSpace(scriptMatch.Groups[1].Value))
                        {
                            var parameters = scriptMatch.Groups[1].Value.Split(',');
                            foreach (var parameter in parameters)
                            {
                                var temporaryParameter = parameter.Trim();
                                var parameterDesc = temporaryParameter.Split(' ');

                                var parameterModel = new ParameterModel
                                {
                                    ParameterName = parameterDesc[0],
                                    Type = parameterDesc[2]
                                };

                                model.ParameterList.Add(parameterModel);
                                foundSubmethod = true;
                            }
                        }
                    }

                    if (line.StartsWith("'"))
                    {
                        if (line.StartsWith(Constants.ParameterNames.Description))
                        {
                            model.Description = line.Substring(Constants.ParameterNames.Description.Length);
                        }
                        else if (line.StartsWith(Constants.ParameterNames.Image))
                        {
                            var splittedLine = line.Split(';');
                            model.Images.AddOrReplace(splittedLine[1], splittedLine[2]);
                        }
                        else
                        {
                            foreach (var parameter in model.ParameterList)
                            {
                                if (line.StartsWith($"'{parameter.ParameterName}:"))
                                {
                                    var parameterInfo = line.Substring(parameter.ParameterName.Length + 2).Split(';');
                                    if (parameterInfo.Length > 0)
                                    {
                                        parameter.DisplayName = parameterInfo[0];
                                        if (parameterInfo.Length > 1)
                                        {
                                            parameter.DefaultValue = parameterInfo[1];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            model.UniqueID = UniqueIdentifierHelper.GenerateUniqueID();
            return model;
        }
    }
}
