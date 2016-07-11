using System;
using System.IO;
using Axh.Fit.Endomondo.Models;

namespace Axh.Fit.Endomondo
{
    static class Program
    {
        public static int Main(string[] args)
        {
            // Parse command line args.
            var commandLineArgs = new CommandLineArgs();
            if (!CommandLine.Parser.Default.ParseArguments(args, commandLineArgs))
            {
                return 1;
            }

            using (var scraper = new EndomondoScraper(commandLineArgs.UserToken))
            {
                // Scrape user id from endoConfig.
                var userId = scraper.GetUserId();
                Console.WriteLine($"Found account id: {userId}");

                // Scrape history from API.
                var history = scraper.GetHistory(userId);
                Console.WriteLine($"Found {history.Data.Count} workouts.");

                var format = commandLineArgs.TcxFileFormat ? "tcx" : "gpx";

                // Scrape workouts.
                Directory.CreateDirectory(format);
                foreach (var data in history.Data)
                {
                    var file = format + Path.DirectorySeparatorChar + data.Id + "." + format;
                    Console.Write($"Saving workout: {data.LocalStartTime} -> {file}");

                    if (File.Exists(file))
                    {
                        Console.WriteLine(" -> already exists.");
                        continue;
                    }

                    var workout = scraper.GetWorkout(userId, data.Id, format);
                    File.WriteAllBytes(file, workout);
                    Console.WriteLine($" -> {workout.Length} bytes");
                }
            }

            return 0;
        }
        
    }


}
