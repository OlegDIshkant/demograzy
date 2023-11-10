using System.Threading.Tasks;

namespace DataAccess.Sql
{
    public interface ISqlCommandBuilderFactory
    {
        Task<ISqlCommandBuilder> CreateAsync();
    }
}