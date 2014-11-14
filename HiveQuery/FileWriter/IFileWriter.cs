using System.Collections.Generic;
using System.Data;

namespace HiveQuery.FileWriter
{
    public interface IFileWriter
    {
        void Write(string fileName, IEnumerable<DataTable> tables);
    }
}