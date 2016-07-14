using System;
using System.IO;
using System.Linq;
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
                if (!userId.HasValue)
                {
                    Console.Error.WriteLine("Cannot get Endomondo user id. Is your USER_TOKEN still valid? Try again with a new one.");
                    return 1;
                }

                Console.WriteLine($"Found account id: {userId}");

                // Scrape history from API.
                var history = scraper.GetHistory(userId.Value);
                if (history?.Data == null || !history.Data.Any())
                {
                    Console.Error.WriteLine("No workout history found.");
                    return 1;
                }

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

                    var workout = scraper.GetWorkout(userId.Value, data.Id, format);
                    File.WriteAllBytes(file, workout);
                    Console.WriteLine($" -> {workout.Length} bytes");
                }
            }

            return 0;
        }
        
    }


}
