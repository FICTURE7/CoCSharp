using CoCSharp.Data.Csv;
using System;

namespace CoCSharp.Data
{
    public abstract class VillageObjectData // make it more universal
    {
        public virtual string Name { get; set; }
        public virtual string TID { get; set; }
        public virtual string InfoTID { get; set; }
        public virtual string SWF { get; set; }
    }
}
