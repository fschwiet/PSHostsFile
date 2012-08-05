using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PSHostsFile.Core
{
    public class Remove : TransformOperation
    {
        public void RemoveFromFile(string target, string hostsPath)
        {
            TransformFile(hostsPath, GetRemoveTransformForHost(target));
        }

        public void RemoveFromFile(Regex pattern, string hostsPath)
        {
            TransformFile(hostsPath, GetRemoveTransform(host => pattern.Match(host).Success));
        }

        public static Func<IEnumerable<string>, IEnumerable<string>> GetRemoveTransformForHost(string hostname)
        {
            return GetRemoveTransform(host => host.Equals(hostname, StringComparison.InvariantCultureIgnoreCase));
        }

        private static Func<IEnumerable<string>, IEnumerable<string>> GetRemoveTransform(Func<string, bool> doHostsMatch)
        {
            return lines => lines.Where(l =>
            {
                var match = HostsFileUtil.TryGetHostsFileEntry(l);

                if (match == null)
                    return true;

                var matchedHost = match.Hostname;
                if (!doHostsMatch(matchedHost))
                    return true;
                else
                    return false;
            });
        }
    }
}
