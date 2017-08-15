using System.Data.Common;

namespace EntityFramework.Extensions.Core.Queries
{
    public interface IDatabaseContext
    {
        DbConnection GetConnection();
    }
}
