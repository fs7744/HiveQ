namespace HiveQuery.DataProvider
{
    public class Hive2DataProvider : HiveDataProvider
    {
        public override Hive.IHiveClient CreateClient(Data.Connection conn)
        {
            return new Hive2.HiveClient(conn.HiveIP, conn.HivePort, conn.UserName, conn.Password);
        }
    }
}