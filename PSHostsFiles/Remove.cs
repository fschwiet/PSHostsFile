using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PSHostsFiles
{
    public class Remove : TransformOperation
    {
        public void RemoveFromStream(string serverName, StreamReader hostsFile, StreamWriter results)
        {
            string line;

            while ((line = hostsFile.ReadLine()) != null)
            {
                var match = HostsFileUtil.TryGetHostsFileEntry(line);

                if (match == null)
                    results.WriteLine(line);
                else if (!match.Host.Equals(serverName, StringComparison.InvariantCultureIgnoreCase))
                    results.WriteLine(line);
            }

            results.Flush();
        }

        public void RemoveFromFile(string hostName, string hostsPath)
        {
            ApplyStreamTransform(hostsPath, (r, w) => this.RemoveFromStream(hostName, r, w));
        }
    }
}
