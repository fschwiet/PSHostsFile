using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PSHostsFiles;

namespace PSHostsFilesTest
{
    // use environment variable  SystemRoot=C:\Windows
    [TestFixture]
    class GetTest
    {
        [Test]
        public void can_read_hosts_file()
        {
            string hostsFile =
                @"
# Copyright (c) 1993-2009 Microsoft Corp.
#
# This is a sample HOSTS file used by Microsoft TCP/IP for Windows.
#
# This file contains the mappings of IP addresses to host names. Each
# entry should be kept on an individual line. The IP address should
# be placed in the first column followed by the corresponding host name.
# The IP address and the host name should be separated by at least one
# space.
#
# Additionally, comments (such as these) may be inserted on individual
# lines or following the machine name denoted by a '#' symbol.
#
# For example:
#
#      102.54.94.97     rhino.acme.com          # source server
#       38.25.63.10     x.acme.com              # x client host

# localhost name resolution is handled within DNS itself.
#	127.0.0.1       localhost
#	::1             localhost

127.0.0.1           www.testserver.com
192.168.1.1         anotherserver.net

";
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(hostsFile));

            var sut = new Get();

            var results = sut.LoadFromStream(ms);

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