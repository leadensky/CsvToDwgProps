using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using CSharpFunctionalExtensions;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CsvToDwgProps
{
    public class MyCommands
    {
        /// <summary>
        /// Reads dwg and property data from a CSV file then updates the DwgProps for each file.
        /// Assumes that CSV and DWG files are in the same folder.
        /// CSV and DWG files can not be open.
        /// </summary>
        [CommandMethod("CSV_TO_DWGPROPS")]
        public void CsvToDwgProps()
        {
            SelectSourceFile()
                .Tap(fn =>
                {
                    /* assume the CSV file is in same directory as the drawings? */
                    var csvFolderPath = Path.GetDirectoryName(fn);

                    var importer = new ImportService<ImportProps>();
                    var imports = importer.Read(fn);

                    foreach (var import in imports)
                    {
                        /* if the full dwgPath is not specified in the CSV, use the CSV folder path */
                        var dwgFolderPath = Path.GetDirectoryName(import.DwgName);
                        if (string.IsNullOrEmpty(dwgFolderPath))
                            dwgFolderPath = csvFolderPath;

                        var fullDwgPath = Path.Combine(dwgFolderPath, import.DwgName);
                        if (File.Exists(fullDwgPath))
                        {
                            UpdateDwgProps(fullDwgPath, import);
                            Active.Editor.WriteMessage($"\nUpdated {fullDwgPath}");
                        }
                        else
                        {
                            Active.Editor.WriteMessage($"\nFile not found: {fullDwgPath}");
                        }
                    }

                })
                .TapError(err => Active.Editor.WriteMessage(err));
        }

        Result<string> SelectSourceFile()
        {
            var openDialog = new FileSelector();
            var result = openDialog.ShowDialog("CSV files|*.csv", 0, multiSelect: false);
            if (result != DialogResult.OK)
            {
                return Result.Failure<string>("\nUser cancelled.");
            }

            return Result.Success(openDialog.FileNames[0]);
        }

        void UpdateDwgProps(string fullFilePath, ImportProps propsToImport)
        {
            var document = acadApp.DocumentManager.Open(fullFilePath, false);

            Active.UsingTransaction(document,
                tr =>
                {
                    /* This is just a sample set of properties and needs 
                     * to be modified to match actual properties of ImportProps class
                     */
                    document.Database.SetCustomProperty(nameof(propsToImport.Prop1), propsToImport.Prop1);
                    document.Database.SetCustomProperty(nameof(propsToImport.Prop2), propsToImport.Prop2);
                    document.Database.SetCustomProperty(nameof(propsToImport.Prop3), propsToImport.Prop3);
                });

            document.CloseAndSave(fullFilePath);
        }

    }
}
