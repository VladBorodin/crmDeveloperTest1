using System.Data;

namespace crmDeveloperTest1.Data {
    internal interface IDataAccess {
        IEnumerable<T> Query<T>(string query, object parameters = null);
        void Execute(string query, object parameters = null);
    }
}