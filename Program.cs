using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);


File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

// Find all *.txt files in the stores folder and its subfolders
// foreach (var file in salesFiles)
// {
//     Console.WriteLine(file);
// }

// Generate summary 
GenerateSummary(salesFiles, salesTotal, salesTotalDir, storesDirectory);


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

void GenerateSummary(IEnumerable<string> salesFiles, double salesTotal, string outputDir, string storesDirectory)
{
    var summaryLines = new List<string>
    {
        "Sales Summary",
        "----------------------------",
        $" Total Sales: ${salesTotal:N2}",
        " Details:"
    };

    foreach (var file in salesFiles)
    {
        string salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        var amount = data?.Total ?? 0;
        var relativePath = Path.GetRelativePath(storesDirectory, file);
        summaryLines.Add($"  {relativePath}: ${amount:N2}");
    }

    File.WriteAllLines(Path.Combine(outputDir, "summary.txt"), summaryLines);
}

record SalesData(double Total);

