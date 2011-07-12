using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PSHostsFiles;

namespace PSHostsFilesTest
{
    [TestFixture]
    public class AddTest : ReadWriteScenario
    {
        [Test]
        public void can_set_host_to_empty_file()
        {
            var address = "192.168.0.12";
            var hostName = "someserver.net";

            var hostsFile = GetFileWithContents(@"", Encoding.UTF8);

            var sut = new Add();

            sut.AddToFile(hostName, address, hostsFile);

            Encoding ignored;
            var result = ReadFileContents(hostsFile, out ignored);

            Assert.That(result, Is.EqualTo("192.168.0.12\t\tsomeserver.net\r\n"));
        }

        [Test]
        public void can_set_host_to_non_empty_file()
        {
            var address = "192.168.0.12";
            var hostName = "someserver.net";

            var hostsFile = GetFileWithContents(@"
#line 1 meh

127.0.0.1           someserver.net

", Encoding.UTF8);

            var sut = new Add();

            sut.AddToFile(hostName, address, hostsFile);

            Encoding ignored;
            var result = ReadFileContents(hostsFile, out ignored);

            AssertStrings.MatchDespiteNewlines(result, @"
#line 1 meh

192.168.0.12" + "\t\t" + @"someserver.net
127.0.0.1           someserver.net

");
        }

    }
}
