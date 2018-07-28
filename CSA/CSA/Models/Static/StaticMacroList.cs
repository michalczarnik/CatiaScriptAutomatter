using System.Collections.Generic;
using System.Linq;

namespace CSA.Models.Static
{
    static class StaticMacroList
    {
        static private List<MacroModel> _listOfMacros;
        public static List<MacroModel> ListOfMacros
        {
            get
            {
                if (_listOfMacros == null)
                    return Enumerable.Empty<MacroModel>().ToList();
                return _listOfMacros;
            }
            set
            {
                _listOfMacros = value;
            }
        }
    }
}
