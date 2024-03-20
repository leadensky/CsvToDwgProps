using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvToDwgProps
{
    public static class DatabaseExtensions
    {
        //From: https://forums.autodesk.com/t5/net/set-a-custom-drawing-property/td-p/6893092
        public static Dictionary<string, string> GetCustomProperties(this Database db)
        {
            var result = new Dictionary<string, string>();
            var dictEnum = db.SummaryInfo.CustomProperties;

            while (dictEnum.MoveNext())
            {
                var entry = dictEnum.Entry;
                result.Add((string)entry.Key, (string)entry.Value);
            }

            return result;
        }

        public static string GetCustomProperty(this Database db, string key)
        {
            var sumInfo = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
            var custProps = sumInfo.CustomPropertyTable;

            return (string)custProps[key];
        }

        public static void SetCustomProperty(this Database db, string key, string value)
        {
            var infoBuilder = new DatabaseSummaryInfoBuilder(db.SummaryInfo);
            var custProps = infoBuilder.CustomPropertyTable;
            if (custProps.Contains(key))
                custProps[key] = value;
            else
                custProps.Add(key, value);

            db.SummaryInfo = infoBuilder.ToDatabaseSummaryInfo();
        }

    }
}
