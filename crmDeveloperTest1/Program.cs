using crmDeveloperTest1.Data;
using crmDeveloperTest1.DataDB;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace crmDeveloperTest1 {
    public class Program {
        static void Main(string[] args) {
            string dbName = "ContractManagmentDB";
            string connStr = $"Server = localgost;Database = {dbName}; Trusted_Connection=True;";

            TestConnection(connStr);

            IDataAccess dataAccess = new SqlDataAccess(connStr);
            DBInit dbInit = new DBInit(dataAccess);

            dbInit.CreateDatabase(dbName);
            dbInit.createTables();

            ContractService contractService = new ContractService(dataAccess);

            //Получение суммы всех договоров за этот год
            decimal totalContractsPrice = contractService.GetTatalContractsPriceForCurrentYear();
            Console.WriteLine($"Сумма всех заключенных договоров за текущий год: {totalContractsPrice:C}");

            // Получение суммы договоров по каждому контрагенту из России
            var contractsByAgent = contractService.GetTotalContractsPriceByRussianCounterparties();
            Console.WriteLine("Сумма договоров по контрагентам из России:");
            foreach (var (companyName, totalAmount) in contractsByAgent) {
                Console.WriteLine($"Компания: {companyName}, Сумма: {totalAmount:C}");
            }

            // Получение списка e-mail уполномоченных лиц с договорами за последние 30 дней на сумму > 40000
            var emails = contractService.GetEmailsOfMainPersonsWithRecentContracts();
            Console.WriteLine("Список e-mail уполномоченных лиц, заключивших договора за последние 30 дней на сумму > 40000:");
            foreach (var email in emails) {
                Console.WriteLine(email);
            }

            // Инициализация соединения и сервиса
            var reportGenerator = new ReportGenerator();

            // Изменение статуса договоров для физических лиц старше 60 лет
            contractService.NoCountryForOldMen();
            Console.WriteLine("Разрыв контракта с физ лицами старше 60-ти лет");

            // Создание отчёта
            var personReports = contractService.GetPersonsWithActiveContractsInMoscow();
            reportGenerator.GenerateJsonReport(personReports, "MoscowPersonsReport.json");
            Console.WriteLine("Отчет сформирован в файл: MoscowPersonsReport.json");
        }
        public static void TestConnection(string connStr) {
            using (SqlConnection conn = new SqlConnection(connStr)) {
                try {
                    conn.Open();
                    Console.WriteLine("Подключение установлено!");
                } catch (Exception ex) {
                    Console.WriteLine($"Ошибка при подключении к БД: {ex.Message}");
                }
            }
        }
        
    }
}