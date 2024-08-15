using crmDeveloperTest1.Data;
using crmDeveloperTest1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crmDeveloperTest1.DataDB {
    internal class ContractService {
        private readonly IDataAccess dataAccess;
        public ContractService(IDataAccess dataAccess) {
            this.dataAccess = dataAccess;
        }
        //Сумма всех договоров за текущий год
        public decimal GetTatalContractsPriceForCurrentYear() {
            string query = @"SELECT SUM(Price) as TotalAmount
                            FROM Contracts
                            WHERE YEAR(SigningDate) = YEAR(GETDATE());";
            var result = dataAccess.Query<decimal?>(query).FirstOrDefault();
            return result ?? 0;
        }
        // Cумма договоров по каждому контрагенту из России
        public IEnumerable<(string CompanyName, decimal TotalAmount)> GetTotalContractsPriceByRussianCounterparties() {
            string query = @"
            SELECT Companies.CompanyName, SUM(Contracts.Price) as TotalAmount
            FROM Contracts
            JOIN Companies ON Contracts.AgentId = Companies.Id
            WHERE Companies.Country = 'Россия'
            GROUP BY Companies.CompanyName;";
            var result = dataAccess.Query<(string CompanyName, decimal TotalAmount)>(query);
            return result;
        }
        // Получения списка e-mail уполномоченных лиц с договорами за последние 30 дней на сумму > 40000
        public IEnumerable<string> GetEmailsOfMainPersonsWithRecentContracts() {
            string query = @"
            SELECT DISTINCT Persons.Email
            FROM Contracts
            JOIN Persons ON Contracts.MainPersonId = Persons.Id
            WHERE Contracts.Price > 40000 AND Contracts.SigningDate >= DATEADD(DAY, -30, GETDATE());";
            var result = dataAccess.Query<string>(query);
            return result;
        }
        // Изменения статуса договора на "Расторгнут" для физических лиц старше 60 лет
        public void NoCountryForOldMen() {
            string query = @"
            UPDATE Contracts
            SET Status = 'Terminated'
            WHERE MainPersonId IN (
                SELECT Id FROM Persons
                WHERE DATEDIFF(YEAR, Birthday, GETDATE()) >= 60
            )
            AND Status = 'Active';";
            dataAccess.Execute(query);
        }
        // Метод для создания отчёта
        public IEnumerable<ReportDTO> GetPersonsWithActiveContractsInMoscow() {
            string query = @"
            SELECT p.FirstName, p.LastName, p.SecondName, p.Email, p.Phone, p.Birthday
            FROM Contracts c
            JOIN Persons p ON c.MainPersonId = p.Id
            JOIN Companies cmp ON c.AgentId = cmp.Id
            WHERE cmp.City = 'Москва' AND c.Status = 'Active';";
            var result = dataAccess.Query<ReportDTO>(query);
            return result;
        }

    }
}
