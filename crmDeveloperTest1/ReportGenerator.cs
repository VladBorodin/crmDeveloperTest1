using crmDeveloperTest1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace crmDeveloperTest1 {
    internal class ReportGenerator {
        public void GenerateJsonReport(IEnumerable<ReportDTO> personReports, string filePath) {
            var options = new JsonSerializerOptions {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            string json = JsonSerializer.Serialize(personReports, options);

            string validFilePath = Path.GetFullPath(filePath);

            File.WriteAllText(validFilePath, json);
            Console.WriteLine($"Отчет успешно создан в файле: {validFilePath}");
        }
    }
}
