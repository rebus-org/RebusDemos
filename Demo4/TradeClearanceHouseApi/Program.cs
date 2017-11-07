using System;
using Microsoft.Owin.Hosting;

namespace TradeClearanceHouseApi
{
    class Program
    {
        static void Main()
        {
            var settings = new ClearanceHouseSettings();

            using (WebApp.Start("http://localhost:30001", a => a.RunClearanceHouseApi(settings)))
            {
                Console.WriteLine("Trade Clearance House API running - press ENTER to quit (M to toggle mode)");

                while (true)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Enter) break;

                    if (char.ToLowerInvariant(key.KeyChar) == 'm')
                    {
                        settings.Toggle();

                        Console.WriteLine($"Clearance House mode: {settings.Mode}");
                    }
                }
            }
        }
    }
}
