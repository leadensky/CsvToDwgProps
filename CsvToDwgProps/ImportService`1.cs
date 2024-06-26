﻿using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvToDwgProps
{
    public class ImportService<T>
    {
        public IReadOnlyList<T> Read(string filePath)
        {
            var results = new List<T>();
            var config = GetConfig();

            using (var reader = new StreamReader(filePath))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    return csv.GetRecords<T>().ToList();
                }
            }
        }

        protected virtual CsvConfiguration GetConfig()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture)
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
        }
    }
}
