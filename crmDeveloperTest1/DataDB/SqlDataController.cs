using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace crmDeveloperTest1.Data {
    public class SqlDataAccess : IDataAccess {
        private readonly string connectionString;
        public SqlDataAccess(string connectionString) {
            this.connectionString = connectionString;
        }
        public IEnumerable<T> Query<T>(string sql, object parameters = null) {
            using (var connection = new SqlConnection(connectionString)) {
                return connection.Query<T>(sql, parameters);
            }
        }
        public void Execute(string sql, object parameters = null) {
            using (var connection = new SqlConnection(connectionString)) {
                connection.Execute(sql, parameters);
            }
        }
    }
}
