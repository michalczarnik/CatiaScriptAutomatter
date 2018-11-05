using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CSA.Services.Static
{
    static class CatiaService
    {
        static private INFITF.Application _catia = null;
        static private string _applicationName = "CATIA.Application";

        public static bool CatiaExists()
        {
            return _catia != null;
        }

        public static bool InitializeCatia()
        {
            try
            {
                _catia = (INFITF.Application)Marshal.GetActiveObject(_applicationName);
            }catch(Exception e)
            {
                //Add logging
                return false;
            }
            return true;
        }

        public static string RunMacro(string macroPath, string macroName, List<object> paramterList)
        {
            if(_catia != null)
            {
                try
                {
                    _catia.SystemService.ExecuteScript(macroPath, INFITF.CatScriptLibraryType.catScriptLibraryTypeDirectory, macroName,
                        "CATMain", paramterList.ToArray());
                }catch(Exception e)
                {
                    return e.Message;
                }
                
            }
            return string.Empty;
        }
    }
}
