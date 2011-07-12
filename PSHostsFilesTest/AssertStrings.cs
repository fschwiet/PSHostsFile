using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PowerAssert;

namespace PSHostsFilesTest
{
    class AssertStrings
    {
        public static void MatchDespiteNewlines(string a, string b)
        {
            a = a.Replace("\r\n", "\n");
            b = b.Replace("\r\n", "\n");
            PAssert.IsTrue(() => a == b);
        }
    }
}
