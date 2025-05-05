using OfficeOpenXml;
using System.Reflection;

namespace EPPLUS.Excel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var people = ReadExcelToList<Person>("Book1.xlsx");
            Console.WriteLine("Hello, World!");
        }

        public static List<T> ReadExcelToList<T>(string filePath) where T : new()
        {
            FileInfo fileInfo = new(filePath);
            using ExcelPackage package = new(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            if (worksheet.Dimension == null) return [];

            var headers = GetHeaders(worksheet);
            return [.. Enumerable.Range(2, worksheet.Dimension.End.Row - 1).Select(row => CreateObjectFromRow<T>(worksheet, headers, row))];
        }

        private static Dictionary<int, string> GetHeaders(ExcelWorksheet worksheet)
        {
            return Enumerable.Range(1, worksheet.Dimension.End.Column)
                             .ToDictionary(
                                 col => col,
                                 col => worksheet.Cells[1, col].Value?.ToString()?.Trim() ?? string.Empty
                             );
        }

        private static T CreateObjectFromRow<T>(ExcelWorksheet worksheet, Dictionary<int, string> headers, int row) where T : new()
        {
            T obj = new T();
            foreach (var (col, header) in headers)
            {
                var property = typeof(T).GetProperty(header, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property != null && property.CanWrite)
                {
                    var cellValue = worksheet.Cells[row, col].Value;
                    if (cellValue != null)
                    {
                        try
                        {
                            object convertedValue = Convert.ChangeType(cellValue, property.PropertyType);
                            property.SetValue(obj, convertedValue);
                        }
                        catch
                        {
                            // Handle conversion errors silently
                        }
                    }
                }
            }
            return obj;
        }
    }
}



public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
}
