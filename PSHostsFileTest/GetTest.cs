using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PSHostsFile;
using PSHostsFile.Core;

namespace PSHostsFileTest
{
    [TestFixture]
    class GetTest
    {
        [Test]
        public void can_read_hosts_file()
        {
            var hostsFile = SampleHostsFile.AsFile();

            var sut = new Get();

            var results = sut.LoadFromHostsFiles(hostsFile);

            Assert.True(new KellermanSoftware.CompareNetObjects.CompareObjects().Compare(
                results.ToArray(), 
                new[] {
                    new HostsFileEntry()
                        {
                            Address = "127.0.0.1",
                            Host = "www.testserver.com"
                        },
                    new HostsFileEntry()
                        {
                            Address = "192.168.1.1",
                            Host = "anotherserver.net"
                        }
                }));
        }
    }
}