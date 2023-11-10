using System.Diagnostics;
using Demograzy.BusinessLogic;
using NUnit.Framework.Constraints;

namespace Demograzy.Core.Test
{
    internal static class CommonRoutines
    {

        internal static MainService PrepareMainService()
        {
            const string dbName = "/workspaces/demograzy/src/core/Demograzy.Core.Test/test_db/~$_unit_test_db";

            var testableService = 
                new Demograzy.BusinessLogic.MainService(
                    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
                       Demograzy.DataAccess.Sql.SQLite.SqlCommandBuilderFactory.Create(dbName)));

            ApplyAllMigrations(dbName);

            return testableService;
        }


        private static void ApplyAllMigrations(string pathToTestDb)
        {
            var changelogDirectory = "/workspaces/demograzy/src/core/Demograzy.DataAccess.Sql/db_version_control/changelog/";
            var changelogRootFile = "changelog-root.json";
            
            Directory.SetCurrentDirectory(changelogDirectory);

            var psi = new ProcessStartInfo()
            {
                FileName = "liquibase",
                Arguments = $"update --changelog-file=\"{changelogRootFile}\" --url=jdbc:sqlite:{pathToTestDb}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (var prc = Process.Start(psi))
            {
                prc.WaitForExit();
                if (prc.ExitCode != 0)
                {
                    var output = prc.StandardOutput.ReadToEnd();
                    var error = prc.StandardError.ReadToEnd();

                    var errMsg = 
                        "Could not apply migration to in-memory database due to some error.\n\n" +
                        $"OUTPUT:\n {output}\n\n" + 
                        $"ERROR:\n {error}";

                    throw new InvalidProgramException(errMsg);
                }
            }
        }


        


    }
}