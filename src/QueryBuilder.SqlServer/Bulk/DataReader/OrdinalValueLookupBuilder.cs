using QueryBuilder.Core.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QueryBuilder.SqlServer.Bulk.DataReader
{
    public delegate object OrdinalValueLookup<T>(T item, int index) where T : class;

    public class OrdinalValueLookupBuilder<T>
        where T : class
    {

        public IReadOnlyCollection<Tuple<int, string>> OrderedColumnNames => _orderedColumns.Select((c, index) => Tuple.Create(index, c.DbColumnName))
                                                                                            .ToList()
                                                                                            .AsReadOnly();
        private readonly List<ColumnMapping<T>> _orderedColumns;

        public bool HasDefaultValue { get; private set; }
        public T DefaultValue { get; private set; }

        public OrdinalValueLookupBuilder()
        {
            _orderedColumns = new List<ColumnMapping<T>>();
        }

        public OrdinalValueLookupBuilder<T> AddMapping(ColumnMapping<T> mapping)
        {
            HasDefaultValue = false;
            _orderedColumns.Add(mapping);
            return this;
        }

        public OrdinalValueLookupBuilder<T> SetDefaultValue(T value)
        {
            HasDefaultValue = true;
            DefaultValue    = value;
            return this;
        }

        #region Build Value Lookup

        public OrdinalValueLookup<T> BuildOrdinalValueLookup()
        {
            ParameterExpression itemParameter    = Expression.Parameter(typeof(T)),
                                ordinalParameter = Expression.Parameter(typeof(int));

            return Expression.Lambda<OrdinalValueLookup<T>>(
                BuildSwitch(itemParameter, ordinalParameter),
                itemParameter,
                ordinalParameter
            )
            .Compile();
        }

        protected SwitchExpression BuildSwitch(ParameterExpression itemParameter, ParameterExpression indexParamter)
        {
            return Expression.Switch(
                switchValue: indexParamter,
                defaultBody: BuildDefaultCase(),
                cases      : _orderedColumns.Select((c, index) => BuildCase(index, itemParameter, c.PropertyInfo))
                                            .ToArray()
            );
        }

        protected Expression BuildDefaultCase()
        {
            if (HasDefaultValue)
                return Expression.Constant(DefaultValue);
            else
                return Expression.Throw(
                    Expression.Constant(new IndexOutOfRangeException())
                );
        }

        protected SwitchCase BuildCase(int index, ParameterExpression itemParameter, PropertyInfo propertyInfo)
        {
            return Expression.SwitchCase(
                Expression.Convert( Expression.MakeMemberAccess(itemParameter, propertyInfo), typeof(object)),
                Expression.Constant(index)
            );
        }

        #endregion
    }
}
