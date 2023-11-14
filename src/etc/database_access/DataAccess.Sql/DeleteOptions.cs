namespace DataAccess.Sql
{
    public sealed class DeleteOptions
    {
        public string From { get; set; }
        public IWhereClause Where { get; set; }
    }
}