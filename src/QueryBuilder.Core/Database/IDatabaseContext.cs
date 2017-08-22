using System.Data.Common;

namespace QueryBuilder.Core.Database
{
    public interface IDatabaseContext
    {

        DbTransaction BeginTransaction();

        DbConnection GetConnection();
    }
}
