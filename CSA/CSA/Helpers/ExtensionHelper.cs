using System;
using System.Collections.Generic;

namespace CSA.Helpers
{
    static class ExtensionHelper
    {
        public static bool AddOrReplace <TKey, TValue>(this Dictionary<TKey, TValue> map, TKey key, TValue value)
        {
            if (map != null)
            {
                if (map.ContainsKey(key))
                {
                    map[key] = value;
                    return true;
                }
                else
                {
                    map.Add(key, value);
                    return false;
                }
            }
            return false;
        }

        public static object ParseToType(this object value, string type)
        {
            switch (type)
            {
                case "Double":
                    return value.TryParseParameter();
                case "Integer":
                    return int.Parse(value.ToString());
                case "String":
                    return value;
                default:
                    return value;
            }
        }

        public static double TryParseParameter(this object value)
        {
            var tempValue = value.ToString().Trim();
            tempValue = tempValue.Replace(".", ",");
            double tempDoubleValue;
            if (!double.TryParse(tempValue, out tempDoubleValue))
            {
                tempValue = tempValue.Replace(",", ".");
                if(!double.TryParse(tempValue, out tempDoubleValue))
                {
                    throw new ArgumentOutOfRangeException("Double Value", "Cannot parse one of the double values.");
                }
            }
            return tempDoubleValue;
        }
    }
}
