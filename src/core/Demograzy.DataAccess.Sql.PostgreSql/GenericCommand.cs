
namespace Demograzy.DataAccess.Sql.PostgreSql
{
    internal sealed class GenericCommand<R> : ISqlCommand<R>
    {
        private readonly Func<Task<R>> _Action;


        public GenericCommand(Func<Task<R>> Action)
        {
            _Action = Action;
        }


        public Task<R> ExecuteAsync()
        {
            return _Action();
        }

    }
}