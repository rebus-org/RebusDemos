using System;

namespace Common
{
    public static class IdGenerator
    {
        public static string NewId(string baseName)
        {
            var now = DateTime.Now;
            return $"{baseName}/{now:yyyyMMdd}/{now:HHmmss}";
        }
    }
}