using Demograzy.BusinessLogic;

namespace Demograzy.Core.Test
{
    internal static class CommonRoutines
    {

        internal static MainService PrepareMainService()
        {
            return 
                new Demograzy.BusinessLogic.MainService(
                    new Demograzy.DataAccess.Sql.TransactionMeansFactory(
                        null /* TODO: put test db here... */));
        }



    }
}