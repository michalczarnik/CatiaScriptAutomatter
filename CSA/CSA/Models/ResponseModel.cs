using System;
using System.Collections.Generic;

namespace CSA.Models
{

    using System.Collections.Generic;

    public class ParameterList
    {
        public string ParamterName { get; set; }
        public object ParameterValue { get; set; }
    }

    public class ResponseModel
    {
        public string UniqueID { get; set; }
        public List<ParameterList> ParameterList { get; set; }
    }
}
