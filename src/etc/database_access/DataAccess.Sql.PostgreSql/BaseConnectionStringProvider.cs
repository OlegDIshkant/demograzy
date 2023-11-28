
using System.Diagnostics;
using System.Security;


namespace DataAccess.Sql.PostgreSql
{
    public class BaseConnectionStringProvider : IConnectionStringProvider
    {

        private readonly string _filePath;

        public SecureString ConnectionString 
        {
            get
            {
                var result = new SecureString();

                try
                {
                    using (var reader = File.OpenText(_filePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            result.AppendChar((char)reader.Read());
                        }
                    }
                    return result;
                }
                catch (Exception e)
                {
                    result.Dispose();  // Dispose partially read secret immediately if failed to read completely!
                    
                    Debug.WriteLine($"Failed to read connection string from file '{_filePath}' due to exception: {e}.");
                    return null;
                }
            }
        }



        /// <param name="path"> Path to the file containing a connection string.</param>
        /// <remarks>
        /// Connection string should look like: 
        /// "Host=...;Username=...;Password=...;Database=..."
        /// </remarks>
        public BaseConnectionStringProvider(string path)
        {
            _filePath = path;
        }

    }
}