using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace ImagePreloader
{
    class Program
    {
        const string BASE_URL = "https://img.scryfall.com/cards/large/en";
        const string FORMAT = ".jpg";

        static void Main(string[] args)
        {
            Console.WriteLine($"Currently implemented sets: {string.Join(", ", SetDefinitions.Sets.Keys)}");

            string code;

            do
            {
                Console.Write("Enter a valid set code: ");
                code = Console.ReadLine();
            } while (!SetDefinitions.Sets.ContainsKey(code.ToLower()));

            DownloadSet(code);
            Console.ReadKey();
        }

        static void DownloadSet(string code)
        {
            var defs = SetDefinitions.Sets[code.ToLower()];

            var dir = Directory.CreateDirectory(code.ToUpper()).FullName;

            using (var client = new WebClient())
            {
                foreach (var card in defs)
                {
                    Console.WriteLine($"Downloading ({card.Key}/{defs.Keys.Count}): {card.Value}");
                    client.DownloadFile($"{BASE_URL}/{code}/{card.Key}{FORMAT}", Path.Combine(dir, $"{card.Value}.full{FORMAT}"));
                    Thread.Sleep(50);
                }
            }

            Console.WriteLine("Done!");
        }
    }
}
