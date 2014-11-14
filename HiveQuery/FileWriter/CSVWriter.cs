using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace HiveQuery.FileWriter
{
    public class CSVWriter : IFileWriter
    {
        private StringBuilder builder = new StringBuilder();

        public void Write(string fileName, IEnumerable<System.Data.DataTable> tables)
        {
            int i = 0;
            fileName = fileName.Replace(".csv", "");

            foreach (var table in tables)
            {
                SaveEveryTable(fileName, i++, table);
            }
        }

        private void SaveEveryTable(string fileName, int i, DataTable table)
        {
            builder.Clear();

            foreach (DataColumn column in table.Columns)
            {
                builder.AppendFormat("{0};", column.ColumnName);
            }
            builder.Append("\n");

            foreach (DataRow row in table.Rows)
            {
                foreach (var column in row.ItemArray)
                {
                    builder.AppendFormat("{0};", column);
                }
                builder.Append("\n");
            }

            File.WriteAllText(string.Format("{0}_{1}.csv", fileName, i), builder.ToString());
        }
    }
}