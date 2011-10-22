using System.Collections.Generic;
using System.IO;

namespace PSHostsFile.Core
{
    public class Add
    {
        public void AddToFile(string hostName, string address, string hostsFile)
        {
            new Remove().RemoveFromFile(hostName, hostsFile);

            var contents = File.ReadAllLines(hostsFile);

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

            File.WriteAllLines(hostsFile, result.ToArray());
        }
    }
}
