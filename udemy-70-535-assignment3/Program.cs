using System;
using System.IO;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Configuration;


class Program
{
    public static IConfiguration Configuration { get; set; }

    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        Configuration = builder.Build();

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Configuration["StorageConnectionString"]);
        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
        CloudTable table = tableClient.GetTableReference("Assignment3Table");


        table.CreateIfNotExistsAsync().GetAwaiter().GetResult();

        CarEntity newcar = new CarEntity("WBA3B5C5XDF595354");
        newcar.Make = "BMW";
        newcar.Model = "3 SERIES";
        newcar.Color = "Grey";

        TableOperation insertOperation = TableOperation.Insert(newcar);

        table.ExecuteAsync(insertOperation).GetAwaiter().GetResult();

    }


    class CarEntity : TableEntity
    {
        public CarEntity(string carvin)
        {
            this.PartitionKey = "CarEntity";
            this.RowKey = carvin.ToUpper();
        }

        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }

}
