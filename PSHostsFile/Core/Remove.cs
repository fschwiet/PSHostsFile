using System;
using System.IO;
using System.Linq;

namespace PSHostsFile.Core
{
    public class Remove : TransformOperation
    {
        public void RemoveFromFile(string hostName, string hostsPath)
        {
            TransformFile(hostsPath, lines => lines.Where(l =>
            {
                var match = HostsFileUtil.TryGetHostsFileEntry(l);

                if (match == null)
                    return true;
                else if (!match.Host.Equals(hostName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                else
                    return false;
            }));
        }
    }
}
