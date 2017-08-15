using System;

namespace EntityFramework.Extensions.Core.Mappings
{
    public class ColumnMapping
    {
        public bool IsIdentity { get; private set; }

        public string PropertyName { get; private set; }

        public Type PropertyType { get; private set; }

        public string SqlName { get; private set; }

        //public SqlDbType SqlType { get; private set; } // TODO

        public ColumnMapping(bool isIdentity, string propertyName, Type propertyType, string sqlName/*, SqlDbType sqlType*/)
        {
            IsIdentity   = isIdentity;
            PropertyName = propertyName;
            PropertyType = propertyType;
            SqlName      = sqlName;
            //SqlType      = sqlType;
        }
    }
}
