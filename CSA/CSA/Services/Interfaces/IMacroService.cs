using CSA.Models;

namespace CSA.Services.Interfaces
{
    interface IMacroService
    {
        MacroModel GetMacro(string fileName, string path);
    }
}
