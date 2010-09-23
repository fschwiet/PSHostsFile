using System.IO;
using System.Text.RegularExpressions;

namespace PSHostsFiles
{
    public class HostsFileEntryRegex
    {
        static public HostsFileEntry GetHostsFileEntry(string line)
        {
            var result = TryGetHostsFileEntry(line);

            if (result == null)
                throw new InvalidDataException();

            return result;
        }

        static public HostsFileEntry TryGetHostsFileEntry(string line)
        {
            var match = RegexHostsEntry.Match(line);

            if (!match.Success)
                return null;

            return new HostsFileEntry()
                {
                    Address =  match.Groups["address"].Value,
                    Host = match.Groups["name"].Value
                };
        }

        static Regex RegexHostsEntry = new Regex(@"\s*(?<address>\S+)\s+(?<name>\S+)\s*", RegexOptions.Compiled);
    }
}