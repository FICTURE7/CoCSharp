using CoCSharp.Data.Csv;
using System;

namespace CoCSharp.Data
{
    public abstract class VillageObjectData
    {
        [CsvIgnore()]
        public virtual string Name { get; set; }
    }
}
