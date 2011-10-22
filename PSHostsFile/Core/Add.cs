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
            new Remove().RemoveFromFile(hostName, hostsFile);

            TransformFile(hostsFile, lines => Transform(lines.ToArray(), hostName, address));
        }

        private List<string> Transform(string[] contents, string hostName, string address)
        {
            List<string> result = new List<string>();

            int index = 0;

            while(index < contents.Length && !HostsFileUtil.IsLineAHostFilesEntry(contents[index]))
            {
                result.Add(contents[index]);
                index++;
            }

            result.Add(address + "\t\t" + hostName);

            while (index < contents.Length)
            {
                result.Add(contents[index]);
                index++;
            }
            return result;
        }
    }
}
