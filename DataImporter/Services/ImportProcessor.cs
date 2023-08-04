
using DataImporter.Models;
using System;
using System.Threading.Tasks;

namespace DataImporter.Services;


public class ImportProcessor
{
    private static Random _random = new Random();

    public async Task<Tuple<bool,int>> ImportData(ImportCode importCode)
    {
        // Simulating some asynchronous work, such as calling an API
        int randomSeconds = _random.Next(1, 11);
        await Task.Delay(TimeSpan.FromSeconds(randomSeconds));

        // Randomly return true or false to simulate success or failure
        var result = _random.Next(2) == 0;

        Console.WriteLine($"{importCode.Id} - Processing Code: {importCode.Code} result: {result} in {randomSeconds} seconds.");
       
       
        return Tuple.Create(result, randomSeconds);
    }
}
