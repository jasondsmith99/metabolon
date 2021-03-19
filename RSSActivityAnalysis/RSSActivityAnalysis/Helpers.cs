using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSActivityAnalysis
{
    internal static class Helpers
    {
        public static DateTimeOffset Max(DateTimeOffset x, DateTimeOffset y)
            => x > y ? x : y;
    }
}
