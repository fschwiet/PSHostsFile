using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSHostsFile.Core;

namespace PSHostsFile
{
    public class HostsFile
    {
        public static IEnumerable<HostsFileEntry> Get()
        {
            return new Get().LoadFromHostsFiles();
        }

        public static void Set(string hostName, string address)
        {
            var hostsPath = Core.Get.GetHostsPath();

            new Remove().RemoveFromFile(hostName, hostsPath);
            new Add().AddToFile(hostName, address, hostsPath);
        }

        public static void Remove(string hostName)
        {
            var hostsPath = Core.Get.GetHostsPath();

            new Remove().RemoveFromFile(hostName, hostsPath);
        }
    }
}
