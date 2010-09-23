using System.IO;
using System.Text.RegularExpressions;

namespace PSHostsFiles
{
    public class HostsFileUtil
    {
        static public bool IsLineAHostFilesEntry(string line)
        {
            if (line.Trim().Length == 0)
                return false;
            else if (line.TrimStart().StartsWith("#"))
                return false;
            else
                return true;
        }

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

        static Regex RegexHostsEntry = new Regex(@"^\s*(?<address>\S+)\s+(?<name>\S+)\s*($|#)", RegexOptions.Compiled);
    }
}