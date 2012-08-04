using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSHostsFile.Core
{
    public class Add : TransformOperation
    {
        public void AddToFile(string hostName, string address, string hostsFile)
        {
            List<Func<IEnumerable<string>, IEnumerable<string>>> transforms = new List<Func<IEnumerable<string>, IEnumerable<string>>>();
            
            transforms.Add(Remove.GetRemoveTransformForHost(hostName));
            transforms.Add(lines => Transform(lines.ToArray(), hostName, address));

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
