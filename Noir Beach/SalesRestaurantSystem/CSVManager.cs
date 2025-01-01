using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Text;
using System.Windows;
using static Org.BouncyCastle.Utilities.Test.FixedSecureRandom;

public static class CSVManager<T> where T : class
{

    public static void ConvertListToCSV(List<T> data)
    {
        string csvContent = BuildCSV(data);

        var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
            Title = "Select a folder to save the CSV file"
        };

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            string folderPath = dialog.FileName;
            string filePath = Path.Combine(folderPath, "output.csv");

            // Guardar el archivo CSV
            File.WriteAllText(filePath, csvContent);

            MessageBox.Show($"CSV file saved at: {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show("No folder was selected.", "Cancelled", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    public static string BuildCSV(List<T> data)
    {
        if (data == null || data.Count == 0)
        {
            throw new ArgumentException("The list contains no data.");
        }

        var properties = typeof(T).GetProperties();
        var csvBuilder = new StringBuilder();

        csvBuilder.AppendLine(string.Join(",", properties.Select(p => EscapeCSV(p.Name))));

        foreach (var item in data)
        {
            var values = properties.Select(p => EscapeCSV(p.GetValue(item)?.ToString() ?? string.Empty));
            csvBuilder.AppendLine(string.Join(",", values));
        }

        return csvBuilder.ToString();
    }

    private static string EscapeCSV(string value)
    {
        if (value.Contains(",") || value.Contains("\""))
        {
            value = value.Replace("\"", "\"\"");
            return $"\"{value}\"";
        }
        return value;
    }
}
