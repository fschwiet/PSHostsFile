using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PowerAssert;

namespace PSHostsFileTest
{
    class AssertFile
    {
        public static void MatchesIgnoringNewlines(string hostsFile, string expectedString)
        {
            string a = File.ReadAllText(hostsFile);
            string b = expectedString;
            a = a.Replace("\r\n", "\n");
            b = b.Replace("\r\n", "\n");
            PAssert.IsTrue(() => a == b);
        }
    }
}
