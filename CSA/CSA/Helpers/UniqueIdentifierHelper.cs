using System;
using System.Collections.Generic;
using System.Linq;

namespace CSA.Helpers
{
    static class UniqueIdentifierHelper
    {
        static private List<Guid> _uniqueIdentifiers;
        static public Guid GenerateUniqueID()
        {
            if (_uniqueIdentifiers == null)
                _uniqueIdentifiers = new List<Guid>();
            var guid = Guid.Empty;
            bool canStop = false;
            do
            {
                guid = Guid.NewGuid();
                if (!_uniqueIdentifiers.Any(g => g.Equals(guid)))
                {
                    _uniqueIdentifiers.Add(guid);
                    canStop = true;
                }
            } while (!canStop);
            return guid;
        }
    }
}
