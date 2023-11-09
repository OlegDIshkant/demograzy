
using System;
using System.Threading.Tasks;

namespace Demograzy.DataAccess.Sql
{
    public sealed class GenericCommand<R> : ISqlCommand<R>
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