using System;
using System.Linq;

namespace CsvToDwgProps
{
    /// <summary>
    /// This is just a sample property list. Property names/types should be modified to match columns of CSV file.
    /// </summary>
    public class ImportProps
    {
        public string DwgName { get; set; }
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
    }
}
