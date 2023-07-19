using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using ME = Microsoft.Office.Interop.Excel;

namespace QuanLib.Excel
{
    public static class WorkbookExtension
    {
        public static Worksheet GetDefaultWorksheet(this Workbook workbook) => workbook.Worksheets[1];
    }
}
