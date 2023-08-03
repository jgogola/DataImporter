
using DataImporter.Models;
using System;
using System.Threading.Tasks;

namespace DataImporter.Services;


public class ImportProcessor
{
    private static Random _random = new Random();

    public async Task<bool> ImportData(ImportCode importCode)
    {
        // Simulating some asynchronous work, such as calling an API
        await Task.Delay(TimeSpan.FromSeconds(5));

        // Randomly return true or false to simulate success or failure
        var result = _random.Next(2) == 0;


        Console.WriteLine($"Processing Code: {importCode.Code} with result of {result}.");
        return result;
    }
}
