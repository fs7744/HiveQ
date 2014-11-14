using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HiveQuery.FileWriter
{
    public class XLSXWriter : XLSWriter
    {
        protected override IWorkbook GetWorkBook()
        {
            return new XSSFWorkbook();
        }
    }
}