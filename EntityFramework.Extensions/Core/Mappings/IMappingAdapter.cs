﻿using System;
using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Mappings
{
    public interface IMappingAdapter<TEntity>
        where TEntity : class
    {
        Type EntityType { get; }

        string GetTableName();

        IEnumerable<ColumnMapping> GetColumns();
    }
}
