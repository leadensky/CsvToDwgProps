using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvToDwgProps
{
    public class DwgPropImportService
    {
        public IReadOnlyList<ImportProps> Read(string filePath)
        {
            var results = new List<ImportProps>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args =>
                {
                    return args.Header.Trim().ToLower();
                },
                TrimOptions = TrimOptions.Trim,
                /* actions can be assigned to each of these to do something (logging?) */
                BadDataFound = null,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    return csv.GetRecords<ImportProps>().ToList();
                }
            }

        }
    }
}
