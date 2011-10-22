using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSHostsFile.Core
{
    public class Get
    {
        public IEnumerable<HostsFileEntry> LoadFromHostsFiles(string hostsFile)
        {
            var lines = File.ReadAllLines(hostsFile);

            return lines
                .Where(l => HostsFileUtil.IsLineAHostFilesEntry(l))
                .Select(l => HostsFileUtil.GetHostsFileEntry(l));
        }
    }
}