using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Axh.Fit.Endomondo.Models;
using CommandLine;
using Humanizer;

namespace Axh.Fit.Endomondo
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Parse command line args.
            switch (Parser.Default.ParseArguments<CommandLineArgs>(args))
            {
                case Parsed<CommandLineArgs> parsed:
                    return await RunAsync(parsed.Value) ? 0 : 1;

                case NotParsed<CommandLineArgs> _:
                    return 1;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static async Task<bool> RunAsync(CommandLineArgs args)
        {
            using (var scraper = new EndomondoScraper(args.UserToken))
            {
                // Scrape user id from endoConfig.
                var userId = await scraper.GetUserIdAsync();
                if (!userId.HasValue)
                {
                    Console.Error.WriteLine("Cannot get Endomondo user id. Is your USER_TOKEN still valid? Try again with a new one.");
                    return false;
                }

                Console.WriteLine($"Found account id: {userId}");

                // Scrape history from API.
                var history = await scraper.GetHistoryAsync(userId.Value);
                if (history?.Data == null || !history.Data.Any())
                {
                    Console.Error.WriteLine("No workout history found.");
                    return false;
                }

                Console.WriteLine($"Found {history.Data.Count} workouts.");
                
                // Slowly, synchronously scrape workouts. Not doing any parallel work as seems to trigger 429 responses.
                foreach (var item in history.Data)
                {
                    foreach (var format in args.GetFormats())
                    {
                        var fileName = $"{item.Id}.{format}";

                        Console.Write($"Saving workout: {item.LocalStartTime} -> {fileName} -> ");

                        if (File.Exists(fileName))
                        {
                            Console.WriteLine("already exists.");
                            continue;
                        }

                        using (var data = await scraper.GetWorkout(userId.Value, item.Id, format))
                        using (var file = File.OpenWrite(fileName))
                        {
                            await data.Stream.CopyToAsync(file);
                            Console.WriteLine(data.Length.Bytes());
                        }
                    }
                }
            }

            return true;
        }

    }
}
