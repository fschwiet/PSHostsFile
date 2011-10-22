using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PSHostsFile.Core
{
    public class Remove : TransformOperation
    {
        public void RemoveFromFile(string target, string hostsPath)
        {
            RemoveFromFile(hostsPath,
                host => host.Equals(target, StringComparison.InvariantCultureIgnoreCase));
        }

        public void RemoveFromFile(Regex pattern, string hostsPath)
        {
            RemoveFromFile(hostsPath,
                host => pattern.Match(host).Success);
        }

        private void RemoveFromFile(string hostsPath, Func<string, bool> doHostsMatch)
        {
            TransformFile(hostsPath, lines => lines.Where(l =>
            {
                var match = HostsFileUtil.TryGetHostsFileEntry(l);

                if (match == null)
                    return true;

                var matchedHost = match.Host;
                if (!doHostsMatch(matchedHost))
                    return true;
                else
                    return false;
            }));
        }
    }
}
