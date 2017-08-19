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

        public abstract string AddParameter<T>(T value);
    }
}
