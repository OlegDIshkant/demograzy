using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sql.SQLite
{
    public static class Utils
    {
        public static void CreateDb(string fullDbPath)
        {


            var directoryPath = Path.GetDirectoryName(fullDbPath);
            using (var p = new Process())
            {
                p.StartInfo.FileName = "mkdir";
                p.StartInfo.Arguments = $"-p {directoryPath}";
                p.Start();
                p.WaitForExit();
            }
            
            using (var p = new Process())
            {
                p.StartInfo.FileName = "sqlite3";
                p.StartInfo.Arguments = $"{fullDbPath} .databases .exit";
                p.Start();
                p.WaitForExit();
            }
            
        }
    }
}