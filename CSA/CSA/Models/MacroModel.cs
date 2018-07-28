using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CSA.Models
{
    class MacroModel
    {
        private Dictionary<string, string> _images { get; set; }
        private List<ParameterModel> _parameterList { get; set; }

        public string DirectoryName { get; internal set; }
        public string FileName { get; internal set; }
        public string Description { get; internal set; }
        public Dictionary<string, string> Images
        {
            get
            {
                if (this._images == null)
                    this._images = new Dictionary<string, string>();
                return this._images;
            }
            set { this._images = value; }
        }
        public List<ParameterModel> ParameterList
        {
            get
            {
                if (this._parameterList == null)
                    this._parameterList = new List<ParameterModel>();
                return this._parameterList;
            }
            set { this._parameterList = value; }
        }
        public Guid UniqueID { get; set; }
        
        public Warnings Warnings { get; set; }

    }

    public class Warnings
    {
        public bool IsAfter { get; set; }
        public List<Warning> WarningList { get; set; }
    }

    public class Warning
    {
        public string WarningImagePath { get; set; }
        public string WarningText { get; set; }
    }
}
