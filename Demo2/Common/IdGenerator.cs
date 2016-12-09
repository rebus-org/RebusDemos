using System;
using System.Collections.Concurrent;

namespace Common
{
    public static class IdGenerator
    {
        static readonly ConcurrentDictionary<string, int> Ids = new ConcurrentDictionary<string, int>();

        public static string NewId(string baseName)
        {
            var now = DateTime.Now;
            var id = $"{baseName}/{now:yyyyMMdd}/{now:HHmmss}";
            var number = Ids.AddOrUpdate(id, 1, (_, n) => n + 1);
            return $"{id}/{number}";
        }
    }
}