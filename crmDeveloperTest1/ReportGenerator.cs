using crmDeveloperTest1.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace crmDeveloperTest1 {
    internal class ReportGenerator {
        public void GenerateJsonReport(IEnumerable<ReportDTO> personReports, string filePath) {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(personReports, options);
            File.WriteAllText(filePath, json);
        }
    }
}
