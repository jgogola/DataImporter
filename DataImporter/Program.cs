// See https://aka.ms/new-console-template for more information

using CommandLine;
using DataImporter;
using DataImporter.Services;


static async Task Main(string[] args)
{

    Console.WriteLine("Starting.");
    await Parser.Default.ParseArguments<Options>(args)
        .WithParsedAsync<Options>(async o =>
        {

        if (o.RunImportData)
        {
            Console.WriteLine("Starting Import-Data job.");
            var dataItemsQueue = DataAccessService.GenerateRandomImportCodes(10);

            var semaphore = new SemaphoreSlim(5); // Limiting concurrent tasks to 5
            var tasks = new List<Task>();

            while (dataItemsQueue.Count > 0)
            {

                await semaphore.WaitAsync(); // Wait if more than 5 tasks are already running

                var importCode = dataItemsQueue.Dequeue();

                var task = Task.Run(async () =>
                {
                    var importProcessor = new ImportProcessor();
                    bool result = await importProcessor.ImportData(importCode);

                    if (result)
                    {
                        Console.WriteLine($"Id: {importCode.Id}, Code: {importCode.Code} has been imported");
                    }
                    else
                    {
                        Console.WriteLine($"Id: {importCode.Id}, Code: {importCode.Code} failed. Enqueued.");
                        dataItemsQueue.Enqueue(importCode);
                    }
                })
                .ContinueWith(t => semaphore.Release()); // Release semaphore after task completion

                tasks.Add(task);
            }

            await Task.WhenAll(tasks); // Wait for all tasks to complete

            Console.WriteLine("End of Import-Data job.");
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

}