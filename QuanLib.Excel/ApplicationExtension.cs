using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using ME = Microsoft.Office.Interop.Excel;

namespace QuanLib.Excel
{
    public static class ApplicationExtension
    {
        public static Workbook CreateWorkbook(this Application application, string path)
        {
            Workbook workbook = application.Workbooks.Add(Type.Missing);
            workbook.SaveAs(path);
            return workbook;
        }

        public static void SaveWorkbooks(this Application application)
        {
            foreach (Workbook book in application.Workbooks)
                book.Save();
        }

        public static void CloseWorkbooks(this Application application)
        {
            foreach (Workbook book in application.Workbooks)
                book.Close();
        }

        public static void CloseQuit(this Application application)
        {
            application.CloseWorkbooks();
            application.Quit();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Marshal.ReleaseComObject(application);
        }

        public static void SaveCloseQuit(this Application application)
        {
            application.SaveWorkbooks();
            application.CloseWorkbooks();
            application.Quit();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Marshal.ReleaseComObject(application);
        }
    }
}
