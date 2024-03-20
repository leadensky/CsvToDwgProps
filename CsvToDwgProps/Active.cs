using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Linq;

namespace CsvToDwgProps
{
    /// <summary>
    /// Adapted from Scott McFarlane's SD12077 AU 2015 class
    /// </summary>
    public static class Active
    {
        public static Editor Editor
        {
            get
            {
                if (Document == null)
                    return null;

                return Document.Editor;
            }
        }

        public static Document Document
        {
            get { return Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument; }
        }

        public static Database Database
        {
            get
            {
                if (Document == null)
                    return null;

                return Document.Database;
            }
        }

        public static void UsingTransaction(this Document document, Action<Transaction> action)
        {
            using (DocumentLock docLock = document.LockDocument())
            {
                using (var tr = document.Database.TransactionManager.StartTransaction())
                {
                    try
                    {
                        action(tr);
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        Active.Editor.WriteMessage($"Error: {ex.Message}");
                        tr.Abort();
                    }
                }
            }
        }

    }
}
