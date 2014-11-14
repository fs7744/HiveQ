using System;
using System.Collections.Generic;
using System.Data;

namespace HiveQuery.FileWriter
{
    public static class FileWriterFactory
    {
        public static Exception Write(string fileName, string format, List<DataTable> result)
        {
            Exception ex = null;
            var writer = GetWriter(format);
            try
            {
                writer.Write(fileName, result);
            }
            catch (Exception e)
            {
                ex = e;
            }
            return ex;
        }

        private static IFileWriter GetWriter(string format)
        {
            IFileWriter result = null;
            switch (format)
            {
                case "CSV":
                    result = new CSVWriter();
                    break;

                case "XLS":
                    result = new XLSWriter();
                    break;

                case "XLSX":
                    result = new XLSXWriter();
                    break;
            }
            return result;
        }
    }
}