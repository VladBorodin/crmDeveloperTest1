using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crmDeveloperTest1.Data {
    internal class DBInit {
        private readonly IDataAccess dataAccess;

        public void CreateDatabase(string serverConnStr, string connStr, string dbName) {
            using (var connection = new SqlConnection(serverConnStr)) {
                connection.Open();

                try {
                    // Создание базы данных, если она не существует
                    string createDatabaseSql = $@"
                    IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}')
                    BEGIN
                        CREATE DATABASE [{dbName}]
                    END;
                ";
                    using (var command = new SqlCommand(createDatabaseSql, connection)) {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"База данных '{dbName}' успешно создана или уже существует.");
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"Ошибка при создании базы данных: {ex.Message}");
                    return;
                }
                connection.Close();
            }
            using (var connection = new SqlConnection(connStr)) {
                connection.Open();
            }
        }
        public DBInit(IDataAccess dataAccess) {
            this.dataAccess = dataAccess;
        }
        public void CreateTables() {
            string createPersonsTableSql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persons')
            BEGIN
                CREATE TABLE Persons (
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
                    Birthday DATE
                );
            END;";

            string createCompaniesTableSql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Companies')
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
                    PhoneNumber NVARCHAR(20)
                );
            END;";

            string createContractsTableSql = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Contracts')
            BEGIN
                CREATE TABLE Contracts (
                    Id INT PRIMARY KEY IDENTITY,
                    AgentId INT FOREIGN KEY REFERENCES Companies(Id),
                    MainPersonId INT FOREIGN KEY REFERENCES Persons(Id),
                    Price DECIMAL(18, 2),
                    Status NVARCHAR(20),
                    SigningDate DATE
                );
            END;";

            try {
                dataAccess.Execute(createPersonsTableSql);
                dataAccess.Execute(createCompaniesTableSql);
                dataAccess.Execute(createContractsTableSql);
                Console.WriteLine("Tables created successfully.");
            } catch (Exception ex) {
                Console.WriteLine($"Error creating tables: {ex.Message}");
            }
        }
    }
}
