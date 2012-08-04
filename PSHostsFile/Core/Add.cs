using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSHostsFile.Core
{
    public class Add : TransformOperation
    {
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

        public void AddToFile(string hostsFile, params Entry[] entries)
        {
            List<Func<IEnumerable<string>, IEnumerable<string>>> transforms = new List<Func<IEnumerable<string>, IEnumerable<string>>>();
            
            foreach(var entry in entries)
            {
                transforms.Add(Remove.GetRemoveTransformForHost(entry.Hostname));
                transforms.Add(lines => Transform(lines.ToArray(), entry.Hostname, entry.Address));
            }

            TransformFile(hostsFile, transforms.ToArray());
        }

        private IEnumerable<string> Transform(IEnumerable<string> contents, string hostName, string address)
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

        private static string GetHostLine(string hostName, string address)
        {
            return address + "\t\t" + hostName;
        }
    }
}
