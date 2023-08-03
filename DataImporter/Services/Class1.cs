using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Services
{
    public class Class1
    {
        public static async Task Run()
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
    }
}
