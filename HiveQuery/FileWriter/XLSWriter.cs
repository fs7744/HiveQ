using HiveQuery.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace HiveQuery.FileWriter
{
    public class XLSWriter : IFileWriter
    {
        protected virtual IWorkbook GetWorkBook()
        {
            return new HSSFWorkbook();
        }

        public void Write(string fileName, IEnumerable<DataTable> tables)
        {
            var workBook = GetWorkBook();
            foreach (var table in tables)
            {
                if (table == null) continue;
                var sheet = workBook.CreateSheet();
                var colRow = sheet.CreateRow(0);
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    var col = colRow.CreateCell(i);
                    col.SetCellValue(table.Columns[i].ColumnName);
                }
                for (int i = 1; i <= table.Rows.Count; i++)
                {
                    var row = table.Rows[i - 1];
                    if (row.ItemArray.IsEmpty()) continue;
                    var xlsRow = sheet.CreateRow(i);
                    for (int j = 0; j < row.ItemArray.Length; j++)
                    {
                        var cell = xlsRow.CreateCell(j);
                        cell.SetCellValue(row[j].ToString());
                    }
                }
            }
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                workBook.Write(stream);
            }
        }
    }
}