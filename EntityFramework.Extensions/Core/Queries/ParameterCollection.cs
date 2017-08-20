using System.Collections.Generic;

namespace EntityFramework.Extensions.Core.Queries
{
    public abstract class ParameterCollection<TQueryParameter>
    {
        protected readonly List<TQueryParameter> _parameters;

        public IEnumerable<TQueryParameter> Parameters { get => _parameters.AsReadOnly(); }

        public ParameterCollection()
        {
            _parameters = new List<TQueryParameter>();
        }

        public abstract string AddParameter(object value);

        //public string AddParameter(object value)
        //{
        //    Type valueType = value.GetType();

        //    MethodInfo method = GetType().GetMethods()
        //                                 .First(m => m.IsGenericMethod && m.Name == nameof(AddParameter));

        //    return (string)method.MakeGenericMethod(valueType)
        //                         .Invoke(this, new object[] { value });
        //}
    }
}
