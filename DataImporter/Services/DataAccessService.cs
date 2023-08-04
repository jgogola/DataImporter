using DataImporter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Services;

public class DataAccessService
{

    public static Queue<ImportCode> GenerateRandomImportCodes(int count)
    {
        Random random = new Random();
        Queue<ImportCode> importCodes = new Queue<ImportCode>();

        for (int i = 0; i < count; i++)
        {
            ImportCode item = new ImportCode
            {
                Id = i + 1,
                Code = "X" + random.Next(1000, 9999)
            };

            importCodes.Enqueue(item);
        }

        return importCodes;
    }

}