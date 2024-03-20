using System;
using System.Linq;
using System.Windows.Forms;

namespace CsvToDwgProps
{
    public class FileSelector
    {
        OpenFileDialog _dialog;
        public FileSelector()
        {
            _dialog = new OpenFileDialog();
        }

        public virtual DialogResult ShowDialog(string filter = "All files|*.*", int filterIndex = 0, string initialDirectory = @"C:\", bool multiSelect = false)
        {
            _dialog.Filter = filter;
            _dialog.FilterIndex = filterIndex;
            _dialog.InitialDirectory = initialDirectory;
            _dialog.Multiselect = multiSelect;

            return _dialog.ShowDialog();
        }

        public virtual string[] FileNames => _dialog.FileNames;
    }
}
