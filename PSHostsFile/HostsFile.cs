using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PSHostsFile.Core;

namespace PSHostsFile
{
    public class HostsFile
    {
        public static IEnumerable<HostsFileEntry> Get()
        {
            return new Get().LoadFromHostsFiles(GetHostsPath());
        }

        public static void Set(string hostName, string ipAddress)
        {
            var hostsPath = GetHostsPath();

            new Add().AddToFile(hostName, ipAddress, hostsPath);
        }

        public static void Remove(string hostName)
        {
            var hostsPath = GetHostsPath();

            new Remove().RemoveFromFile(hostName, hostsPath);
        }

        public static void Remove(Regex pattern)
        {
            var hostsPath = GetHostsPath();

            new Remove().RemoveFromFile(pattern, hostsPath);
        }

        public static string GetHostsPath()
        {
            var systemPath = Environment.GetEnvironmentVariable("SystemRoot");
            var hostsPath = Path.Combine(systemPath, "system32\\drivers\\etc\\hosts");

            if (!File.Exists(hostsPath))
                throw new FileNotFoundException("Hosts file not found at expected location.");
            return hostsPath;
        }
    }
}
