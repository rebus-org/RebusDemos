using System;
using Microsoft.Owin.Hosting;

namespace TradeClearanceHouseApi
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start("http://localhost:30001", a => a.RunClearanceHouseApi()))
            {
                Console.WriteLine("Trade Clearance House API running - press ENTER to quit");
                Console.ReadLine();
            }
        }
    }
}
