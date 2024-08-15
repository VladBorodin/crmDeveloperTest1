using crmDeveloperTest1.Data;
using crmDeveloperTest1.DataDB;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace crmDeveloperTest1 {
    public class Program {
        static void Main(string[] args) {
            string dbName = "ContractManagementDB";
            // Строка подключения к серверу для создания базы данных
            string serverConnStr = $"Server=localhost;Trusted_Connection=True;TrustServerCertificate=True;";
            // Строка подключения к созданной базе данных
            string connStr = $"Server=localhost;Database={dbName};Trusted_Connection=True;TrustServerCertificate=True;";

            IDataAccess dataAccess = new SqlDataAccess(connStr);
            DBInit dbInit = new DBInit(dataAccess);
            dbInit.CreateDatabase(serverConnStr, connStr, dbName);
            dbInit.CreateTables();

            TestConnection(connStr);

            ContractService contractService = new ContractService(dataAccess);
            ReportGenerator reportGenerator = new ReportGenerator();

            bool exit = false;

            while (!exit) {
                Console.Clear();
                Console.WriteLine("=== Меню ===");
                Console.WriteLine("1. Получение суммы всех договоров за этот год");
                Console.WriteLine("2. Сумма договоров по каждому контрагенту из России");
                Console.WriteLine("3. Список e-mail уполномоченных лиц с договорами за последние 30 дней на сумму > 40000");
                Console.WriteLine("4. Разрыв контракта с физ лицами старше 60-ти лет");
                Console.WriteLine("5. Создание отчёта");
                Console.WriteLine("6. Выход");
                Console.WriteLine("=================");
                Console.Write("Выберите пункт меню: ");

                var key = Console.ReadKey();

                switch (key.KeyChar) {
                    case '1':
                        GetTotalContractsPriceForCurrentYear(contractService);
                        break;
                    case '2':
                        GetTotalContractsPriceByRussianCounterparties(contractService);
                        break;
                    case '3':
                        GetEmailsOfMainPersonsWithRecentContracts(contractService);
                        break;
                    case '4':
                        NoCountryForOldMen(contractService);
                        break;
                    case '5':
                        GenerateReport(contractService, reportGenerator);
                        break;
                    case '6':
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }

                if (!exit) {
                    Console.WriteLine("Нажмите любую клавишу, чтобы вернуться в меню...");
                    Console.ReadKey();
                }
            }
        }
        private static void GetTotalContractsPriceForCurrentYear(ContractService contractService) {
            decimal totalContractsPrice = contractService.GetTatalContractsPriceForCurrentYear();
            Console.WriteLine($"\nСумма всех заключенных договоров за текущий год: {totalContractsPrice:C}");
        }
        private static void GetTotalContractsPriceByRussianCounterparties(ContractService contractService) {
            var contractsByAgent = contractService.GetTotalContractsPriceByRussianCounterparties();
            Console.WriteLine("\nСумма договоров по контрагентам из России:");
            foreach (var (companyName, totalAmount) in contractsByAgent) {
                Console.WriteLine($"Компания: {companyName}, Сумма: {totalAmount:C}");
            }
        }
        private static void GetEmailsOfMainPersonsWithRecentContracts(ContractService contractService) {
            var emails = contractService.GetEmailsOfMainPersonsWithRecentContracts();
            Console.WriteLine("\nСписок e-mail уполномоченных лиц, заключивших договора за последние 30 дней на сумму > 40000:");
            foreach (var email in emails) {
                Console.WriteLine(email);
            }
        }
        private static void NoCountryForOldMen(ContractService contractService) {
            contractService.NoCountryForOldMen();
            Console.WriteLine("\nРазрыв контракта с физ лицами старше 60-ти лет");
        }
        private static void GenerateReport(ContractService contractService, ReportGenerator reportGenerator) {
            var personReports = contractService.GetPersonsWithActiveContractsInMoscow();
            reportGenerator.GenerateJsonReport(personReports, "MoscowPersonsReport.json");
            Console.WriteLine("\nОтчет сформирован в файл: MoscowPersonsReport.json");
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