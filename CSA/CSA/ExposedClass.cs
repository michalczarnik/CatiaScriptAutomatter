﻿using CefSharp.WinForms;
using CSA.Forms;
using CefSharp;
using CSA.Models;
using System.Collections.Generic;
using CSA.Services.Static;
using Newtonsoft.Json;
using System.Linq;
using System;
using CSA.Helpers;
using CSA.Models.Static;

namespace CSA
{
    class ExposedClass
    {
        private static ChromiumWebBrowser _browser = null;
        private static BrowserForm _form = null;

        public ExposedClass(ChromiumWebBrowser browser, BrowserForm form)
        {
            //Add asserts
            _browser = browser;
            _form = form;
        }

        public bool IsCatiaStarted()
        {
            if (!CatiaService.CatiaExists())
            {
                return CatiaService.InitializeCatia();
            }
            return CatiaService.CatiaExists();
        }

        public string loadMacros(string path,bool isXml = true)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;
            FileService.PopulateMacros(path, isXml);
            return JsonConvert.SerializeObject(StaticMacroList.ListOfMacros);
        }

        public string openFile()
        {
            return WinFormsService.OpenFileDialog();
        }

        public string getFolderPath()
        {
            WinFormsService.OpenFolderDialog();
            return WinFormsService.PathToDirectory;
        }

        public string runMacro(string responseModel)
        {
            var response = JsonConvert.DeserializeObject<ResponseModel>(responseModel);
            Guid macroID;
            if(Guid.TryParse(response.UniqueID, out macroID)){
                var foundMacro = StaticMacroList.ListOfMacros.FirstOrDefault(_ => _.UniqueID.Equals(macroID));
                if (foundMacro != null)
                {
                    var parameterModel = new List<object>();
                    foundMacro.ParameterList.ForEach(_ =>
                    {
                        var foundParameter = response.ParameterList.FirstOrDefault(p => p.ParamterName.Equals(_.ParameterName));
                        if (foundParameter == null)
                            parameterModel.Add(_.DefaultValue);
                        else
                            parameterModel.Add(foundParameter.ParameterValue.ParseToType(_.Type));
                    });

                    return CatiaService.RunMacro(foundMacro.DirectoryName, foundMacro.FileName, parameterModel);
                }
            }
            return $"Macro ID is in incorrect format! MacroID: {response.UniqueID}";
        }

        public void showDevTools()
        {
            _browser.ShowDevTools();
        }

        public void LargeWindow()
        {
            WinFormsService.ChangeSize(Constants.ScreenSize.L);
        }

        public void MediumWindow()
        {
            WinFormsService.ChangeSize(Constants.ScreenSize.M);
        }

        public void SmallWindow()
        {
            WinFormsService.ChangeSize(Constants.ScreenSize.S);
        }

        public string LoadRepositories()
        {
            var repos = FileService.GetRepositories();
            return JsonConvert.SerializeObject(repos.Folder);
        }

        public bool AddRepository(string path)
        {
            return FileService.AddRepository(path, false);
        }

        public void DeleteRepository(string path)
        {
            FileService.DeleteRepository(path);
        }

        public void ChangeToDefault(string path)
        {
            FileService.ChangeToDefault(path);
        }
    }
}
