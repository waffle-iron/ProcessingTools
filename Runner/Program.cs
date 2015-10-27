﻿namespace Runner
{
    using System;
    using System.Data.Entity;
    using Newtonsoft.Json;
    using ProcessingTools.MimeResolver.Context;
    using ProcessingTools.MimeResolver.Migrations;
    using System.IO;
    using ProcessingTools.MimeResolver.Models.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            string jsonFilePath = "Data/mime.json";
            string jsonString = File.ReadAllText(jsonFilePath);

            var json = JsonConvert.DeserializeObject<ExtensionJson[]>(jsonString);

           

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MimeTypesDbContext, Configuration>());

            var db = new MimeTypesDbContext();
            db.Database.CreateIfNotExists();

            var fileExtensionsInDb = new HashSet<string>(db.FileExtensions
                .Select(e => e.Name)
                .ToList());

            var fileExtensionsInJson = new HashSet<string>(json.Select(j => j.Extension));

            var mimeTypesInDb = new HashSet<string>(db.MimeTypes
                .Select(m => m.Name)
                .ToList());

            var mimeTypesInJson = new HashSet<string>(json.Select(j => j.MimeType));

            var mimeSubtyesInDb = new HashSet<string>(db.MimeSubtypes
                .Select(m => m.Name)
                .ToList());

            var mimeSubtypesInJson = new HashSet<string>(json.Select(j => j.MimeSubtype));

            Console.WriteLine(fileExtensionsInJson.Count);
            Console.WriteLine(mimeTypesInJson.Count);
            Console.WriteLine(mimeSubtypesInJson.Count);

            db.SaveChanges();
            db.Dispose();
        }
    }
}
