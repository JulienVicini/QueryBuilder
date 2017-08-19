﻿namespace EntityFramework.Extensions.Core.Bulk
{
    public interface IBulkExecutor<T>
        where T : class
    {
        void Write(string tableName, T records);
    }
}
