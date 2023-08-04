// See https://aka.ms/new-console-template for more information

using CommandLine;
using DataImporter;
using DataImporter.Models;
using DataImporter.Services;
using System.Collections.Concurrent;

await Parser.Default.ParseArguments<Options>(args)
.WithParsedAsync<Options>(async o =>
{

    if (o.RunImportData)
    {
        Console.WriteLine("Starting Import-Data job.");
        //   var dataItemsQueue = DataAccessService.GenerateRandomImportCodes(10);

        var dataItemsQueue = new ConcurrentQueue<ImportCode>(DataAccessService.GenerateRandomImportCodes(10));
        int runningTasks = 0;

        var semaphore = new SemaphoreSlim(5); // Limiting concurrent tasks to 5
        var tasks = new List<Task>();
        var successCount = 0;
        var failCount = 0;
        var successSeconds = 0;
        var failSeconds = 0;

        while (dataItemsQueue.Count > 0 || runningTasks > 0)
        {
            if (dataItemsQueue.Count > 0)
            {

                await semaphore.WaitAsync(); // Wait if more than 5 tasks are already running

                ImportCode importCode;
                if (dataItemsQueue.TryDequeue(out importCode))
                {
                    Interlocked.Increment(ref runningTasks);

                    var task = Task.Run(async () =>
                    {
                        var importProcessor = new ImportProcessor();
                        var resultTuple = await importProcessor.ImportData(importCode);
                        bool result = resultTuple.Item1;
                        int seconds = resultTuple.Item2;

                        if (result)
                        {
                            successCount++;
                            successSeconds += seconds;
                            Console.WriteLine($"{importCode.Id} ***SUCCESS Code: {importCode.Code}");
                        }
                        else
                        {
                            failCount++;
                            failSeconds += seconds;
                            Console.WriteLine($"{importCode.Id} ---ENQUEUED Code: {importCode.Code}");
                            dataItemsQueue.Enqueue(importCode);
                        }
                    })
                    .ContinueWith(t =>
                    {
                        semaphore.Release(); // Release semaphore after task completion
                        Interlocked.Decrement(ref runningTasks);
                    });


                    tasks.Add(task);
                }
            }
        }

        await Task.WhenAll(tasks); // Wait for all tasks to complete
        Console.WriteLine("End of Import-Data job.");
        Console.WriteLine("");
        Console.WriteLine($"Success Count: {successCount}, Fail Count: {failCount}");
        Console.WriteLine($"Success Seconds: {successSeconds}, Fail Seconds: {failSeconds}");

    }


});




    //if (o.RunImportDataXXX)
    //{
    //    Console.WriteLine($"Starting Import-Data job.");
    //    var dataItemsQueue = DataAccessService.GenerateRandomImportCodes(100);


    //    while (dataItemsQueue.Count > 0)
    //    {
    //        var importCode = dataItemsQueue.Dequeue();

    //        var importProcessor = new ImportProcessor();
    //        //* The code will be passed to the ImportData method where an API call will be made to retreive the data and and save it the the DB.
    //        bool result = await importProcessor.ImportData(importCode);

    //        if (result)
    //        {
    //            Console.WriteLine($"Id: {importCode.Id}, Code: {importCode.Code} has been imported");
    //        }
    //        else
    //        {
    //            dataItemsQueue.Enqueue(importCode);
    //        }

    //    }

    //    Console.WriteLine($"End of Import-Data job.");
    //}




    //int iteration = 0;

    // If it's the 9th iteration, move the current item to the end of the queue
    //if (iteration % 9 == 8)
    //{
    //    var skippedItem = dataItemsQueue.Dequeue();
    //    dataItemsQueue.Enqueue(skippedItem);
    //    iteration++;
    //    continue;
    //}


    //iteration++;

