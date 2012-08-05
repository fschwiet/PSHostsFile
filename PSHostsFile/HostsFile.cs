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
        public static IEnumerable<HostsFileEntry> Get(string filepath = null)
        {
            filepath = filepath ?? GetHostsPath();
            
            var lines = File.ReadAllLines(filepath);

            return lines
                .Where(l => HostsFileUtil.IsLineAHostFilesEntry(l))
                .Select(l => HostsFileUtil.GetHostsFileEntry(l));
        }

        public static void Set(string hostName, string ipAddress, string filepath = null)
        {
            filepath = filepath ?? GetHostsPath();

            List<Func<IEnumerable<string>, IEnumerable<string>>> transforms = new List<Func<IEnumerable<string>, IEnumerable<string>>>();
            
            foreach(var entry in new HostsFile.Entry[] { new HostsFile.Entry(hostName, ipAddress)})
            {
                transforms.Add(Core.Remove.GetRemoveTransformForHost(entry.Hostname));
                transforms.Add(lines => GetSetHostTransform(lines.ToArray(), entry.Hostname, entry.Address));
            }

            TransformOperation.TransformFile(filepath, transforms.ToArray());
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

        public class Entry
        {
            public string Hostname;
            public string Address;

            public Entry(string hostname, string address)
            {
                Hostname = hostname;
                Address = address;
            }
        }

        public static IEnumerable<string> GetSetHostTransform(IEnumerable<string> contents, string hostName, string address)
        {
            List<string> result = new List<string>();

            var needsInsert = true;

            foreach(var line in contents)
            {
                if (!HostsFileUtil.IsLineAHostFilesEntry(line))
                {
                    result.Add(line);
                    continue;
                }
 
                if (needsInsert)
                {
                    result.Add(GetHostLine(hostName, address));
                    needsInsert = false;
                }
                result.Add(line);
            }

            if (needsInsert)
            {
                result.Add(GetHostLine(hostName, address));
                needsInsert = false;
            }

            return result;
        }

        public static string GetHostLine(string hostName, string address)
        {
            return address + "\t\t" + hostName;
        }
    }
}
