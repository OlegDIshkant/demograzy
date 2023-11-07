using System.Threading.Tasks;

namespace Demograzy.DataAccess.Sql
{
    public interface ISqlCommandBuilderFactory
    {
        Task<ISqlCommandBuilder> CreateAsync();
    }
}