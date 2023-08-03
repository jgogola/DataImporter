using CommandLine;

namespace DataImporter;

public class Options
{
    [Option('i', "import", Required = false, HelpText = "Run job to import data.")]
    public bool RunImportData { get; set; }
}
