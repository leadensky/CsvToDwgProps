using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
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
            var openDialog = new FileSelector();
            var result = openDialog.ShowDialog("CSV files|*.csv", 0, multiSelect: false);
            if (result != DialogResult.OK)
            {
                Active.Editor.WriteMessage("\nUser cancelled.");
                return;
            }


            var fullCsvFilePath = openDialog.FileNames[0];
            /* assume the CSV file is in same directory as the drawings? */
            var csvFolderPath = Path.GetDirectoryName(fullCsvFilePath);

            var importer = new DwgPropImportService();
            var imports = importer.Read(fullCsvFilePath);

            foreach (var import in imports)
            {
                /* if the full dwgPath is specified in the CSV, then don't use the next line */
                var dwgPath = Path.Combine(csvFolderPath, import.DwgName);
                if (File.Exists(dwgPath))
                {
                    UpdateDwgProps(dwgPath, import);
                    Active.Editor.WriteMessage($"\nUpdated {dwgPath}");
                }
                else
                {
                    Active.Editor.WriteMessage($"\nFile not found: {dwgPath}");
                }
            }
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
