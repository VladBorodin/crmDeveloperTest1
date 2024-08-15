using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crmDeveloperTest1.Data {
    internal class DBInit {
        private readonly IDataAccess dataAccess;
        public DBInit(IDataAccess dataAccess) {
            this.dataAccess = dataAccess;
        }
        public void CreateDatabase(string databaseName) {
            string masterConnectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
            string createDbQuery = $@"IF NOT EXIST (SELECT name FROM sys.database WHERE name = '{databaseName}')
                                    BEGIN CREATE DATABASE[{databaseName}]
                                    END;";
            using (SqlConnection connection = new SqlConnection(masterConnectionString)) {
                SqlCommand command = new SqlCommand(createDbQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine($"База данных '{databaseName}' успешно создана.");
            }
        }
        public void createTables() {
            string createPersonsTableSql = @"
            IF NOT EXIST  (SELECT * FROM sys.tables WHERE name = 'Persons')
            BEGIN
                CREATE TABLE Persons(
                Id INT PRIMARY KEY IDENTITY,
                FirstName NVARCHAR(50),
                LastName NVARCHAR(50),
                SecondName NVARCHAR(50),
                Gender NVARCHAR(10),
                Age INT,
                Workplace NVARCHAR(100),
                Country NVARCHAR(50),
                City NVARCHAR(50),
                Address NVARCHAR(100),
                Email NVARCHAR(100),
                PhoneNumber NVARCHAR(20),
                Birthday DATE)
            END;";

            string createCompaniesTableSql = @"
            IF NOT EXIST  (SELECT * FROM sys.tables WHERE name = 'Companies')
            BEGIN
            CREATE TABLE Companies (
                Id INT PRIMARY KEY IDENTITY,
                CompanyName NVARCHAR(100),
                INN NVARCHAR(20),
                OGRN NVARCHAR(20),
                Country NVARCHAR(50),
                City NVARCHAR(50),
                Address NVARCHAR(100),
                Email NVARCHAR(100),
                PhoneNumber NVARCHAR(20))
            END;";

            string createContractsTableSql = @"
            IF NOT EXIST  (SELECT * FROM sys.tables WHERE name = 'Contracts')
            BEGIN
            CREATE TABLE Contracts (
                Id INT PRIMARY KEY IDENTITY,
                AgentId INT FOREIGN KEY REFERENCES Companies(Id),
                MainPersonId INT FOREIGN KEY REFERENCES Persons(Id),
                Price DECIMAL(18, 2),
                Status NVARCHAR(20),
                SigningDate DATE)
            END;";

            dataAccess.Execute(createPersonsTableSql);
            dataAccess.Execute(createCompaniesTableSql);
            dataAccess.Execute(createContractsTableSql);
        }
    }
}
